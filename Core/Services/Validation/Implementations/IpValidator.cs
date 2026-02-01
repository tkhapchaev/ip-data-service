using Core.Services.Validation.Interfaces;
using System.Net;
using System.Net.Sockets;

namespace Core.Services.Validation.Implementations
{
    public class IpValidator : IIpValidator
    {
        public bool ValidateIp(string line)
        {
            if (string.IsNullOrWhiteSpace(line))
                return false;

            line = line.Trim();

            if (!IPAddress.TryParse(line, out var ip))
                return false;

            if (ip.AddressFamily == AddressFamily.InterNetwork)
            {
                var parts = line.Split('.');

                if (parts.Length != 4)
                    return false;

                foreach (var part in parts)
                {
                    if (!int.TryParse(part, out var value))
                        return false;

                    if (value < 0 || value > 255)
                        return false;
                }

                return true;
            }

            if (ip.AddressFamily == AddressFamily.InterNetworkV6)
            {
                return line.Contains(':');
            }

            return false;
        }
    }
}