using Core.Models.Data.Interfaces;

namespace Core.Services.Info.Interfaces
{
    public interface IIpInfoFetcher
    {
        Task<IIpData> FetchIpInfo(string ip);
    }
}