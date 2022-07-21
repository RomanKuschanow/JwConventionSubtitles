using System.Collections.Generic;
using System.Threading.Tasks;

namespace JwConventionSubtitles
{
    public interface IFileReader
    {
       Task<IEnumerable<string>> ReadLines(string path);
    }
}
