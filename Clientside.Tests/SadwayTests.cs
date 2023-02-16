using BL.Lib;

namespace Clientside.Tests;

public class SadwayTests
{
    [Theory]
    [MemberData(nameof(GetTestDataForHttpRequestException))]
    public Task Can_ThrowHttpRequestException(string url)
    {
        // Given is MemberData

        // When
        Action action = () => new Reader().GetTextinputData(url);

        // Then
        Assert.Throws<HttpRequestException>(action);

        return Task.CompletedTask;
    }

    public static IEnumerable<object[]> GetTestDataForHttpRequestException()
    {
        string url = "http://httpstat.us/404";

        yield return new object[] { url };

        url = "http://httpstat.us/500";

        yield return new object[] { url };
    }
}
