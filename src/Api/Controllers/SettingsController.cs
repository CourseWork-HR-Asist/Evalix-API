using Application.Services.Interfaces;
using Application.Services.LLM;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SettingsController : ControllerBase
    {
        private readonly ILLMSettingsService _llmSettingsService;

        public SettingsController(ILLMSettingsService llmSettingsService)
        {
            _llmSettingsService = llmSettingsService;
        }

        [HttpGet("[action]")]
        public ActionResult<LLMSetting> GetLLMSettings()
        {
            return Ok(_llmSettingsService.Settings);
        }

        [HttpPost("[action]")]
        public IActionResult UpdateLLMSettings([FromBody] LLMSetting updatedSettings)
        {
            _llmSettingsService.Update(updatedSettings);
            return NoContent();
        }
    }
}
