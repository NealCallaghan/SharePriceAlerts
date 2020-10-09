namespace SharePriceAlerts.Exceptions
{
    using System;

    [Serializable]
    public class UnSuccessfulTwilioAlertException : Exception
    {
        public UnSuccessfulTwilioAlertException(string message, Exception innerException)
            : base(message, innerException) { }
    }
}