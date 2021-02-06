namespace SharePriceAlerts
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;
    using Exceptions;
    using Microsoft.Azure.WebJobs;
    using Microsoft.Extensions.Logging;
    using ScrapySharp.Network;

    using static PriceRetriever;
    using static RuleOutComes;
    using static MessageAlert;

    public static class SharePriceAlerterFunction
    {
        [FunctionName("SharePriceAlerterFunction")]
        public static async Task Run([TimerTrigger("0 0 9-17 * * 1-5")]TimerInfo timer, ILogger log)
        {
            var httpClientFactory = new HttpClientFactory();
            var executeCatchAllWithLogger = ExecuteCatchAll(new SharePriceAlertLogger(log, httpClientFactory));

            await executeCatchAllWithLogger(async () =>
            {
                var outComeTestsAndSymbols = SymbolToTestDictionary();
                var getPriceOutcomes = DetermineRuleDetails(outComeTestsAndSymbols);

                var getDailyPriceWithBrowser = GetDailyPrice(new ScrapingBrowser());
                var dailyPrices = outComeTestsAndSymbols.Keys.Select(getDailyPriceWithBrowser);

                var dayPriceInformation = await Task.WhenAll(dailyPrices);

                var ruleOutcomes = getPriceOutcomes(dayPriceInformation);

                await AlertWhereRequired(httpClientFactory.HttpClient, ruleOutcomes);
            });
        }

        private static Func<Action, Task> ExecuteCatchAll(ISharePriceAlertLogger log) =>
            async func =>
            {
                try
                {
                    log.LogInformation($"Share Price function started executing at: {DateTime.Now}");
                    await Task.Run(func);
                    log.LogInformation($"Share Price function finished executing at: {DateTime.Now}");
                }
                catch (MissingRuleException e)
                {
                    await log.LogError(e, "Share symbol found with no rule");
                }
                catch (UnSuccessfulScrapingException e)
                {
                    await log.LogError(e, $"Issue with getting information from Scraper at {DateTime.Now}");
                }
                catch (UnSuccessfulTwilioAlertException e)
                {
                    await log.LogError(e, $"Issue with alerting through twilio at {DateTime.Now}");
                }
                catch (UnSuccessfulSlackException e)
                {
                    await log.LogCritical(e, $"Issue with alerting through slack at {DateTime.Now}");
                }
                catch (Exception e)
                {
                    await log.LogCritical(e, $"Critical exception thrown at {DateTime.Now}");
                }
            };
    }
}
