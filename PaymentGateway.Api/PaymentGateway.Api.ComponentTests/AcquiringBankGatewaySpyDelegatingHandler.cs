namespace PaymentGateway.Api.Gateways
{
    using System.Net;
    using System.Net.Http;
    using System.Net.Http.Json;
    using System.Threading;
    using System.Threading.Tasks;
    using Models.Web;

    public class AcquiringBankGatewaySpyDelegatingHandler : DelegatingHandler
    {
        private readonly bool _isBroken;

        public AcquiringBankGatewaySpyDelegatingHandler(bool isBroken = false)
        {
            _isBroken = isBroken;
        }
        public string RequestUri { get; set; }
        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            RequestUri = request.RequestUri.AbsoluteUri;
            if (_isBroken)
            {
                return new HttpResponseMessage(HttpStatusCode.InternalServerError);
            }
            return await base.SendAsync(request, cancellationToken);
        }
    }
}