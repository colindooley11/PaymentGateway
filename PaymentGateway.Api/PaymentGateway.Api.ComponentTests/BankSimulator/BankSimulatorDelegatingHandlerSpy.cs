namespace PaymentGateway.Api.ComponentTests.BankSimulator
{
    using System.Net;
    using System.Net.Http;
    using System.Threading;
    using System.Threading.Tasks;

    public class BankSimulatorDelegatingHandlerSpy : DelegatingHandler
    {
        private readonly bool _withFailure;

        public BankSimulatorDelegatingHandlerSpy(bool withFailure = false)
        {
            _withFailure = withFailure;
        }

        public string RequestUri { get; set; }

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            RequestUri = request.RequestUri.AbsoluteUri;
            if (_withFailure)
            {
                return new HttpResponseMessage(HttpStatusCode.InternalServerError);
            }
            return await base.SendAsync(request, cancellationToken);
        }
    }
}