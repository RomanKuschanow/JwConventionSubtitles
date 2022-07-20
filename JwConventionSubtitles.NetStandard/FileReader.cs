using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace JwConventionSubtitles
{
    public class FileReader : IFileReader
    {
        public IEnumerable<string> ReadLines(string path)
        {
            List<string> lines = new List<string>();

            using StreamReader reader = new StreamReader(path);

            string? line;
            while ((line = reader.ReadLine()) != null)
            {
                lines.Add(line);
            }

            return lines;
        }
    }
}
