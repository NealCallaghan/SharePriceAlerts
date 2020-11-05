namespace SharePriceAlerts
{
    using System;
    using System.Net.Http;

    public class HttpClientFactory : IHttpClientFactory
    {
        private static readonly Lazy<HttpClient> LazyClientFunc = new Lazy<HttpClient>(() => new HttpClient());

        public HttpClient HttpClient => LazyClientFunc.Value;
    }
}