using System;
using System.Collections.Generic;
using System.Text;

namespace JwConventionSubtitles;

public class SpeechConverter
{
    public IEnumerable<string> Convert(List<VttFrame> frames, List<string> speeches)
    {
        List<string> speechesWithText = new List<string>();

        string speech = "";

        foreach (VttFrame frame in frames)
        {

        }

        return speechesWithText;
    }
}
