using System;
using System.Collections.Generic;
using System.Text;

namespace JwConventionSubtitles
{
    public class FileReader : IFileReader
    {
        public IEnumerable<string> ReadLines(string path)
        {
            List<string> strings = new List<string>();



            return strings;
        }

        public string ReadText(string path)
        {
            return "";
        }
    }
}
