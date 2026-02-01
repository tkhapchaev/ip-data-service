using Core.Services.Reader.Implementations;

namespace Core.Tests.Reader;

public class FileReaderTests
{
    [Fact]
    public void ReadAllLines_Throws_WhenPathInvalid()
    {
        var reader = new FileReader();

        Assert.Throws<ArgumentException>(() => reader.ReadAllLines("   "));
    }

    [Fact]
    public void ReadAllLines_Throws_WhenFileDoesNotExist()
    {
        var reader = new FileReader();
        var path = Path.Combine(Path.GetTempPath(), Guid.NewGuid() + ".txt");

        Assert.Throws<FileNotFoundException>(() => reader.ReadAllLines(path));
    }

    [Fact]
    public void ReadAllLines_ReturnsLines_WhenFileExists()
    {
        var reader = new FileReader();
        var path = Path.Combine(Path.GetTempPath(), Guid.NewGuid() + ".txt");

        File.WriteAllLines(path, new[] { "a", "b", "c" });

        try
        {
            var lines = reader.ReadAllLines(path);

            Assert.Equal(3, lines.Count);
            Assert.Equal("a", lines[0]);
            Assert.Equal("b", lines[1]);
            Assert.Equal("c", lines[2]);
        }
        finally
        {
            File.Delete(path);
        }
    }
}