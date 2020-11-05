namespace SharePriceAlerts
{
    using System.Linq;
    using System.Threading.Tasks;

    using static TextMessageSender;
    using static SlackMessageSender;

    public static class MessageAlert
    {
        public static AlertWhereRequired AlertWhereRequired = (client, ruleOutComes) =>
        {
            var failedSymbols = ruleOutComes
                .Where(x => x.TestPassed)
                .Select(x => x.Symbol)
                .ToList();

            if (!failedSymbols.Any()) return Task.CompletedTask;

            var failedAggregate = failedSymbols.Aggregate((x, y) => $"{x}, {y}");

            SendTextSharePrice(failedAggregate);
            return SendSlackSharePrice(client, failedAggregate);
        };
    }
}