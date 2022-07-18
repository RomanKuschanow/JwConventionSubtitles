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
        var frames = sut.Parse(lines.ToList());

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
        var frames = sut.Parse(lines.ToList());

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
        var frames = sut.Parse(lines.ToList());

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
        var frames = sut.Parse(lines.ToList());

        // Assert
        var frame = frames.First();
        frame.StartTime.Should().Be(TimeSpan.Parse("00:13:29.371"));
        frame.EndTime.Should().Be(TimeSpan.Parse("00:13:31.373"));
        frame.Lines.Should().Equal(new[] { "It is my great pleasure", "2" });
    }

    [Fact]
    public void WhenLinesHas3Frames_Then3FramesWithCorrectProperties()
    {
        // Arrange
        var sut = new VttParser();
        var lines = new[] {
            "00:13:29.371 --> 00:13:31.373 line:90% position:50% align:center",
            "It is my great pleasure",
            "\n",
            "01:37:34.285 --> 01:37:36.788 line:90% position:50% align:center",
            "our continued love for Jehovah,",
            "for our neighbor, and for God’s Word",
            "\n",
            "01:37:41.751 --> 01:37:45.338 line:90% position:50% align:center",
            "will lead to our enjoying everlasting peace"
        };

        // Act
        var frames = sut.Parse(lines.ToList());

        // Assert
        var framesList = frames.ToList();

        var frame1 = framesList[0];
        frame1.StartTime.Should().Be(TimeSpan.Parse("00:13:29.371"));
        frame1.EndTime.Should().Be(TimeSpan.Parse("00:13:31.373"));
        frame1.Lines.Should().Equal(new[] { "It is my great pleasure" });

        var frame2 = framesList[1];
        frame2.StartTime.Should().Be(TimeSpan.Parse("01:37:34.285"));
        frame2.EndTime.Should().Be(TimeSpan.Parse("01:37:36.788"));
        frame2.Lines.Should().Equal(new[] { "our continued love for Jehovah,", "for our neighbor, and for God’s Word" });

        var frame3 = framesList[2];
        frame3.StartTime.Should().Be(TimeSpan.Parse("01:37:41.751"));
        frame3.EndTime.Should().Be(TimeSpan.Parse("01:37:45.338"));
        frame3.Lines.Should().Equal(new[] { "will lead to our enjoying everlasting peace" });
    }
}