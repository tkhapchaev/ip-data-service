using Core.Models.Result.Interfaces;

namespace Core.Models.Result.Implementations
{
    public record IpProcessingResult : IIpProcessingResult
    {
        private IReadOnlyDictionary<string, int> _ipCountByCountry;
        private IReadOnlyCollection<string> _mostPopularCountryCities;

        public IpProcessingResult(IDictionary<string, int> ipCountByCountry, ICollection<string> mostPopularCountryCities, string mostPopularCountryName) 
        {
            if (ipCountByCountry is null)
                throw new ArgumentNullException("Ip count by country cannot be null", nameof(ipCountByCountry));

            if (mostPopularCountryCities is null)
                throw new ArgumentNullException("Most popular country cities cannot be null", nameof(mostPopularCountryCities));

            _ipCountByCountry = ipCountByCountry.ToDictionary().AsReadOnly();
            _mostPopularCountryCities = mostPopularCountryCities.ToList().AsReadOnly();

            MostPopularCountryName = string.IsNullOrEmpty(mostPopularCountryName) ? throw new ArgumentException("Most popular country name is invalid", nameof(mostPopularCountryName)) : mostPopularCountryName;
        }

        public string MostPopularCountryName { get; }

        public IReadOnlyDictionary<string, int> IpCountByCountry => _ipCountByCountry;
        public IReadOnlyCollection<string> MostPopularCountryCities => _mostPopularCountryCities;
    }
}