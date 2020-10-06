namespace SharePriceAlerts.Models
{
    using Newtonsoft.Json;

    public class MetaData
    {
        [JsonProperty(PropertyName = "1. Information")]
        public string Information { get; set; }

        [JsonProperty(PropertyName = "2. Symbol")]
        public string Symbol { get; set; }

        [JsonProperty(PropertyName = "3. Last Refreshed")]
        public string LastRefreshed { get; set; }

        [JsonProperty(PropertyName = "4. Output Size")]
        public string OutputSize { get; set; }

        [JsonProperty(PropertyName = "5. Time Zone")]
        public string TimeZone { get; set; }
	}
}
