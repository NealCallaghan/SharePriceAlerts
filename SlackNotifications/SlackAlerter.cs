namespace SlackNotifications
{
    using System;
    using System.Net;
    using System.Net.Http;
    using System.Net.Http.Headers;
    using System.Text;
    using System.Threading.Tasks;
    using Models;
    using Newtonsoft.Json;

    public static class SlackAlerter
    {
        public static async Task<HttpStatusCode> AlertSlack(NotifyMessage message)
        {
            var oauthToken = Environment.GetEnvironmentVariable("oauthToken");
            var channelId = Environment.GetEnvironmentVariable("channelId");

            var client = new HttpClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", oauthToken);
            var dataPost = new
            {
                channel = channelId,
                text = $"{GetPreformedAlertLevel(message.AlertLevel)}{message.Message}",
            };
            var content = new StringContent(JsonConvert.SerializeObject(dataPost), Encoding.UTF8, "application/json");
            content.Headers.ContentType = MediaTypeHeaderValue.Parse("application/json");

            var response = await client.PostAsync("https://slack.com/api/chat.postMessage", content);
            return response.StatusCode;
        }

        private static string GetPreformedAlertLevel(AlertLevel alertLevel) =>
            alertLevel switch
            {
                AlertLevel.Debug => "Debug Message: ",
                AlertLevel.Normal => "Incoming Share Price News: ",
                AlertLevel.Error => "ERROR FOUND WITHING SYSTEM: ",
                _ => $"Unknown Message Type {nameof(AlertLevel)}: ",
            };
    }
}