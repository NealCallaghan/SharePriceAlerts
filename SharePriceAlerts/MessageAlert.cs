namespace SharePriceAlerts
{
    using System;
    using System.Linq;
    using Exceptions;
    using Twilio;
    using Twilio.Rest.Api.V2010.Account;
    using Twilio.Types;

    public static class MessageAlert
    {
        public static AlertWhereRequired AlertWhereRequired = ruleOutComes =>
        {
            var failedSymbols = ruleOutComes
                .Where(x => x.TestPassed)
                .Select(x => x.Symbol)
                .ToList();

            if (!failedSymbols.Any()) return;

            var failedAggregate = failedSymbols.Aggregate((x, y) => $"{x}, {y}");

            AlertByTextMessage(failedAggregate);
        };

        private static void AlertByTextMessage(string failedSymbols)
        {
            try
            {
                var ssid = Environment.GetEnvironmentVariable("twilioSsid");
                var key = Environment.GetEnvironmentVariable("twilioKey");

                TwilioClient.Init(
                    ssid,
                    key);

                var fromNumber = Environment.GetEnvironmentVariable("twilioFromNumber");
                var toNumber = Environment.GetEnvironmentVariable("twilioToNumber");

                var _ = MessageResource.Create(
                    body: $"Happy days {failedSymbols}",
                    from: new PhoneNumber(fromNumber),
                    to: new PhoneNumber(toNumber)
                );
            }
            catch (Exception e)
            {
                throw new UnSuccessfulTwilioAlertException("Twilio Exception", e);
            }
        }
    }
}