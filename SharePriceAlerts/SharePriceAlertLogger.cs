namespace SharePriceAlerts
{
    using System;
    using System.Threading.Tasks;
    using Exceptions;
    using Microsoft.Extensions.Logging;

    using static TextMessageSender;
    using static SlackMessageSender;

    public class SharePriceAlertLogger : ISharePriceAlertLogger
    {
        private readonly ILogger _logger;
        private readonly IHttpClientFactory _clientFactory;

        public SharePriceAlertLogger(ILogger logger, IHttpClientFactory clientFactory) =>
            (_logger, _clientFactory) = (logger, clientFactory);

        public void LogInformation(string message) =>
            _logger.LogInformation(message);

        public Task LogError(Exception exception, string message)
        {
            _logger.LogError(exception, message);
            return exception is UnSuccessfulSlackException 
                ? Task.CompletedTask 
                : SendSlackLogError(_clientFactory.HttpClient, message);
        }

        public Task LogCritical(Exception exception, string message)
        {
            _logger.LogCritical(exception, message);

            if (!(exception is UnSuccessfulSlackException)) 
                SendTextLogError(message);

            return exception is UnSuccessfulTwilioAlertException
                ? Task.CompletedTask
                : SendSlackLogError(_clientFactory.HttpClient, message);
        }
    }
}
