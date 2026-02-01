using Core.Services.Logger.Interfaces;
using Core.Services.Reader.Interfaces;
using Core.Services.Source.Interfaces;
using Core.Services.Validation.Interfaces;

namespace Core.Services.Source.Implementations
{
    public class FileIpSource : IIpSource
    {
        private readonly string _path;

        private readonly IFileReader _fileReader;
        private readonly IIpValidator _ipValidator;

        private readonly ILogger _logger;

        public FileIpSource(string path, IFileReader fileReader, IIpValidator ipValidator, ILogger logger)
        {
            _path = string.IsNullOrWhiteSpace(path) ? throw new ArgumentException("Path is invalid", nameof(path)) : path;

            _fileReader = fileReader ?? throw new ArgumentNullException("File reader cannot be null", nameof(fileReader));
            _ipValidator = ipValidator ?? throw new ArgumentNullException("Ip validator cannot be null", nameof(ipValidator));

            _logger = logger ?? throw new ArgumentNullException("Logger cannot be null", nameof(logger));
        }

        public List<string> GetIps()
        {
            var ips = new List<string>();
            var lineNumber = 0;

            List<string> lines;

            try
            {
                lines = _fileReader.ReadAllLines(_path);
            }
            catch (Exception exception)
            {
                _logger.AppendLog($"Exception while reading ips from file: {exception.Message}");

                return [];
            }

            foreach (var line in lines)
            {
                if (_ipValidator.ValidateIp(line))
                {
                    ips.Add(line);

                    _logger.AppendLog($"Successfully read ip \"{line}\"");

                } else
                {
                    _logger.AppendLog($"Ip \"{line}\" is invalid (line number: {lineNumber})");
                }

                lineNumber++;
            }

            return ips;
        }
    }
}