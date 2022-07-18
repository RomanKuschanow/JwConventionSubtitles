using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;

namespace VTT_Reader;

public class VttParser
{
    private List<VttFrame> frames = new();
    public List<VttFrame> Frames { get => frames; set => frames = value; }


    public VttParser(string path)
    {
        Regex regex = new Regex(@"\d{2}:\d{2}:\d{2}\.\d{3}");
        using StreamReader sr = new StreamReader(path);
        string? line;


        while ((line = sr.ReadLine()) != null)
        {
            MatchCollection matches = regex.Matches(line);

            if (matches.Count > 0)
            {

            }

            continue;
        }
    }
}
