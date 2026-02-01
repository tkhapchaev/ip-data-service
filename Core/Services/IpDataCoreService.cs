using Core.Services.Info.Implementations;
using Core.Services.Validation.Implementations;
using Core.Services.Logger.Interfaces;
using Core.Services.Source.Implementations;
using Core.Services.Reader.Implementations;
using Core.Models.Data.Interfaces;
using Core.Models.Result.Implementations;

namespace Core.Services
{
    public class IpDataCoreService
    {
        public const int HttpClientTimeoutSeconds = 10;

        private readonly ILogger _logger;

        private readonly FileIpSource _fileIpSource;
        private readonly IpInfoFetcher _ipInfoService;

        public IpDataCoreService(string filePath, ILogger logger)
        {
            var fileReader = new FileReader();
            var ipValidator = new IpValidator();

            var httpClient = new HttpClient
            {
                Timeout = TimeSpan.FromSeconds(HttpClientTimeoutSeconds)
            };

            _logger = logger ?? throw new ArgumentNullException("Logger cannot be null", nameof(logger));

            _fileIpSource = new FileIpSource(filePath, fileReader, ipValidator, _logger);
            _ipInfoService = new IpInfoFetcher(httpClient, _logger);
        }

        public async Task<IpProcessingResult?> GetIpProcessingResult()
        {
            var ipDataByCountry = new Dictionary<string, List<IIpData>>();
            var ips = _fileIpSource.GetIps();

            foreach (var ip in ips)
            {
                IIpData ipData;

                try
                {
                    ipData = await _ipInfoService.FetchIpInfo(ip);
                }
                catch (Exception exception)
                {
                    _logger.AppendLog($"Exception while fetching ip info: {exception.Message}");

                    continue;
                }

                if (!ipDataByCountry.ContainsKey(ipData.Country))
                {
                    ipDataByCountry[ipData.Country] = [];
                }

                ipDataByCountry[ipData.Country].Add(ipData);
            }

            if (ipDataByCountry.Count == 0)
            {
                return null;
            }

            var ipCountByCountry = new Dictionary<string, int>();

            foreach (var kvp in ipDataByCountry)
            {
                ipCountByCountry[kvp.Key] = kvp.Value.Count;
            }

            var mostPopularCountry = ipDataByCountry.OrderByDescending(kvp => kvp.Value.Count).First();
            var mostPopularCountryCities = mostPopularCountry.Value.Select(ipData => ipData.City).Distinct().ToList();

            var result = new IpProcessingResult(ipCountByCountry, mostPopularCountryCities, mostPopularCountry.Key);

            return result;
        }
    }
}