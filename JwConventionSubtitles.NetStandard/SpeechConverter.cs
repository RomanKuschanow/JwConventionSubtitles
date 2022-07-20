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

        int currFrame = 0;

        for (int s = 0; s < speeches.Count; s++)
        {
            for (int i = currFrame; i < frames.Count; i++)
            {
                speechStop = false;
                foreach (string line in frames[i].Lines)
                {
                    if (line == "—Love for God’s Word.”")
                        s = s;

                    if ((regex.Replace(line, "").Contains(regex.Replace(speeches[s].Name, "")) && !speechStart) || (s + 1 < speeches.Count && (regex.Replace(line, "").Contains(regex.Replace(speeches[s + 1].Name, "")) && speechStart)) || i + 1 == frames.Count)
                    {
                        currSpeech = speeches[s];
                        speechStartTime = frames[i].StartTime;
                        if (speechStart)
                        {
                            speechesWithText.Add(new SpeechWithText(currSpeech.Name, text));
                            text = "";
                            speechStop = true;
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
