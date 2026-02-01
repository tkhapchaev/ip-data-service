namespace Core.Models.Data.Interfaces
{
    public interface IIpData
    {
        string Ip { get; }
        string City { get; }
        string Region { get; }
        string Country { get; }
    }
}