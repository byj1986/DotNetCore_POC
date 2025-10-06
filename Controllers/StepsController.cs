using Microsoft.AspNetCore.Mvc;
using zenBeat.Services;
using zenBeat.DTOs;

namespace zenBeat.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class StepsController : ControllerBase
    {
        private readonly IStepService _stepService;

        public StepsController(IStepService stepService)
        {
            _stepService = stepService;
        }

        /// <summary>
        /// 获取步骤列表
        /// </summary>
        /// <param name="language">语言代码 (en-US, zh-CN, zh-TW)</param>
        /// <returns>步骤列表</returns>
        [HttpGet]
        public async Task<ActionResult<List<StepDto>>> GetSteps()
        {
            try
            {
                // 从请求头获取语言参数，支持Accept-Language和language两种头
                var language = Request.Headers["Accept-Language"].FirstOrDefault() 
                             ?? Request.Headers["language"].FirstOrDefault() 
                             ?? "en-US";
                
                var steps = await _stepService.GetStepsAsync(language);
                return Ok(steps);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Internal server error", error = ex.Message });
            }
        }
    }
}