using System.Collections.Generic;

namespace JwConventionSubtitles
{
    public interface IFileReader
    {
        IEnumerable<string> ReadLines(string path);
    }
}
