using Newtonsoft.Json;

namespace Core.Dtos
{
    public class IpInfoResponse
    {
        [JsonProperty("ip")]
        public string Ip { get; set; }

        [JsonProperty("hostname")]
        public string HostName { get; set; }

        [JsonProperty("city")]
        public string City { get; set; }

        [JsonProperty("region")]
        public string Region { get; set; }

        [JsonProperty("country")]
        public string Country { get; set; }

        [JsonProperty("loc")]
        public string Location { get; set; }

        [JsonProperty("org")]
        public string Organization { get; set; }

        [JsonProperty("postal")]
        public string PostalCode { get; set; }

        [JsonProperty("timezone")]
        public string TimeZone { get; set; }

        [JsonProperty("readme")]
        public string ReadMe { get; set; }
    }
}