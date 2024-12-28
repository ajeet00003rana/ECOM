using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Project.DataAccess.DBContext;

namespace Project.Server.Configuration
{
    public static class RegisterRedis
    {        
        public static void ConfigureRedis(this WebApplicationBuilder builder)
        {
            builder.Services.AddStackExchangeRedisCache(options =>
            {
                options.Configuration = "localhost:6379"; 
                options.InstanceName = "ProductCache_"; 
            });

        }
    }
}
