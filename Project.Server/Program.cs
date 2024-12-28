using Project.Server.Configuration;
using Serilog;
using Microsoft.Extensions.Caching.StackExchangeRedis;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApiExplorer;


var builder = WebApplication.CreateBuilder(args);


//Seri-log start
Log.Logger = new LoggerConfiguration()
    .ReadFrom.Configuration(builder.Configuration) 
    .Enrich.FromLogContext()
    .WriteTo.Console()
    .WriteTo.File("logs/log-.txt", rollingInterval: RollingInterval.Day)
    .CreateLogger();

builder.Host.UseSerilog();

// Seri-log end

// Configure Redis connection
builder.Services.AddStackExchangeRedisCache(options =>
{
    // Replace with your Redis server details (this is an example connection string)
    options.Configuration = "localhost:6379"; // Change to your Redis server address
    options.InstanceName = "ProductCache_"; // Optional: Prefix for keys in Redis
});

// Add API versioning
builder.Services.AddApiVersioning(options =>
{
    options.AssumeDefaultVersionWhenUnspecified = true;
    options.DefaultApiVersion = new ApiVersion(1, 0);
    options.ReportApiVersions = true;
});




//Custom Configuration start

builder.ConfigureDBAndIdentity();
builder.RegisterJwt();
builder.Services.RegisterServices();

//Custom Configuration end

// Add services to the container.
builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
// Configure routing for versioning
builder.Services.AddVersionedApiExplorer(options =>
{
    options.GroupNameFormat = "'v'VVV"; // e.g., "v1", "v2"
    options.SubstituteApiVersionInUrl = true;
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

//Register custom middleware start
app.RegisterMiddleware();
//Register custom middleware end

app.MapControllers();

app.Run();
