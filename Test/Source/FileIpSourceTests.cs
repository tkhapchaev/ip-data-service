using Core.Services.Source.Implementations;
using Core.Tests.TestDoubles;

namespace Core.Tests.Source;

public class FileIpSourceTests
{
    [Fact]
    public void GetIps_ReturnsOnlyValidIps()
    {
        var logger = new FakeLogger();
        var reader = new FakeFileReader(new List<string> { "8.8.8.8", "bad", "1.1.1.1" });

        var validator = new FakeIpValidator(s => s == "8.8.8.8" || s == "1.1.1.1");

        var source = new FileIpSource("any.txt", reader, validator, logger);

        var ips = source.GetIps();

        Assert.Equal(2, ips.Count);
        Assert.Contains("8.8.8.8", ips);
        Assert.Contains("1.1.1.1", ips);

        Assert.Contains(logger.Messages, message => message.Contains("Successfully read ip"));
        Assert.Contains(logger.Messages, message => message.Contains("is invalid"));
    }

    [Fact]
    public void GetIps_ReturnsEmptyAndLogs_WhenReaderThrows()
    {
        var logger = new FakeLogger();
        var reader = new FakeFileReader(new IOException("boom"));
        var validator = new FakeIpValidator(_ => true);

        var source = new FileIpSource("any.txt", reader, validator, logger);

        var ips = source.GetIps();

        Assert.Empty(ips);
        Assert.Contains(logger.Messages, message => message.Contains("Exception while reading ips from file"));
    }
}