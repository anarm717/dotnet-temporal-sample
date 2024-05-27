// IbanService/Controllers/IbanController.cs
using Microsoft.AspNetCore.Mvc;

namespace IbanService.Controllers
{
    [ApiController]
    [Route("api")]
    public class IbanController : ControllerBase
    {
        [HttpPost("topup")]
        public IActionResult TopUp([FromBody] TopUpRequest request)
        {
            // Logic to top-up amount
            return Ok();
        }
    }

    public class TopUpRequest
    {
        public string Iban { get; set; }
        public decimal Amount { get; set; }
    }
}