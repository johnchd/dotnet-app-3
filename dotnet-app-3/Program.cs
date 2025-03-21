using Serilog;
using System.IO;
using Serilog.Formatting.Json;

var builder = WebApplication.CreateBuilder(args);

// Add services before Build
builder.Services.AddOpenApi();
builder.Services.AddHttpClient();
builder.Services.AddControllers();

// Serilog setup
var logDirectory = "/tmp/logs";
if (!Directory.Exists(logDirectory))
{
    Directory.CreateDirectory(logDirectory);
}

Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
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
    .Enrich.WithProperty("Application", "dotnet-app-3")
    .Enrich.WithProperty("Environment", builder.Environment.EnvironmentName)
    .MinimumLevel.Information()
    .Filter.ByExcluding(log => log.MessageTemplate.Text.Contains("/favicon.ico"))
    .CreateLogger();

builder.Host.UseSerilog();

// Env Configuration
IConfigurationRoot configurationRoot = new ConfigurationBuilder()
    .AddJsonFile("appsettings.json")
    .AddEnvironmentVariables()
    .Build();

// Build the app
var app = builder.Build();

// Middleware pipeline
app.UseSerilogRequestLogging();
app.UseDefaultFiles();
app.UseStaticFiles();

// Middleware for API auth for BFF
app.Use(async (context, next) =>
{
    if (context.Request.Path.StartsWithSegments("/api"))
    {
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

app.UseHttpsRedirection();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.MapControllers();

app.Run();