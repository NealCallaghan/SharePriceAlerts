namespace SharePriceAlerts
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Exceptions;
    using Models;
    using ScrapySharp.Extensions;

    public static class PriceRetriever
    {
        public static DailyPriceGetter GetDailyPrice = browser => async ((string webAddress, string symbol) tuple) =>
        {
            var (webAddress, symbol) = tuple;

            var pageResult = await browser.NavigateToPageAsync(new Uri(webAddress));
            var priceNode = pageResult.Html.CssSelect(".ml-1")?.First();

            if(priceNode == null || !decimal.TryParse(priceNode.InnerText.Trim().Replace("p", string.Empty), out var dailyPrice))
                throw new UnSuccessfulScrapingException($"Unable to scrape: {symbol} {webAddress}");

            return new DayPriceInformation
            {
                DailyPrice = dailyPrice,
                WebAddress = webAddress,
                Symbol = symbol,
            };
        };

        private static DailyPrice GetLatestDailyData(Dictionary<DateTime, DailyPrice> timeSeries) =>
            timeSeries.OrderByDescending(ts => ts.Key).First().Value;

        private static string GetRequestUrl(string symbol, string apiKey) =>
            $"https://www.alphavantage.co/query?function=TIME_SERIES_DAILY&symbol={symbol}&apikey={apiKey}";
    }
}