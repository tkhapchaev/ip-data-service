using Core.Services.Logger.Interfaces;

namespace Core.Tests.TestDoubles;

public sealed class FakeLogger : ILogger
{
    public List<string> Messages { get; } = new();

    public void AppendLog(string message)
    {
        Messages.Add(message);
    }
}