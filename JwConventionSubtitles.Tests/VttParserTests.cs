using JwConventionSubtitles;
using Moq;
using FluentAssertions;

namespace JwConventionSubtitles.Tests;

public class VttParserTests
{
    [Fact]
    public void WhenLinesEmpty_ThenFramesEmpty()
    {
        // Arrange
        var sut = new VttParser();
        var lines = new[] { 
            ""
        };

        // Act
        var frames = sut.Parse(lines);

        // Assert
        frames.Should().BeEmpty();
    }

    [Fact]
    public void WhenLinesHas1Frame_Then1Frame()
    {
        // Arrange
        var sut = new VttParser();
        var lines = new[] {
            "00:13:29.371 --> 00:13:31.373 line:90% position:50% align:center",
            "It is my great pleasure"
        };

        // Act
        var frames = sut.Parse(lines);

        // Assert
        frames.Should().NotBeEmpty();
    }

    [Fact]
    public void WhenLinesHas1Frame_ThenFrameHasCorrectProperties()
    {
        // Arrange
        var sut = new VttParser();
        var lines = new[] {
            "00:13:29.371 --> 00:13:31.373 line:90% position:50% align:center",
            "It is my great pleasure"
        };

        // Act
        var frames = sut.Parse(lines);

        // Assert
        var frame = frames.First();
        frame.StartTime.Should().Be(TimeSpan.Parse("00:13:29.371"));
        frame.EndTime.Should().Be(TimeSpan.Parse("00:13:31.373"));
        frame.Lines.Should().Equal(new[] { "It is my great pleasure" });
    }

    [Fact]
    public void WhenLinesHas1FrameMultipleLines_ThenFrameLinesHasInputLines()
    {
        // Arrange
        var sut = new VttParser();
        var lines = new[] {
            "00:13:29.371 --> 00:13:31.373 line:90% position:50% align:center",
            "It is my great pleasure",
            "2"
        };

        // Act
        var frames = sut.Parse(lines);

        // Assert
        var frame = frames.First();
        frame.StartTime.Should().Be(TimeSpan.Parse("00:13:29.371"));
        frame.EndTime.Should().Be(TimeSpan.Parse("00:13:31.373"));
        frame.Lines.Should().Equal(new[] { "It is my great pleasure", "2" });
    }
}