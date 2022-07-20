using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace JwConventionSubtitles;

public class SpeechConverter
{
    public IEnumerable<SpeechWithText> Convert(List<VttFrame> frames, List<string> speeches)
    {
        List<SpeechWithText> speechesWithText = new List<SpeechWithText>();

        string text = "";
        string name = "";
        bool speechStart = false;

        foreach (VttFrame frame in frames)
        {
            foreach (string line in frame.Lines)
            {
                foreach (string speech in speeches)
                {
                    var regex = new Regex(@"[A-ZА-ЯІЇЄ’ ]+(?=\:|\.)[\:|\.] |[“”«»]");

                    if (regex.Replace(line, "").Contains(speech))
                    {
                        name = speech;
                        text = "";
                        if (speechStart)
                        {
                            speechesWithText.Add(new SpeechWithText(name, text));
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
