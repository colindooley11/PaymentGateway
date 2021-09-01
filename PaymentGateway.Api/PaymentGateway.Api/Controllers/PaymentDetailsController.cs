namespace PaymentGateway.Api.Controllers
{
    using System;
    using System.Threading.Tasks;
    using Mapper;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Query;

    [Authorize]
    [Route("PaymentGateway/[controller]")]
    [ApiController]
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

            return new OkObjectResult(CardPaymentMapper.ToPaymentResponse(paymentDetails));
        }
    }
}
