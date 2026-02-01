using Core.Services.Validation.Interfaces;

namespace Core.Tests.TestDoubles;

public sealed class FakeIpValidator : IIpValidator
{
    private readonly Func<string, bool> _rule;

    public FakeIpValidator(Func<string, bool> rule) => _rule = rule;

    public bool ValidateIp(string line) => _rule(line);
}