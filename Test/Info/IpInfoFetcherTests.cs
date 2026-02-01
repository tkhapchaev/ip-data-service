using Core.Services.Info.Implementations;
using Core.Tests.TestDoubles;
using System.Net;
using System.Text;

namespace Core.Tests.Info;

public class IpInfoFetcherTests
{
    [Fact]
    public async Task FetchIpInfo_Throws_WhenIpInvalid()
    {
        var logger = new FakeLogger();

        var httpClient = new HttpClient(new StubHttpMessageHandler(_ => new HttpResponseMessage(HttpStatusCode.OK)));

        var fetcher = new IpInfoFetcher(httpClient, logger);

        await Assert.ThrowsAsync<ArgumentException>(() => fetcher.FetchIpInfo("   "));
    }

    [Fact]
    public async Task FetchIpInfo_ReturnsIpData_WhenResponseIsOk()
    {
        var logger = new FakeLogger();

        var json = """
        {
          "ip": "8.8.8.8",
          "city": "Mountain View",
          "region": "California",
          "country": "US"
        }
        """;

        var handler = new StubHttpMessageHandler(req =>
        {
            Assert.Equal(HttpMethod.Get, req.Method);
            Assert.Contains("https://ipinfo.io/8.8.8.8/json", req.RequestUri!.ToString());

            return new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent(json, Encoding.UTF8, "application/json")
            };
        });

        var httpClient = new HttpClient(handler);
        var fetcher = new IpInfoFetcher(httpClient, logger);

        var data = await fetcher.FetchIpInfo("8.8.8.8");

        Assert.Equal("8.8.8.8", data.Ip);
        Assert.Equal("Mountain View", data.City);
        Assert.Equal("California", data.Region);
        Assert.Equal("US", data.Country);

        Assert.Contains(logger.Messages, message => message.Contains("GET request") && message.Contains("successful"));
    }

    [Fact]
    public async Task FetchIpInfo_ThrowsAndLogs_WhenStatusNotSuccess()
    {
        var logger = new FakeLogger();

        var handler = new StubHttpMessageHandler(_ =>
            new HttpResponseMessage(HttpStatusCode.TooManyRequests)
            {
                Content = new StringContent("{\"error\":\"rate limit\"}")
            });

        var httpClient = new HttpClient(handler);
        var fetcher = new IpInfoFetcher(httpClient, logger);

        var exception = await Assert.ThrowsAsync<InvalidOperationException>(() => fetcher.FetchIpInfo("8.8.8.8"));

        Assert.Contains("unsuccessful", exception.Message);
        Assert.Contains("rate limit", exception.Message);
        Assert.Contains(logger.Messages, message => message.Contains("unsuccessful"));
    }

    [Fact]
    public async Task FetchIpInfo_Throws_WhenJsonMissingRequiredFields()
    {
        var logger = new FakeLogger();

        var json = """
        {
          "ip": "8.8.8.8",
          "city": "",
          "region": "California",
          "country": "US"
        }
        """;

        var handler = new StubHttpMessageHandler(_ =>
            new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent(json, Encoding.UTF8, "application/json")
            });

        var httpClient = new HttpClient(handler);
        var fetcher = new IpInfoFetcher(httpClient, logger);

        await Assert.ThrowsAsync<ArgumentException>(() => fetcher.FetchIpInfo("8.8.8.8"));
    }
}