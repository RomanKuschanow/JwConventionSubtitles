using JwConventionSubtitles;
using Moq;
using FluentAssertions;
using System;

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
            "",
            "01:37:34.285 --> 01:37:36.788 line:90% position:50% align:center",
            "our continued love for Jehovah,",
            "for our neighbor, and for God’s Word",
            "",
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

public class FileReaderTests
{
    [Fact]
    public void FileReadCorrect()
    {
        // Arrange
        var sut = new FileReader();

        // Act
        var lines = sut.ReadLines(@"C:\CO-r22_E_01.vtt");

        // Assert
        lines.Should().NotBeEmpty();
    }
}

public class ReadFileAndParseIntegrationTests
{
    [Fact]
    public void ReadFileAndParseCorrect()
    {
        // Arrange
        var reader = new FileReader();
        var parser = new VttParser();

        // Act
        var lines = reader.ReadLines(@"C:\CO-r22_E_01.vtt");
        var frames = parser.Parse(lines.ToList());

        // Assert
        frames.Should().NotBeEmpty();
        var frame = frames.First();
        frame.StartTime.Should().Be(TimeSpan.Parse("00:00:22.251"));
        frame.EndTime.Should().Be(TimeSpan.Parse("00:00:26.255"));
        frame.Lines.Should().Equal(new[] { "On behalf of the Governing Body", "and all of those working" });
    }
}

public class ConventionProgramParserTests
{
    [Fact]
    public void WhenLinesEmpty_ThenSpeechesEmpty()
    {
        // Arrange
        var sut = new ConventionProgramParser();
        var lines = new[] {
            ""
        };

        // Act
        var speeches = sut.Parse(lines.ToList());

        // Assert
        speeches.Should().BeEmpty();
    }

    [Fact]
    public void WhenLinesHas1Speech_ThenSpeechesListHas1Speech()
    {
        // Arrange
        var sut = new ConventionProgramParser();
        var lines = new[] {
            "9:40 CHAIRMAN’S ADDRESS: Jehovah Is “the God Who Gives Peace” (Romans 15:33; Philippians 4:6, 7)"
        };

        // Act
        var speeches = sut.Parse(lines.ToList());

        // Assert
        speeches.Should().NotBeEmpty();
        speeches.First().Name.Should().Be("Jehovah Is the God Who Gives Peace");
    }

    [Fact]
    public void WhenLinesHas1SpeechAndSomeOtherLines_ThenSpeechesListHas1Speech()
    {
        // Arrange
        var sut = new ConventionProgramParser();
        var lines = new[] {
            "9:20 Music-Video Presentation",
            "",
            "9:30 Song No. 86 and Prayer",
            "",
            "9:40 CHAIRMAN’S ADDRESS: Jehovah Is “the God Who Gives Peace” (Romans 15:33; Philippians 4:6, 7)"
        };

        // Act
        var speeches = sut.Parse(lines.ToList());

        // Assert
        speeches.Count().Should().Be(1);
    }

    [Fact]
    public void WhenLinesHasSymposium_ThenSpeechesListHasAllSymposiumSpeeches()
    {
        // Arrange
        var sut = new ConventionProgramParser();
        var lines = new[] {
            "10:10 SYMPOSIUM: How Love Leads to Genuine Peace",
            "",
            "• Love for God (Matthew 22:37, 38; Romans 12:17 - 19)",
            "",
            "• Love of Neighbor (Matthew 22:39; Romans 13:8 - 10)",
            "",
            "• Love for God’s Word (Psalm 119:165, 167, 168)",
            "",
            "11:05 Song No. 24 and Announcements"
            };

        // Act
        var speeches = sut.Parse(lines.ToList());

        // Assert
        speeches.Count().Should().Be(3);
        speeches.First().Name.Should().Be("How Love Leads to Genuine Peace: Love for God");
    }

    [Fact]
    public void WhenLinesHasSpeechAndSymposium_ThenSpeechesListHasAllSpeeches()
    {
        // Arrange
        var sut = new ConventionProgramParser();
        var lines = new[] {
            "9:20 Music-Video Presentation",
            "",
            "9:30 Song No. 86 and Prayer",
            "",
            "9:40 CHAIRMAN’S ADDRESS: Jehovah Is “the God Who Gives Peace” (Romans 15:33; Philippians 4:6, 7)",
            "",
            "10:10 SYMPOSIUM: How Love Leads to Genuine Peace",
            "",
            "• Love for God (Matthew 22:37, 38; Romans 12:17 - 19)",
            "",
            "• Love of Neighbor (Matthew 22:39; Romans 13:8 - 10)",
            "",
            "• Love for God’s Word (Psalm 119:165, 167, 168)",
            "",
            "11:05 Song No. 24 and Announcements"
            };

        // Act
        var speeches = sut.Parse(lines.ToList());

        // Assert
        var speechesList = speeches.ToList();
        speechesList.Count.Should().Be(4);
        speechesList[0].Name.Should().Be("Jehovah Is the God Who Gives Peace");
        speechesList[1].Name.Should().Be("How Love Leads to Genuine Peace: Love for God");
        speechesList[2].Name.Should().Be("How Love Leads to Genuine Peace: Love of Neighbor");
        speechesList[3].Name.Should().Be("How Love Leads to Genuine Peace: Love for God’s Word");
    }
}
public class ReadProgrtamAndParseIntegrationTests
{
    [Fact]
    public void ReadFileAndParseCorrect()
    {
        // Arrange
        var reader = new FileReader();
        var parser = new ConventionProgramParser();

        // Act
        var lines = reader.ReadLines(@"C:\Users\Roman\Documents\Projects\JwConventionSubtitles\Program.txt");
        var speeches = parser.Parse(lines.ToList());

        // Assert
        var speechesList = speeches.ToList();
        speechesList[0].Name.Should().Be("Jehovah Is the God Who Gives Peace");
        speechesList[1].Name.Should().Be("How Love Leads to Genuine Peace: Love for God");
        speechesList[2].Name.Should().Be("How Love Leads to Genuine Peace: Love of Neighbor");
        speechesList[3].Name.Should().Be("How Love Leads to Genuine Peace: Love for God’s Word");
    }
}

public class SpeechConverterTests
{
    [Fact]
    public void WhenFramesAndSpeechesFromProgramListNotEmpty_ThenSpeechesWithTextNotEmpty()
    {
        //Arrage
        var reader = new FileReader();
        var programParser = new ConventionProgramParser();
        var vttParser = new VttParser();
        var speechConverter = new SpeechConverter();

        // Act
        var vttLines = reader.ReadLines(@"C:\CO-r22_E_01.vtt");
        var programLines = reader.ReadLines(@"C:\Users\Roman\Documents\Projects\JwConventionSubtitles\Program.txt");
        var frames = vttParser.Parse(vttLines.ToList());
        var speechesFromProgram = programParser.Parse(programLines.ToList());
        var speechesWithText = speechConverter.Convert(frames.ToList(), speechesFromProgram.ToList());

        // Assert
        speechesWithText.Should().NotBeEmpty();
    }

    [Fact]
    public void WhenFramesAndSpeechesFromProgramListHas4Speeches_ThenSpeechesWithTextHas4Speeches()
    {
        //Arrage
        var reader = new FileReader();
        var programParser = new ConventionProgramParser();
        var vttParser = new VttParser();
        var speechConverter = new SpeechConverter();

        // Act
        var vttLines = reader.ReadLines(@"C:\CO-r22_E_01.vtt");
        var programLines = reader.ReadLines(@"C:\Users\Roman\Documents\Projects\JwConventionSubtitles\Program.txt");
        var frames = vttParser.Parse(vttLines.ToList());
        var speechesFromProgram = programParser.Parse(programLines.ToList());
        var speechesWithText = speechConverter.Convert(frames.ToList(), speechesFromProgram.ToList());

        // Assert
        speechesWithText.Count().Should().Be(4);
    }
}