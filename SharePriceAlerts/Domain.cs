namespace SharePriceAlerts
{
    using System;
    using System.Collections.Generic;
    using System.Net.Http;
    using System.Threading.Tasks;
    using Models;
    using ScrapySharp.Network;

    public class DayPriceInformation
    {
        public string WebAddress { get; set; } 
        public string Symbol { get; set; }
        public decimal DailyPrice { get; set; }
    }

    public class RuleOutcomeDetails
    {
        public bool TestPassed { get; set; } 
        public string Symbol { get; set; }
    }

    public delegate Func<(string, string), Task<DayPriceInformation>> DailyPriceGetter(ScrapingBrowser browser);

    public delegate bool PriceInformationTest(DayPriceInformation dayPriceInformation);

    public delegate IDictionary<(string, string), PriceInformationTest> SymbolToTestDictionary();

    public delegate Func<IEnumerable<DayPriceInformation>, IEnumerable<RuleOutcomeDetails>> DetermineRuleDetails(IDictionary<(string, string), PriceInformationTest> priceInformationTests);

    public delegate Task AlertWhereRequired(HttpClient client, IEnumerable<RuleOutcomeDetails> ruleOutcomeDetails);
}