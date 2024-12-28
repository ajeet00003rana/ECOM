using Project.Server.Configuration;
using Serilog;

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

//Custom Configuration start

builder.ConfigureDBAndIdentity();
builder.RegisterJwt();
builder.Services.RegisterServices();

//Custom Configuration end

// Add services to the container.
builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

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
