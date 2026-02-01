using Core.Services.Reader.Interfaces;

namespace Core.Tests.TestDoubles;

public sealed class FakeFileReader : IFileReader
{
    private readonly List<string>? _lines;
    private readonly Exception? _exception;

    public FakeFileReader(List<string> lines) => _lines = lines;
    public FakeFileReader(Exception exception) => _exception = exception;

    public List<string> ReadAllLines(string path)
    {
        if (_exception is not null)
            throw _exception;

        return _lines ?? new List<string>();
    }
}