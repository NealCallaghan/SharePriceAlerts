namespace SharePriceAlerts.Exceptions
{
    using System;
    
    [Serializable]
    public class MissingRuleException : Exception
    {
        public MissingRuleException(string symbol) 
            : base($"Missing rule for symbol {symbol}") { } 
    }
}