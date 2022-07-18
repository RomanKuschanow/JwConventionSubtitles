using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;

namespace JwConventionSubtitles;

public class VttParser
{
    public IEnumerable<VttFrame> Parse(IEnumerable<string> lines)
    {
        IEnumerable<VttFrame> frames = new List<VttFrame>();

        Regex regex = new Regex(@"\d{2}:\d{2}:\d{2}\.\d{3}");

        foreach (var line in lines)
        {
            MatchCollection matches = regex.Matches(line);

            if (matches.Count > 0)
            {

            }

            continue;
        }

        return frames;
    }
}
