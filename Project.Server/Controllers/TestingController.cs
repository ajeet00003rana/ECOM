using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Project.DataAccess.Services;

namespace Project.Server.Controllers
{
    [Authorize]
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class TestingController : ControllerBase
    {
        private readonly IProjectService _service;
        private readonly ILogger<TestingController> _logger;

        public TestingController(IProjectService service, ILogger<TestingController> logger)
        {
            _service = service;
            _logger = logger;

        }

        /// <summary>
        /// https://localhost:7025/api/Testing/GetProjects
        /// pass token as bearer in Authrization header
        /// https://prnt.sc/pXY7pq30XWj-
        /// but this one dont need auth
        /// </summary>
        /// <returns></returns>

        [AllowAnonymous]
        [HttpGet]
        public IQueryable<Models.EntityModels.Project> GetProjects()
        {
            _logger.LogInformation("Fetching project data.");
            //return await _service.GetAllAsync();
            return _service.GetAllProject();
        }

        [HttpGet]
        public IActionResult Get()
        {
            return Ok("Hello World");
        }

        [Authorize(Roles = "Admin")]
        [HttpGet]
        public IActionResult Get12()
        {
            return Ok("Hello World");
        }
    }
}
