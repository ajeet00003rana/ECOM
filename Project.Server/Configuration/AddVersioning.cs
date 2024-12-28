using Microsoft.AspNetCore.Mvc;

namespace Project.Server.Configuration
{
    public static class AddVersioning
    {
        public static void ConfigureVersioning(this WebApplicationBuilder builder)
        {
            builder.Services.AddApiVersioning(options =>
            {
                options.AssumeDefaultVersionWhenUnspecified = true;
                options.DefaultApiVersion = new ApiVersion(1, 0);
                options.ReportApiVersions = true;
            });

            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
            // Configure routing for versioning
            builder.Services.AddVersionedApiExplorer(options =>
            {
                options.GroupNameFormat = "'v'VVV"; // e.g., "v1", "v2"
                options.SubstituteApiVersionInUrl = true;
            });
        }
    }
}
