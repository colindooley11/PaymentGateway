namespace PaymentGateway.Api.ComponentTests.OutOfProcess
{
    using System;
    using System.Net.Http;

    public class OutOfProcessApiBuilder : IApiBuilder
    {
        public HttpClient CreateClient()
        {
            return new HttpClient
            {
                BaseAddress = new Uri("https://paymentgateway-api.azurewebsites.net/")
            };
        }
    }
}