namespace PaymentGateway.Api.IntegrationTests
{
    using System.Threading.Tasks;
    using Commands;
    using Microsoft.Azure.Documents.Client;
    using Models;

    public class SaveCardPaymentCommand : ISaveCardPaymentCommand
    {
        private readonly string _documentCollectionUri;
        private readonly DocumentClient _documentClient;

        public SaveCardPaymentCommand(string documentCollectionUri, DocumentClient documentClient)
        {
            _documentCollectionUri = documentCollectionUri;
            _documentClient = documentClient;
        }

        public async Task Execute(CardPayment cardPayment)
        {
            await _documentClient.CreateDocumentAsync(_documentCollectionUri, cardPayment);
        }
    }
}