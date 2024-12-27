using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Project.DataAccess.DBContext;

namespace Project.Server.Configuration
{
    public static class RegisterIdentityAndDB
    {        
        public static void ConfigureDBAndIdentity(this WebApplicationBuilder builder)
        {
            builder.Services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("Default"))
            );


            // For Identity
            builder.Services.AddIdentity<IdentityUser, IdentityRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders();

            builder.Services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
            builder.Services.AddScoped(typeof(IService<>), typeof(Service<>));
        }
    }
}
