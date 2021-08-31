namespace PaymentGateway.Api.ComponentTests.OutOfProcess
{
    using System;
    using System.Net.Http;

    public class OutOfProcessApplicationHost : IApplicationHost
    {
        public HttpClient CreateClient()
        {
            return new HttpClient
            {
                BaseAddress = new Uri("https://localhost:5001")
            };
        }
    }
}