namespace SharePriceAlerts
{
    using System;
    using Exceptions;
    using Twilio;
    using Twilio.Rest.Api.V2010.Account;
    using Twilio.Types;

    public static class TextMessageSender
    {
        private static readonly string Ssid = Environment.GetEnvironmentVariable("twilioSsid");
        private static readonly string Key = Environment.GetEnvironmentVariable("twilioKey");
        private static readonly string FromNumber = Environment.GetEnvironmentVariable("twilioFromNumber");
        private static readonly string ToNumber = Environment.GetEnvironmentVariable("twilioToNumber");

        public static void SendTextLogError(string message)
        {
            try
            {
                SendTextMessage(message);
            }
            catch (Exception e)
            {
                throw new UnSuccessfulTwilioAlertException("Twilio Exception", e);
            }
        }

        public static void SendTextSharePrice(string symbols)
        {
            try
            {
                SendTextMessage($"Happy days {symbols}");
            }
            catch (Exception e)
            {
                throw new UnSuccessfulTwilioAlertException("Twilio Exception", e);
            }
        }

        private static void SendTextMessage(string body)
        {
            TwilioClient.Init(Ssid, Key);
            MessageResource.Create(
                body: body,
                from: new PhoneNumber(FromNumber),
                to: new PhoneNumber(ToNumber));
        }
            
    }
}