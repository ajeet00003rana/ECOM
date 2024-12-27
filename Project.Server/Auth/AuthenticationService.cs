using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using Project.Models.Auth;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Security.Claims;
using System.Text;

namespace Project.Server.Auth
{
    public interface IAuthenticationService
    {
        Task<AuthenticationResponse> LoginAsync(LoginModel model);
        Task<AuthenticationResponse> RegisterAsync(RegisterModel model, params string[] roles);
    }

    public class AuthenticationService : IAuthenticationService
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IConfiguration _configuration;

        public AuthenticationService(
            UserManager<IdentityUser> userManager,
            IConfiguration configuration)
        {
            _userManager = userManager;
            _configuration = configuration;
        }

        public async Task<AuthenticationResponse> LoginAsync(LoginModel model)
        {
            var user = await _userManager.FindByNameAsync(model.Username);
            if (user != null && await _userManager.CheckPasswordAsync(user, model.Password))
            {
                var userRoles = await _userManager.GetRolesAsync(user);

                var authClaims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, user.UserName),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                };

                foreach (var userRole in userRoles)
                {
                    authClaims.Add(new Claim(ClaimTypes.Role, userRole));
                }

                var token = GetToken(authClaims);

                return new AuthenticationResponse
                {
                    Status = HttpStatusCode.OK,
                    Token = new JwtSecurityTokenHandler().WriteToken(token),
                    Expiration = token.ValidTo
                };
            }
            return new AuthenticationResponse
            {
                Status = HttpStatusCode.Unauthorized,
                Message = "Invalid username or password."
            };
        }

        public async Task<AuthenticationResponse> RegisterAsync(RegisterModel model, params string[] roles)
        {
            var result = await CreateUserAsync(model, roles);

            return new AuthenticationResponse
            {
                Status = result.Succeeded ? HttpStatusCode.OK : HttpStatusCode.InternalServerError,
                Message = result.Succeeded ? "User created successfully!" : "User creation failed! Please check user details and try again.",
            };
        }

        private async Task<IdentityResult> CreateUserAsync(RegisterModel model, params string[] roles)
        {
            var userExists = await _userManager.FindByNameAsync(model.Username);

            if (userExists != null)
                return IdentityResult.Failed(new IdentityError { Description = "User already exists!" });

            var user = new IdentityUser
            {
                Email = model.Email,
                SecurityStamp = Guid.NewGuid().ToString(),
                UserName = model.Username
            };

            var result = await _userManager.CreateAsync(user, model.Password);

            if (result.Succeeded && roles.Any())
                await _userManager.AddToRolesAsync(user, roles);

            return result;
        }

        private JwtSecurityToken GetToken(List<Claim> authClaims)
        {
            var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:Secret"]));

            var token = new JwtSecurityToken(
                issuer: _configuration["JWT:ValidIssuer"],
                audience: _configuration["JWT:ValidAudience"],
                expires: DateTime.Now.AddHours(3),
                claims: authClaims,
                signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
            );

            return token;
        }

    }

}
