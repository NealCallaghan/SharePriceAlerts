namespace SharePriceAlerts
{
    using System;
    using System.Collections.Generic;
    using System.Net.Http;
    using System.Threading.Tasks;
    using Models;

    public class DayPriceInformation
    {
        public string Symbol { get; set; } 
        public DailyPrice DailyPrice { get; set; }
    }

    public class RuleOutcomeDetails
    {
        public bool TestPassed { get; set; } 
        public string Symbol { get; set; }
    }

    public delegate Func<string, Task<DayPriceInformation>> DailyPriceGetter(HttpClient client);

    public delegate bool PriceInformationTest(DayPriceInformation dayPriceInformation);

    public delegate IDictionary<string, PriceInformationTest> SymbolToTestDictionary();

    public delegate Func<IEnumerable<DayPriceInformation>, IEnumerable<RuleOutcomeDetails>> DetermineRuleDetails(IDictionary<string, PriceInformationTest> priceInformationTests);

    public delegate Task AlertWhereRequired(HttpClient client, IEnumerable<RuleOutcomeDetails> ruleOutcomeDetails);
}