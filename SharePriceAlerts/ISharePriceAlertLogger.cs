namespace SharePriceAlerts
{
    using System;
    using System.Threading.Tasks;

    public interface ISharePriceAlertLogger
    {
        void LogInformation(string message);
        Task LogError(Exception exception, string message);
        Task LogCritical(Exception exception, string message);
    }
}