using System;
using System.Collections.Immutable;

namespace JwConventionSubtitles;

public record VttFrame(TimeSpan StartTime, TimeSpan EndTime, ImmutableArray<string> Lines);
