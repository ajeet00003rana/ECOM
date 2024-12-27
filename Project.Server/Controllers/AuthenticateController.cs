using Project.Server.Auth;
using Microsoft.AspNetCore.Mvc;
using Project.Models.Auth;

namespace Project.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticateController : ControllerBase
    {
        private readonly IAuthenticationService _authenticationService;

        public AuthenticateController(IAuthenticationService authenticationService)
        {
            _authenticationService = authenticationService;
        }

        /// <summary>
        /// https://localhost:7025/api/Authenticate/login
        /// {  "username": "user",  "password": "User@123"}
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> Login([FromBody] LoginModel model)
        {
            var response = await _authenticationService.LoginAsync(model);
            return StatusCode((int)response.Status, response);
        }

        [HttpPost]
        [Route("register")]
        public async Task<IActionResult> Register([FromBody] RegisterModel model)
        {
            var response = await _authenticationService.RegisterAsync(model, UserRoles.User);
            return StatusCode((int)response.Status, response);
        }

        [HttpPost]
        [Route("register-admin")]
        public async Task<IActionResult> RegisterAdmin([FromBody] RegisterModel model)
        {
            var response = await _authenticationService.RegisterAsync(model, UserRoles.Admin);
            return StatusCode((int)response.Status, response);
        }
    }
}
