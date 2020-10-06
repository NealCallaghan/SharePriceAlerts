namespace SharePriceAlerts.Exceptions
{
    using System;

    public class MissingRuleException : Exception
    {
        public MissingRuleException(string symbol) =>
            Message = $"Missing rule for symbol {symbol}";
        
        public override string Message { get; }
    }
}