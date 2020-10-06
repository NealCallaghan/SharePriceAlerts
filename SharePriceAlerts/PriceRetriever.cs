namespace SharePriceAlerts
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net.Http;
    using Exceptions;
    using Models;
    using Newtonsoft.Json;

    public static class PriceRetriever
    {
        public static DailyPriceGetter GetDailyPrice = client => async symbol =>
        {
            var apiKey = Environment.GetEnvironmentVariable("alphavantageApiKey");
            using var request = new HttpRequestMessage(HttpMethod.Get, GetRequestUrl(symbol, apiKey));
            using var response = await client.SendAsync(request);
            var stream = await response.Content.ReadAsStringAsync();

            var latestPrice = response.IsSuccessStatusCode
                ? GetLatestDailyData(JsonConvert.DeserializeObject<DailyData>(stream).TimeSeries)
                : throw new UnSuccessfulAlphaResponse("Unsuccessful response from alphavantage api");

            return new DayPriceInformation
            {
                DailyPrice = latestPrice,
                Symbol = symbol,
            };
        };

        private static DailyPrice GetLatestDailyData(Dictionary<DateTime, DailyPrice> timeSeries) =>
            timeSeries.OrderByDescending(ts => ts.Key).First().Value;

        private static string GetRequestUrl(string symbol, string apiKey) =>
            $"https://www.alphavantage.co/query?function=TIME_SERIES_DAILY&symbol={symbol}&apikey={apiKey}";
    }
}