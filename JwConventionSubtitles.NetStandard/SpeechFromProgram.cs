using System;
using System.Collections.Generic;
using System.Text;

namespace JwConventionSubtitles;

public record SpeechFromProgram(bool IsSymposium, TimeSpan StartTime, TimeSpan EndTime, string Name);
