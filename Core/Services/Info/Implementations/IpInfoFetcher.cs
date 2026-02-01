using Core.Dtos;
using Core.Models.Data.Implementations;
using Core.Models.Data.Interfaces;
using Core.Services.Info.Interfaces;
using Core.Services.Logger.Interfaces;
using Newtonsoft.Json;

namespace Core.Services.Info.Implementations
{
    public class IpInfoFetcher : IIpInfoFetcher
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger _logger;

        public IpInfoFetcher(HttpClient httpClient, ILogger logger)
        {
            _httpClient = httpClient ?? throw new ArgumentNullException("Http client cannot be null", nameof(httpClient));
            _logger = logger ?? throw new ArgumentNullException("Logger cannot be null", nameof(logger));
        }

        public async Task<IIpData> FetchIpInfo(string ip)
        {
            if (string.IsNullOrWhiteSpace(ip))
                throw new ArgumentException("Ip is invalid", nameof(ip));

            var url = $"https://ipinfo.io/{ip}/json";
            _logger.AppendLog($"Fetching {url} ...");

            var response = await _httpClient.GetAsync(url);

            if (!response.IsSuccessStatusCode)
            {
                var errorBody = await response.Content.ReadAsStringAsync();

                var message = $"GET request to {url} is unsuccessful. Status code: {response.StatusCode}. Content: {errorBody}";
                _logger.AppendLog(message);

                throw new InvalidOperationException(message);
            }
            else
            {
                _logger.AppendLog($"GET request to {url} is successful");
            }
        
            var json = await response.Content.ReadAsStringAsync();
            var ipInfoResponse = JsonConvert.DeserializeObject<IpInfoResponse>(json);

            if (ipInfoResponse is null)
                throw new InvalidOperationException("Ip info response cannot be null");

            var ipData = new IpData(ipInfoResponse.Ip, ipInfoResponse.City, ipInfoResponse.Region, ipInfoResponse.Country);

            return ipData;
        }
    }
}