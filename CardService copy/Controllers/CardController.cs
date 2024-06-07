// CardService/Controllers/CardController.cs
using Microsoft.AspNetCore.Mvc;

namespace CardService.Controllers
{
    [ApiController]
    [Route("api")]
    public class CardController : ControllerBase
    {
        [HttpGet("balance/{cardNumber}")]
        public IActionResult GetBalance(string cardNumber)
        {
            // Logic to get balance
            return Ok(100); // Example balance
        }

        [HttpPost("withdraw")]
        public IActionResult Withdraw([FromBody] WithdrawRequest request)
        {
            // Logic to withdraw amount
            return Ok();
        }
    }

    public class WithdrawRequest
    {
        public string CardNumber { get; set; }
        public decimal Amount { get; set; }
    }
}