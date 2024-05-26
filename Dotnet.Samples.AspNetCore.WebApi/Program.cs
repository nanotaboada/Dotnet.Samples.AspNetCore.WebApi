using System.Reflection;
using Dotnet.Samples.AspNetCore.WebApi.Data;
using Dotnet.Samples.AspNetCore.WebApi.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging.Console;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

/* -----------------------------------------------------------------------------
 * Logging
 * -------------------------------------------------------------------------- */

if (builder.Environment.IsDevelopment())
{
    builder.Logging.ClearProviders();
    builder.Logging.AddSimpleConsole(options => options.SingleLine = true);
}

/* -----------------------------------------------------------------------------
 * Services
 * -------------------------------------------------------------------------- */

builder.Services.AddControllers();

var dataSource =
    $"{AppDomain.CurrentDomain.SetupInformation.ApplicationBase}/Data/players-sqlite3.db";

builder.Services.AddDbContextPool<PlayerDbContext>(options =>
{
    options.UseSqlite($"Data Source={dataSource}");

    if (builder.Environment.IsDevelopment())
    {
        options.EnableSensitiveDataLogging();
        options.LogTo(Console.WriteLine, LogLevel.Information);
    }
});

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc(
        "v1",
        new OpenApiInfo
        {
            Version = "v1",
            Title = "Dotnet.Samples.AspNetCore.WebApi",
            Description =
                "Proof of Concept for a Web API (Async) made with .NET 8 (LTS) and ASP.NET Core 8.0",
            Contact = new OpenApiContact
            {
                Name = "GitHub Repository",
                Url = new Uri("https://github.com/nanotaboada/Dotnet.Samples.AspNetCore.WebApi")
            },
            License = new OpenApiLicense
            {
                Name = "MIT License",
                Url = new Uri("https://opensource.org/license/mit")
            }
        }
    );

    // using System.Reflection;
    var filePath = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, filePath));
});

builder.Services.AddScoped<IPlayerService, PlayerService>();
builder.Services.AddMemoryCache();

var app = builder.Build();

/* -----------------------------------------------------------------------------
 * Middlewares
 * https://learn.microsoft.com/en-us/aspnet/core/fundamentals/middleware
 * -------------------------------------------------------------------------- */

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

// https://learn.microsoft.com/en-us/aspnet/core/security/cors
app.UseCors();
app.UseAuthorization();
app.MapControllers();

/* -----------------------------------------------------------------------------
 * Data Seeding
 * https://learn.microsoft.com/en-us/ef/core/modeling/data-seeding
 * -------------------------------------------------------------------------- */

app.SeedDbContext();

app.Run();
