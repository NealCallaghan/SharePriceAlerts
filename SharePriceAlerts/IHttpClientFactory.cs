namespace SharePriceAlerts
{
    using System.Net.Http;

    public interface IHttpClientFactory
    {
        public HttpClient HttpClient { get; }
    }
}