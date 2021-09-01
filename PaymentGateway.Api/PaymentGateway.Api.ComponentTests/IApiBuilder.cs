namespace PaymentGateway.Api.ComponentTests
{
    using System.Net.Http;

    public interface IApiBuilder
    {
        HttpClient CreateClient();
    }
}