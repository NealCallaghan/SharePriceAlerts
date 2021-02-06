namespace SharePriceAlerts
{
    using System.Collections.Generic;
    using System.Linq;
    using Exceptions;

    public static class RuleOutComes
    {
        public static SymbolToTestDictionary SymbolToTestDictionary = () =>
        {
            return new Dictionary<(string, string), PriceInformationTest>
            {
                { ("https://www.markets.iweb-sharedealing.co.uk/shares-centre/etfs/detail/INRG/IE00B1XNHC34/0P0000956B", "INRG Clean Energy"), priceInformation => priceInformation.DailyPrice <= 964.00M },
                { ("https://www.markets.iweb-sharedealing.co.uk/shares-centre/sharedealing/details/?csid=24710", "Powerhouse energy"), priceInformation => priceInformation.DailyPrice <= 7.016M },
                { ("https://www.markets.iweb-sharedealing.co.uk/shares-centre/etfs/detail/IH2O/IE00B1TXK627/0P0000GGXP", "IH20 Clean Water"), priceInformation => priceInformation.DailyPrice <= 3215.36M },
                { ("https://www.markets.iweb-sharedealing.co.uk/shares-centre/etfs/detail/SWDA/IE00B4L5Y983/0P0000LZZD", "SWDA"), priceInformation => priceInformation.DailyPrice <= 4259.68M },
                { ("https://www.markets.iweb-sharedealing.co.uk/shares-centre/sharedealing/details/?csid=1857420", "EQT"), priceInformation => priceInformation.DailyPrice <= 1.652M },
                { ("https://www.markets.iweb-sharedealing.co.uk/shares-centre/etfs/detail/BATG/IE00BF0M2Z96/0P0001CLRJ", "BATG"), priceInformation => priceInformation.DailyPrice <= 950.86M },
                //{ ("https://www.markets.iweb-sharedealing.co.uk/shares-centre/etfs/detail/ESGB/IE00BYWQWR46/0P0001HWOS", "ESGB video games"), priceInformation => priceInformation.DailyPrice <= 2453.19M },
                { ("https://www.markets.iweb-sharedealing.co.uk/shares-centre/etfs/detail/VWRL/IE00B3RBWM25/0P0000WAHE", "VWRL"), priceInformation => priceInformation.DailyPrice <= 6152.04M },
                //{ ("https://www.markets.iweb-sharedealing.co.uk/shares-centre/etfs/detail/VUAG/IE00BFMXXD54/0P0001HZW3", "VAUG S&P"), priceInformation => priceInformation.DailyPrice <= 3973.72M },
                //{ ("https://www.markets.iweb-sharedealing.co.uk/shares-centre/etfs/detail/HMCA/IE00BF4NQ904/0P0001DYSB", "HMCT China"), priceInformation => priceInformation.DailyPrice <= 815M },
            };
        };

        public static DetermineRuleDetails DetermineRuleDetails = ruleDictionary => dailyPriceInformation =>
            dailyPriceInformation.Select(x => 
                ruleDictionary.TryGetValue((x.WebAddress, x.Symbol), out var test)
                ? GetDetailsFromTest(test, x)
                : throw new MissingRuleException(x.WebAddress));

        private static RuleOutcomeDetails GetDetailsFromTest(PriceInformationTest priceInformationTest, DayPriceInformation dayPriceInformation) =>
            new RuleOutcomeDetails
                {
                    TestPassed = priceInformationTest(dayPriceInformation),
                    Symbol = dayPriceInformation.Symbol,
                };
    }
}