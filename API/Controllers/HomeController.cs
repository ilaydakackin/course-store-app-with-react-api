using API.Entity;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [ApiController]
    [Route("")]
    public class HomeController : ControllerBase
    {
        [HttpGet]
        public IActionResult GetIndex()
        {
            return Ok(new { Message = "Healthy" });
        }
       
    }
}
