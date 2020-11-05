namespace SharePriceAlerts.Exceptions
{
    using System;

    [Serializable]
    public class UnSuccessfulSlackException : Exception
    {
        public UnSuccessfulSlackException(string message, Exception e) : base(message, e) { }
    }
}