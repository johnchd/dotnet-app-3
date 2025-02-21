var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

builder.Services.AddControllers();
builder.Services.AddHttpClient();


var app = builder.Build();

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
