using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace JwConventionSubtitles;

public class SpeechConverter
{
    public IEnumerable<SpeechWithText> Convert(List<VttFrame> frames, List<SpeechFromProgram> speeches)
    {
        List<SpeechWithText> speechesWithText = new List<SpeechWithText>();

        string text = "";
        TimeSpan speechStartTime;
        SpeechFromProgram currSpeech = new SpeechFromProgram(false, TimeSpan.Zero, TimeSpan.Zero, "");
        bool speechStart = false;

        foreach (VttFrame frame in frames)
        {
            foreach (string line in frame.Lines)
            {
                foreach (var speech in speeches)
                {
                    var regex = new Regex(@"[A-ZА-ЯІЇЄ’ ]+(?=\:|\.)[\:|\.] |[“”«»]");

                    if (regex.Replace(line, "").Contains(speech.Name) || (speechStart && Math.Round((currSpeech.EndTime - currSpeech.StartTime).TotalMinutes) == Math.Round((frame.StartTime - speechStartTime).TotalMinutes)))
                    {
                        currSpeech = speech;
                        speechStartTime = frame.StartTime;
                        text = "";
                        if (speechStart)
                        {
                            speechesWithText.Add(new SpeechWithText(currSpeech.Name, text));
                            speechStart = false;
                            break;
                        }
                        speechStart = true;
                        break;
                    }
                    else if (speechStart)
                    {
                        text += " " + line;
                    }
                }
            }
        }

        return speechesWithText;
    }
}
