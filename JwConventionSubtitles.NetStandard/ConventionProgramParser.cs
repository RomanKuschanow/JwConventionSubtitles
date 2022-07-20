using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace JwConventionSubtitles;

public class ConventionProgramParser
{
    public IEnumerable<string> Parse(List<string> lines)
    {
        List<string> speeches = new List<string>();

        Regex speechRegex = new Regex(@"^(\d{1,2}:\d{2}) (?!Song|Пісня|Music-Video Presentation|Музичний відеоролик|СЕРІЯ ПРОМОВ\.|SYMPOSIUM:)([^(]*(?!\())");
        Regex symposiumHeadRegex = new Regex(@"^(\d{1,2}:\d{2}) (?:СЕРІЯ ПРОМОВ\.|SYMPOSIUM:) (.*)");
        Regex symposiumSpeechRegex = new Regex(@"^(•) ([^(]*(?!\())");

        string currSymposiumName = "";

        for (int i = 0; i < lines.Count; i++)
        {
            MatchCollection speechMatches = speechRegex.Matches(lines[i]);
            MatchCollection symposiumHeadMatches = symposiumHeadRegex.Matches(lines[i]);
            MatchCollection symposiumSpeechMatches = symposiumSpeechRegex.Matches(lines[i]);

            if (speechMatches.Count > 0)
            {
                currSymposiumName = "";
                string name = speechMatches[0].Groups[2].Value;
                var regex = new Regex(@"[A-Z’ ]*(?=\:)\: |[“”«»]");
                speeches.Add(regex.Replace(name, ""));
            }
            else if (symposiumHeadMatches.Count > 0)
            {
                currSymposiumName = symposiumHeadMatches[0].Groups[2].Value;
            }
            else if (symposiumSpeechMatches.Count > 0)
            {
                speeches.Add($"{currSymposiumName}: {symposiumSpeechMatches[0].Groups[2].Value}");
            }
        }

        return speeches;
    }
}
