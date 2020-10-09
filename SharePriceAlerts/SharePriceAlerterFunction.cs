namespace SharePriceAlerts
{
    using System;
    using System.Linq;
    using System.Net.Http;
    using System.Threading.Tasks;
    using Exceptions;
    using Microsoft.Azure.WebJobs;
    using Microsoft.Extensions.Logging;

    using static PriceRetriever;
    using static RuleOutComes;
    using static MessageAlert;

    public static class SharePriceAlerterFunction
    {
        [FunctionName("SharePriceAlerterFunction")]
        public static async Task Run([TimerTrigger("0 0 9-17 * * 1-5")]TimerInfo myTimer, ILogger log)
        {
            log.LogInformation($"Share Price function started executing at: {DateTime.Now}");

            var executeCatchAllWithLogger = ExecuteCatchAll(log);

            await executeCatchAllWithLogger(async () =>
            {
                var outComeTestsAndSymbols = GetAllTests();
                var getPriceOutcomes = DetermineRuleDetails(outComeTestsAndSymbols);

                using var client = new HttpClient();
                var getDailyPriceWithClient = GetDailyPrice(client);
                var dailyPrices = outComeTestsAndSymbols.Keys.Select(getDailyPriceWithClient);

                var dayPriceInformation = await Task.WhenAll(dailyPrices);

                var ruleOutcomes = getPriceOutcomes(dayPriceInformation);

                AlertWhereRequired(ruleOutcomes);
            });

            log.LogInformation($"Share Price function finished executing at: {DateTime.Now}");
        }

        private static Func<Action, Task> ExecuteCatchAll(ILogger log) =>
            async func =>
            {
                try
                {
                    await Task.Run(func);
                }
                catch (MissingRuleException e)
                {
                    log.LogWarning(e, "Share symbol found with no rule");
                }
                catch (UnSuccessfulAlphaResponseException e)
                {
                    log.LogWarning(e, $"Issue with getting information from Alpha Vantage at {DateTime.Now}");
                }
                catch (UnSuccessfulTwilioAlertException e)
                {
                    log.LogWarning(e, $"Issue with alerting through twilio at {DateTime.Now}");
                }
                catch (Exception e)
                {
                    log.LogCritical(e, $"Critical exception thrown at {DateTime.Now}");
                }
            };
    }
}
