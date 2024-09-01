using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SpringBootCloneApp.Controllers.RequestModels;
using SpringBootCloneApp.Controllers.ResponseModels;
using SpringBootCloneApp.Models;
using SpringBootCloneApp.Services;

namespace SpringBootCloneApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PaymentController : ControllerBase
    {
        private readonly IPaymentService _paymentService;

        public PaymentController(IPaymentService paymentService)
        {
            _paymentService = paymentService;
        }


        [HttpPost("create-payment")]
        public async Task<IActionResult> CreatePayment([FromBody] PaymentRequest paymentRequest)
        {
            try
            {
                var intentionResponse =  await _paymentService.CreateIntention(paymentRequest);
                var link = _paymentService.CreateLink(intentionResponse);
                return Ok(link);

            }
            catch(Exception)
            {
                return BadRequest();
            }
        }


        // /api/Payment/ProcessTransactionCallback

        [HttpPost("ProcessTransactionCallback")]
        public async Task<IActionResult> Process([FromBody] TransactionProcessedCallBackResponse response, [FromQuery]string hmac)
        {
            if (!_paymentService.IsHmacCorrect(response, hmac))
            {
                // ??
                Console.WriteLine("HMAC CORRECT!");
            }
            Console.WriteLine("Transaction Processed");
            var type = response.Type;
            return Ok();

        }


/*        [HttpPost("ProcessShippingData")]
        public async Task<IActionResult> ProcessShippingData([FromBody] ShippingData response)
        {

            Console.WriteLine("Transaction Processed");
            return Ok();

        }*/



    }
}
