using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace JwConventionSubtitles;

public class ConventionProgramParser
{
    public IEnumerable<SpeechFromProgram> Parse(List<string> lines)
    {
        List<SpeechFromProgram> speeches = new List<SpeechFromProgram>();

        Regex speechRegex = new Regex(@"^(\d{1,2}:\d{2}) (?!Song|Пісня|Music-Video Presentation|Музичний відеоролик|СЕРІЯ ПРОМОВ\.|SYMPOSIUM:)([^(]*(?!\())");
        Regex symposiumHeadRegex = new Regex(@"^(\d{1,2}:\d{2}) (?:СЕРІЯ ПРОМОВ\.|SYMPOSIUM:) (.*)");
        Regex symposiumSpeechRegex = new Regex(@"^(•) ([^(]*(?!\())");

        string currSymposiumName = "";
        int symposiumSpeechesCount = 0;
        int currSymposiumSpeech = 0;
        TimeSpan symposiumStartTime = new();
        TimeSpan symposiumEndTime = new();

        for (int i = 0; i < lines.Count; i++)
        {
            MatchCollection speechMatches = speechRegex.Matches(lines[i]);
            MatchCollection symposiumHeadMatches = symposiumHeadRegex.Matches(lines[i]);
            MatchCollection symposiumSpeechMatches = symposiumSpeechRegex.Matches(lines[i]);

            if (speechMatches.Count > 0)
            {
                currSymposiumName = "";
                symposiumSpeechesCount = 0;
                currSymposiumSpeech = 0;
                string name = speechMatches[0].Groups[2].Value;
                var regex = new Regex(@"[A-ZА-ЯІЇЄ’ ]+(?=\:|\.)[\:|\.] |[“”«»]");
                TimeSpan endTime;
                Regex timeRegex = new Regex(@"^(\d{1,2}:\d{2})");
                for (int j = i + 1; j < lines.Count; j++)
                {
                    MatchCollection timeMatches = timeRegex.Matches(lines[j]);

                    if (timeMatches.Count > 0)
                    {
                        endTime = TimeSpan.Parse(timeMatches[0].Value);
                        break;
                    }
                }
                speeches.Add(new SpeechFromProgram(false, TimeSpan.Parse(speechMatches[0].Groups[1].Value), endTime, regex.Replace(name, "")));
            }
            else if (symposiumHeadMatches.Count > 0)
            {
                currSymposiumName = symposiumHeadMatches[0].Groups[2].Value;
                symposiumSpeechesCount = 0;
                currSymposiumSpeech = 0;
                symposiumStartTime = TimeSpan.Parse(symposiumHeadMatches[0].Groups[1].Value);
                Regex timeRegex = new Regex(@"^(\d{1,2}:\d{2})");
                for (int j = i + 1; j < lines.Count; j++)
                {
                    MatchCollection timeMatches = timeRegex.Matches(lines[j]);
                    symposiumSpeechMatches = symposiumSpeechRegex.Matches(lines[j]);

                    if (symposiumSpeechMatches.Count > 0)
                    {
                        symposiumSpeechesCount++;
                    }
                    else if (timeMatches.Count > 0)
                    {
                        symposiumEndTime = TimeSpan.Parse(timeMatches[0].Value);
                        break;
                    }
                }
            }
            else if (symposiumSpeechMatches.Count > 0 && symposiumSpeechesCount > 0)
            {
                currSymposiumSpeech++;
                TimeSpan startTime = TimeSpan.FromMinutes((symposiumEndTime - symposiumStartTime).TotalMinutes / symposiumSpeechesCount * (currSymposiumSpeech - 1));
                TimeSpan endTime;
                if (currSymposiumSpeech == symposiumSpeechesCount)
                {
                    Regex timeRegex = new Regex(@"^(\d{1,2}:\d{2})");
                    for (int j = i + 1; j < lines.Count; j++)
                    {
                        MatchCollection timeMatches = timeRegex.Matches(lines[j]);

                        if (timeMatches.Count > 0)
                        {
                            endTime = TimeSpan.Parse(timeMatches[0].Value);
                            break;
                        }
                    }
                }
                else
                {
                    endTime = symposiumStartTime.Add(TimeSpan.FromMinutes((symposiumEndTime - symposiumStartTime).TotalMinutes / symposiumSpeechesCount * (currSymposiumSpeech)));
                }
                speeches.Add(new SpeechFromProgram(true, symposiumStartTime.Add(startTime), endTime, $"{symposiumSpeechMatches[0].Groups[2].Value}"));
            }
        }

        return speeches;
    }
}
