using Serilog; // serilog
using System; // serilog
using System.IO; // serilog
using Serilog.Formatting.Json; // serilog



var builder = WebApplication.CreateBuilder(args); // main


builder.Services.AddOpenApi();
builder.Services.AddControllers();
builder.Services.AddHttpClient();

// serilog
var logDirectory = "/tmp/logs";
if (!Directory.Exists(logDirectory))
{
    Directory.CreateDirectory(logDirectory);
}
// iteration 1 - basic
// Log.Logger = new LoggerConfiguration()
//     // .WriteTo.Console()
//     .WriteTo.File(Path.Combine(logDirectory, "logs.txt"), rollingInterval: RollingInterval.Infinite) // 
//     .Enrich.FromLogContext()
//     .CreateLogger();

Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()  // Logs to console
    .WriteTo.File(
        Path.Combine(logDirectory, "logs-.txt"),
        rollingInterval: RollingInterval.Day,
        outputTemplate: "{Timestamp:yyyy-MM-dd HH:mm:ss.fff zzz} [{Level}] {Message}{NewLine}{Exception}"
    )
    .WriteTo.File(
        new JsonFormatter(), 
        Path.Combine(logDirectory, "logs-.json"), 
        rollingInterval: RollingInterval.Day 
    )
    .Enrich.FromLogContext()
    .Enrich.WithProperty("Application", "dotnet-app-3") // Add application name
    .Enrich.WithProperty("Environment", builder.Environment.EnvironmentName) // Add environment
    .MinimumLevel.Information() 
    .Filter.ByExcluding(log => log.MessageTemplate.Text.Contains("/favicon.ico")) // Ignore favicon.ico requests
    .CreateLogger();

builder.Host.UseSerilog();
// var appSerilog = builder.Build();
var app = builder.Build();

app.UseSerilogRequestLogging(); // Logs HTTP requests



// builder.Services.AddOpenApi();
// builder.Services.AddControllers();
// builder.Services.AddHttpClient();


// env var - local vs docker
IConfigurationRoot configurationRoot = new ConfigurationBuilder()
    .AddJsonFile("appsettings.json")
    .AddEnvironmentVariables()
.Build();


// var app = builder.Build(); // had to move up bc of serilog
//test for secret scanning via codeQL
string password = "adfasdf12311";  // Hardcoded secret test

app.UseDefaultFiles();
app.UseStaticFiles();

//attempt at auth
app.Use(async (context, next) =>
{
    // Check if the request is to an /api endpoint
    if (context.Request.Path.StartsWithSegments("/api"))
    {
        // Try to get the custom header
        if (!context.Request.Headers.TryGetValue("X-Secret-Key", out var headerValue) ||
            headerValue != "test")
        {
            context.Response.StatusCode = 401; // Unauthorized
            await context.Response.WriteAsync("Unauthorized");
            return;
        }
    }
    await next();
});


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

//test

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
