using Project.Server.Configuration;
using Serilog;


var builder = WebApplication.CreateBuilder(args);


#region Custom configuration
//Seri-log start
Log.Logger = new LoggerConfiguration()
    .ReadFrom.Configuration(builder.Configuration)
    .Enrich.FromLogContext()
    .WriteTo.Console()
    .WriteTo.File("logs/log-.txt", rollingInterval: RollingInterval.Day)
    .CreateLogger();

builder.Host.UseSerilog();

// Seri-log end

builder.ConfigureRedis();


builder.ConfigureDBAndIdentity();
builder.RegisterJwt();
builder.Services.RegisterServices();

// Add API versioning
builder.ConfigureVersioning();

#endregion

//Custom Configuration end

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowSpecificOrigin", policy =>
    {
        policy.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod();
    });
});

// Add services to the container.
builder.Services.AddControllers();

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

app.UseCors("AllowSpecificOrigin");

app.MapControllers();

app.Run();
