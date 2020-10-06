namespace SharePriceAlerts.Exceptions
{
    using System;

    public class UnSuccessfulAlphaResponse : Exception
    {
        public UnSuccessfulAlphaResponse(string message) =>
            Message = message;
        
        public override string Message { get; }
    }
}