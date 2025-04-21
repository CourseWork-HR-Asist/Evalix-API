using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    [Route("test/v1/[controller]")]
    [ApiController]
    public class ToDeleteController : ControllerBase
    {
        [HttpGet("[action]")]
        public IActionResult TestMethod()
        {
            return Ok("Test");
        }
    }
}
