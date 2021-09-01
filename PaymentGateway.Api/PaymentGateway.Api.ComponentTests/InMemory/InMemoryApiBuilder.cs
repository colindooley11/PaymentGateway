namespace PaymentGateway.Api.ComponentTests.InMemory
{
    using System;
    using System.Net.Http;
    using Microsoft.Extensions.DependencyInjection;

    public class InMemoryApiBuilder : IApiBuilder
    {
        private readonly CustomWebApplicationFactory<TestStartup> _webApplicationFactory;

        public InMemoryApiBuilder(Action<IServiceCollection> collectionAction)
        {
            _webApplicationFactory = new CustomWebApplicationFactory<TestStartup>(collectionAction);
        }

        public HttpClient CreateClient()
        {
            return _webApplicationFactory.CreateClient();
        }
    }
}