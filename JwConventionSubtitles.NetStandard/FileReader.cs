using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace JwConventionSubtitles;

public class FileReader : IFileReader
{
    public async Task<IEnumerable<string>> ReadLines(string path)
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

public class WebFileReader : IFileReader
{
    static readonly HttpClient client = new HttpClient();

    private async Task<string> GetText(string url)
    {
        string text = await client.GetStringAsync(url);

        return text;
    }

    public async Task<IEnumerable<string>> ReadLines(string url)
    {
        List<string> lines = new List<string>();

        string text = await GetText(url);

        lines = text.Split('\n').ToList();

        for (int i = 0; i < lines.Count; i++)
        {
            lines[i] = lines[i].Replace("\r", "");
        }

        return lines;
    }
}
