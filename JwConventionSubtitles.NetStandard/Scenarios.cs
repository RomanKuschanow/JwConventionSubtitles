using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JwConventionSubtitles
{
    internal class Scenarios
    {
        public static async Task Scenario1(string filePath, string programPath)
        {
            var fileReader = new FileReader();
            var parser = new VttParser();
            var programParser = new ConventionProgramParser();
            var speechConverter = new SpeechConverter();

            var lines = fileReader.ReadLines(filePath);
            var frames = parser.Parse(lines.Result.ToList());

            var conventionPrograms = fileReader.ReadLines(programPath);
            var speechesFromProgram = programParser.Parse(conventionPrograms.Result.ToList());

            //var speeches = speechConverter.Convert(frames, speechesFromProgram);
        }
    }
}
