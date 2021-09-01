namespace PaymentGateway.Api.Controllers
{
    using System;
    using System.Threading.Tasks;
    using Filters;
    using Microsoft.AspNetCore.Mvc;
    using Query;

    [Route("PaymentGateway/[controller]")]
    [ApiController]
    [ApiKey]
    public class PaymentDetailsController : ControllerBase
    {
        private readonly IGetPaymentDetailsQuery _getPaymentDetailsCosmosQuery;

        public PaymentDetailsController(IGetPaymentDetailsQuery getPaymentDetailsCosmosQuery)
        {
            _getPaymentDetailsCosmosQuery = getPaymentDetailsCosmosQuery;
        }

        [HttpGet]
        [Route("{paymentReference:guid}")]
        public async Task<IActionResult> RetrievePaymentDetails(Guid paymentReference)
        {
            var paymentDetails = await _getPaymentDetailsCosmosQuery.Execute(paymentReference);
            if (paymentDetails == null)
            {
                return new NotFoundResult(); 
            }

            return new OkObjectResult(paymentDetails);
        }
    }
}
