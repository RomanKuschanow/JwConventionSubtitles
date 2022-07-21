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
        bool speechStop = false;

        var regex = new Regex(@"[A-ZА-ЯІЇЄ’ ]+(?=\:|\.)[\:|\.] |[‘’“”«».]");
        var tegregex = new Regex(@"<.>|<\/.>");

        int currFrame = 0;

        for (int s = 0; s < speeches.Count; s++)
        {
            for (int i = currFrame; i < frames.Count; i++)
            {
                speechStop = false;
                string lines = String.Join(" ", frames[i].Lines);

                if ((regex.Replace(lines, "").Contains(regex.Replace(speeches[s].Name, "")) && !speechStart) || (s + 1 < speeches.Count && (regex.Replace(lines, "").Contains(regex.Replace(speeches[s + 1].Name, "")) && speechStart)) || (i + 1 == frames.Count && speechStart))
                {
                    currSpeech = speeches[s];
                    speechStartTime = frames[i].StartTime;
                    if (speechStart)
                    {
                        speechesWithText.Add(new SpeechWithText(currSpeech.Name, text));
                        text = "";
                        speechStop = true;
                        speechStart = false;
                    }
                    speechStart = true;
                }
                else if (speechStart)
                {
                    text += tegregex.Replace(lines, "") + " ";
                }
                if (speechStop)
                {
                    currFrame = i;
                    break;
                }
            }
            if (currFrame + 1 == frames.Count)
                break;
        }

        return speechesWithText;
    }
}
