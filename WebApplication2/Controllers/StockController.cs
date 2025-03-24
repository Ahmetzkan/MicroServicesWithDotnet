using Microsoft.AspNetCore.Mvc;

namespace WebApplication2.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class StockController : ControllerBase
    {
        [HttpGet]
        public IActionResult GetStockCount()
        {
            throw new Exception("db hatası");
            return Ok(new { Count = 100 });
        }
    }
}