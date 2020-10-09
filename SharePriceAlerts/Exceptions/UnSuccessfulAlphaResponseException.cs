namespace SharePriceAlerts.Exceptions
{
    using System;

    [Serializable]
    public class UnSuccessfulAlphaResponseException : Exception
    {
        public UnSuccessfulAlphaResponseException(string message)
            : base(message) { }
    }
}