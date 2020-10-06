namespace SharePriceAlerts.Models
{
    using Newtonsoft.Json;

    public class DailyPrice
    {
		[JsonProperty(PropertyName = "1. open")]
        public decimal Open { get; set; }

        [JsonProperty(PropertyName = "2. high")]
        public decimal High { get; set; }

        [JsonProperty(PropertyName = "3. low")]
        public decimal Low { get; set; }

        [JsonProperty(PropertyName = "4. close")]
        public decimal Close { get; set; }

        [JsonProperty(PropertyName = "5. volume")]
        public decimal Volume { get; set; }
	}
}