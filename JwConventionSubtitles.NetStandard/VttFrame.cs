using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JwConventionSubtitles;

public class VttFrame
{
    public TimeSpan StartTime { get; set; }
    public TimeSpan EndTime { get; set; }
    public List<string> Text { get; set; }

    public VttFrame(TimeSpan start_time, TimeSpan end_time, List<string> text)
    {
        StartTime = start_time;
        EndTime = end_time;
        Text = text;
    }
}
