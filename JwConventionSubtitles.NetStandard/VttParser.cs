using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;

namespace JwConventionSubtitles;

public class VttParser
{
    public List<VttFrame> Frames { get; private set; }

    public VttParser()
    {
    }

    public StreamReader LoadSubtitles(string path)
    {
        Regex regex = new Regex(@"\d{2}:\d{2}:\d{2}\.\d{3}");
        StreamReader sr = new StreamReader(path);
        string? line;


        while ((line = sr.ReadLine()) != null)
        {
            MatchCollection matches = regex.Matches(line);

            if (matches.Count > 0)
            {

            }

            continue;
        }

        return sr;
    }
}
