using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace Project.Server.Configuration
{
    public static class RegisterJWT
    {
        public static void RegisterJwt(this WebApplicationBuilder builder)
        {
            //Jwt configuration starts here
            var configuration = builder.Configuration;
            var jwtIssuer = configuration.GetSection("JWT:ValidIssuer").Get<string>();
            var jwtAudience = configuration.GetSection("JWT:ValidAudience").Get<string>();
            var jwtKey = configuration.GetSection("Jwt:Secret").Get<string>();

            builder.Services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            })

             .AddJwtBearer(options =>
             {
                 options.SaveToken = true;
                 options.RequireHttpsMetadata = false;
                 options.TokenValidationParameters = new TokenValidationParameters()
                 {
                     ValidateIssuer = true,
                     ValidateAudience = true,
                     ValidateLifetime = true,
                     ValidateIssuerSigningKey = true,
                     ValidIssuer = jwtIssuer,
                     ValidAudience = jwtAudience,
                     IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey))
                 };
             });
            //Jwt configuration ends here

        }

    }
}
