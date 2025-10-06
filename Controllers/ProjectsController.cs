using Microsoft.AspNetCore.Mvc;
using zenBeat.Services;
using zenBeat.DTOs;

namespace zenBeat.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProjectsController : ControllerBase
    {
        private readonly IProjectService _projectService;

        public ProjectsController(IProjectService projectService)
        {
            _projectService = projectService;
        }

        /// <summary>
        /// 获取项目列表
        /// </summary>
        /// <param name="language">语言代码 (en-US, zh-CN, zh-TW)</param>
        /// <returns>项目列表</returns>
        [HttpGet]
        public async Task<ActionResult<List<ProjectDto>>> GetProjects()
        {
            try
            {
                // 从请求头获取语言参数，支持Accept-Language和language两种头
                var language = Request.Headers["Accept-Language"].FirstOrDefault() 
                             ?? Request.Headers["language"].FirstOrDefault() 
                             ?? "en-US";
                
                var projects = await _projectService.GetProjectsAsync(language);
                return Ok(projects);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Internal server error", error = ex.Message });
            }
        }
    }
}