namespace Core.Models.Result.Interfaces
{
    public interface IIpProcessingResult
    {
        string MostPopularCountryName { get; }

        IReadOnlyDictionary<string, int> IpCountByCountry { get; }
        IReadOnlyCollection<string> MostPopularCountryCities { get; }
    }
}