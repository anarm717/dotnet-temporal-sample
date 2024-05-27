// LimitService/Controllers/LimitController.cs
using Microsoft.AspNetCore.Mvc;

namespace LimitService.Controllers
{
    [ApiController]
    [Route("api")]
    public class LimitController : ControllerBase
    {
        [HttpGet("{cardNumber}")]
        public IActionResult GetLimit(string cardNumber)
        {
            // Logic to get limit
            return Ok(50); // Example limit
        }
    }
}