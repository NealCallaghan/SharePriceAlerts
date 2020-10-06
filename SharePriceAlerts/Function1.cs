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

    public static class Function1
    {
        [FunctionName("Function1")]
        //"0 */5 * * * *"
        public static async Task Run([TimerTrigger("1 * * * * *")]TimerInfo myTimer, ILogger log)
        {
            var logInformation = GetLogger(log);

            var thing = Environment.GetEnvironmentVariable("your_key_here");

            try
            {
                var outComeTestsAndSymbols = GetAllTests();
                var getPriceOutcomes = DetermineRuleDetails(outComeTestsAndSymbols);

                using var client = new HttpClient();
                var getDailyPriceWithClient = GetDailyPrice(client);
                var dailyPrices = outComeTestsAndSymbols.Keys.Select(getDailyPriceWithClient);

                var dayPriceInformation = await Task.WhenAll(dailyPrices);

                var ruleOutcomes = getPriceOutcomes(dayPriceInformation);

                AlertWhereRequired(ruleOutcomes);
            }
            catch (UnSuccessfulAlphaResponse e)
            {
                Console.WriteLine(e);
                throw;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }

            logInformation($"C# Timer trigger function executed at: {DateTime.Now}");
        }

        private static Action<string> GetLogger(ILogger logger) =>
            stringToLog => logger.LogInformation(stringToLog);
    }
}
