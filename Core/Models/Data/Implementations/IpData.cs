using Core.Models.Data.Interfaces;

namespace Core.Models.Data.Implementations
{
    public record IpData : IIpData
    {
        public IpData(string ip, string city, string region, string country)
        {
            Ip = string.IsNullOrWhiteSpace(ip) ? throw new ArgumentException("Ip is invalid", nameof(ip)) : ip;
            City = string.IsNullOrWhiteSpace(city) ? throw new ArgumentException("City is invalid", nameof(city)) : city;
            Region = string.IsNullOrWhiteSpace(region) ? throw new ArgumentException("Region is invalid", nameof(region)) : region;
            Country = string.IsNullOrWhiteSpace(country) ? throw new ArgumentException("Country is invalid", nameof(country)) : country;
        }

        public string Ip { get; }
        public string City { get; }
        public string Region { get; }
        public string Country { get;}
    }
}