namespace Core.Services.Reader.Interfaces
{
    public interface IFileReader
    {
        List<string> ReadAllLines(string path);
    }
}