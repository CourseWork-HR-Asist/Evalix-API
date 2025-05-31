using Application.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    [Route("test/v1/[controller]")]
    [ApiController]
    public class ToDeleteController(ILLMService lLMService) : ControllerBase
    {
        [HttpGet("[action]")]
        public IActionResult TestMethod()
        {
            return Ok("Test");
        }

        [HttpPost("[action]")]
        public async Task<IActionResult> TestMethod([FromForm] IFormFile file)
        {
            var response = await lLMService.ExtractSkillsFromPdfAsync(file.OpenReadStream(), file.FileName);

            return Ok(response);
        }

        [HttpPost("[action]")]
        public async Task<IActionResult> TestMethod2([FromForm] IFormFile file)
        {
            var response = await lLMService.MatchRequirementsFromPdfAsync(file.OpenReadStream(), file.FileName, "Просто той хто знає мову програмування");

            return Ok(response);
        }
    }
}
