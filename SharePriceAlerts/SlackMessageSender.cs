namespace SharePriceAlerts
{
    using System;
    using System.Net.Http;
    using System.Net.Http.Headers;
    using System.Text;
    using System.Threading.Tasks;
    using Exceptions;
    using Newtonsoft.Json;
    using SlackNotifications.Models;

    public static class SlackMessageSender
    {
        private static readonly string SlackAlerterUrl = Environment.GetEnvironmentVariable("SlackNotifierUrl");

        public static Task SendSlackLogError(HttpClient client, string message)
        {
            try
            {
                return SendMessage(client, AlertLevel.Error, message);
            }
            catch (Exception e)
            {
                throw new UnSuccessfulSlackException("Slack Exception", e);
            }
        }

        public static Task SendSlackSharePrice(HttpClient client, string symbols)
        {
            try
            {
                return SendMessage(client, AlertLevel.Normal, $"Happy days {symbols}");
            }
            catch (Exception e)
            {
                throw new UnSuccessfulSlackException("Slack Exception", e);
            }
        }

        public static Task SendMessage(HttpClient client, AlertLevel alertLevel, string messageToSend)
        {
            var dataPost = new
            {
                alertLevel = alertLevel.ToString(),
                message = messageToSend,
            };
            var content = new StringContent(JsonConvert.SerializeObject(dataPost), Encoding.UTF8, "application/json");
            content.Headers.ContentType = MediaTypeHeaderValue.Parse("application/json");

            return client.PostAsync(SlackAlerterUrl, content);
        }
    }
}