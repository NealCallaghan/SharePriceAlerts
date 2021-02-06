namespace SharePriceAlerts.Exceptions
{
    using System;

    [Serializable]
    public class UnSuccessfulScrapingException : Exception
    {
        public UnSuccessfulScrapingException(string message)
            : base(message) { }
    }
}