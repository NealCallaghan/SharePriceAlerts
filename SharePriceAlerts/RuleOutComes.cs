namespace SharePriceAlerts
{
    using System.Collections.Generic;
    using System.Linq;
    using Exceptions;

    public static class RuleOutComes
    {
        public static GetAllTests GetAllTests = () =>
        {
            return new Dictionary<string, PriceInformationTest>
            {
                { "PRSM.L", priceInformation => priceInformation.DailyPrice.Close >= 1700 },
            };
        };

        public static DetermineRuleDetails DetermineRuleDetails = ruleDictionary => dailyPriceInformation =>
            dailyPriceInformation.Select(x => 
                ruleDictionary.TryGetValue(x.Symbol, out var test)
                ? GetDetailsFromTest(test, x)
                : throw new MissingRuleException(x.Symbol));

        private static RuleOutcomeDetails GetDetailsFromTest(PriceInformationTest priceInformationTest, DayPriceInformation dayPriceInformation) =>
            new RuleOutcomeDetails
                {
                    TestPassed = priceInformationTest(dayPriceInformation),
                    Symbol = dayPriceInformation.Symbol,
                };
    }
}