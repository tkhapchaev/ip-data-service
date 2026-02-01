using Core.Services.Validation.Implementations;

namespace Core.Tests.Validation;

public class IpValidatorTests
{
    [Theory]
    [InlineData("8.8.8.8", true)]
    [InlineData("192.168.0.1", true)]
    [InlineData("::1", true)]
    [InlineData("999.1.1.1", false)]
    [InlineData("1.2.3", false)]
    [InlineData("", false)]
    [InlineData("   ", false)]
    public void ValidateIp_ReturnsExpected(string input, bool expected)
    {
        var validator = new IpValidator();

        var result = validator.ValidateIp(input);

        Assert.Equal(expected, result);
    }
}