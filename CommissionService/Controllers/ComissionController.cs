// CommissionService/Controllers/CommissionController.cs
using Microsoft.AspNetCore.Mvc;

namespace CommissionService.Controllers
{
    [ApiController]
    [Route("api")]
    public class CommissionController : ControllerBase
    {
        [HttpGet("{cardNumber}")]
        public IActionResult GetCommission(string cardNumber)
        {
            // Logic to get commission rate
            return Ok(0.1); // Example commission rate: 10%
        }
    }
}