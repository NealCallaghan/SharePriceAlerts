namespace SlackNotifications
{
    using System;
    using System.IO;
    using System.Net;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Azure.WebJobs;
    using Microsoft.Azure.WebJobs.Extensions.Http;
    using Microsoft.AspNetCore.Http;
    using Microsoft.Extensions.Logging;
    using Newtonsoft.Json;
    using Models;

    using static SlackAlerter;

    public static class SlackNotify
    {
        [FunctionName("SlackNotify")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "SlackNotify")] HttpRequest req,
            ILogger log)
        {
            var executeCatchAllWithLogger = ExecuteCatchAll(log);

            var result = await executeCatchAllWithLogger(async () =>
            {
                var message = await RetrieveObjectFromRequestBody<NotifyMessage>(req.Body);
                return await AlertSlack(message);
            });

            return new StatusCodeResult((int)result);
        }

        private static async Task<T> RetrieveObjectFromRequestBody<T>(Stream body)
        {
            var requestBody = await new StreamReader(body).ReadToEndAsync();
            return JsonConvert.DeserializeObject<T>(requestBody);
        }

        private static Func<Func<Task<HttpStatusCode>>, Task<HttpStatusCode>> ExecuteCatchAll(ILogger log) =>
            async func =>
            {
                try
                {
                    log.LogInformation($"Slack notifications function started executing at: {DateTime.Now}");
                    var resultCode = await Task.Run(func);
                    log.LogInformation($"Slack notifications function finished executing at: {DateTime.Now}");
                    return resultCode;
                }
                catch (JsonSerializationException e)
                {
                    log.LogError(e, $"Serialization issue at {DateTime.Now}");
                    return HttpStatusCode.BadRequest;
                }
                catch (Exception e)
                {
                    log.LogCritical(e, $"Critical exception thrown at {DateTime.Now}");
                    return HttpStatusCode.InternalServerError;
                }
            };
    }
}
