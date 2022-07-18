using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using System;
using System.Collections.Immutable;

namespace JwConventionSubtitles;

public class VttParser
{
    public IEnumerable<VttFrame> Parse(List<string> lines)
    {
        List<VttFrame> frames = new List<VttFrame>();

        Regex regex = new Regex(@"\d{2}:\d{2}:\d{2}\.\d{3}");

        for (int i = 0; i < lines.Count; i++)
        {
            MatchCollection matches = regex.Matches(lines[i]);

            if (matches.Count > 0)
            {
                int j = i;

                while (lines[j] != "\n" && lines[j] != "\r\n")
                {
                    j++;
                    if (j >= lines.Count) break;
                }

                List<string> textLines = new();

                for (int k = 1; k < j - i; k++)
                {
                    textLines.Add(lines[i + k]);
                }

                frames.Add(new VttFrame(TimeSpan.Parse(matches[0].Value), TimeSpan.Parse(matches[1].Value), textLines.ToImmutableArray()));
            }

            continue;
        }

        return frames;
    }
}
