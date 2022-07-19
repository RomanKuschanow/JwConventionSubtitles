using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JwConventionSubtitles
{
    internal class Scenarios
    {
        public static async Task Scenario1()
        {
            var fileReader = new FileReader();
            var parser = new VttParser();
            //var speechConverter = new SpeechConverter();

            var lines = fileReader.ReadLines(@"C:\CO-r22_E_01.vtt");
            var frames = parser.Parse(lines.ToList());

            string conventionProgramString = fileReader.ReadText(@"");
            //List<SpeechFromProgram> speechesFromProgram = new ConventionProgramParser(conventionProgramString);

            //List<SpeechWithText> speeches = speechConverter.Convert(frames, speechesFromProgram);
        }
    }
}
