using Core.Services.Reader.Interfaces;

namespace Core.Services.Reader.Implementations
{
    public class FileReader : IFileReader
    {
        public List<string> ReadAllLines(string path)
        {
            if (string.IsNullOrWhiteSpace(path))
                throw new ArgumentException("Path is invalid", nameof(path));

            if (!File.Exists(path))
                throw new FileNotFoundException("File does not exist", path);

            return new List<string>(File.ReadAllLines(path));
        }
    }
}