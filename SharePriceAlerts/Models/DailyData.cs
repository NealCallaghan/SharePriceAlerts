namespace SharePriceAlerts.Models
{
    using System;
    using System.Collections.Generic;
    using Newtonsoft.Json;

    public class DailyData
    {
        [JsonProperty(PropertyName = "Meta Data")]
        public MetaData MetaData { get; set; }

        [JsonProperty(PropertyName = "Time Series (Daily)")]
        public Dictionary<DateTime, DailyPrice> TimeSeries { get; set; }
    }
}