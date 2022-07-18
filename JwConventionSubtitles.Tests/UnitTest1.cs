using VTT_Reader;
using Moq;
using FluentAssertions;

namespace JwConventionSubtitles.Tests;

public class UnitTest1
{
    [Fact]
    public void Test1()
    {
        // Arrange
        var sut = new VttParser();

        // Act
        sut.LoadSubtitles();

        // Assert

    }
}