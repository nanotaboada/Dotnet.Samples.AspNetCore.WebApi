using System.Reflection;
using Dotnet.Samples.AspNetCore.WebApi.Data;
using Dotnet.Samples.AspNetCore.WebApi.Mappings;
using Dotnet.Samples.AspNetCore.WebApi.Services;
using Dotnet.Samples.AspNetCore.WebApi.Utilities;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

/* -----------------------------------------------------------------------------
 * Configuration
 * -------------------------------------------------------------------------- */
builder
    .Configuration.SetBasePath(AppContext.BaseDirectory)
    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
    .AddEnvironmentVariables();

/* -----------------------------------------------------------------------------
 * Logging
 * -------------------------------------------------------------------------- */
Log.Logger = new LoggerConfiguration().ReadFrom.Configuration(builder.Configuration).CreateLogger();
builder.Host.UseSerilog();

/* -----------------------------------------------------------------------------
 * Services
 * -------------------------------------------------------------------------- */
builder.Services.AddControllers();
builder.Services.AddDbContextPool<PlayerDbContext>(options =>
{
    var dataSource = Path.Combine(AppContext.BaseDirectory, "Data", "players-sqlite3.db");
    options.UseSqlite($"Data Source={dataSource}");
    if (builder.Environment.IsDevelopment())
    {
        options.EnableSensitiveDataLogging();
        options.LogTo(Log.Logger.Information, LogLevel.Information);
        // https://learn.microsoft.com/en-us/ef/core/what-is-new/ef-core-9.0/whatsnew#improved-data-seeding
        options.UseAsyncSeeding(DbContextUtils.SeedAsync);
    }
});

builder.Services.AddScoped<IPlayerRepository, PlayerRepository>();
builder.Services.AddScoped<IPlayerService, PlayerService>();
builder.Services.AddMemoryCache();
builder.Services.AddAutoMapper(typeof(PlayerMappingProfile));

if (builder.Environment.IsDevelopment())
{
    builder.Services.AddSwaggerGen(options =>
    {
        options.SwaggerDoc(
            "v1",
            new OpenApiInfo
            {
                Version = "1.0.0",
                Title = "Dotnet.Samples.AspNetCore.WebApi",
                Description =
                    "🧪 Proof of Concept for a Web API (Async) made with .NET 8 (LTS) and ASP.NET Core 8.0",
                Contact = new OpenApiContact
                {
                    Name = "GitHub",
                    Url = new Uri("https://github.com/nanotaboada/Dotnet.Samples.AspNetCore.WebApi")
                },
                License = new OpenApiLicense
                {
                    Name = "MIT License",
                    Url = new Uri("https://opensource.org/license/mit")
                }
            }
        );

        var filePath = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
        options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, filePath));
    });
}

var app = builder.Build();

/* -----------------------------------------------------------------------------
 * Middlewares
 * https://learn.microsoft.com/en-us/aspnet/core/fundamentals/middleware
 * -------------------------------------------------------------------------- */
if (app.Environment.IsDevelopment())
{
    // https://learn.microsoft.com/en-us/aspnet/core/tutorials/getting-started-with-swashbuckle
    app.UseSwagger();
    app.UseSwaggerUI();
}

// https://github.com/serilog/serilog-aspnetcore
app.UseSerilogRequestLogging();

// https://learn.microsoft.com/en-us/aspnet/core/security/enforcing-ssl
app.UseHttpsRedirection();

// https://learn.microsoft.com/en-us/aspnet/core/security/cors
app.UseCors();

// https://learn.microsoft.com/en-us/aspnet/core/fundamentals/routing#endpoints
app.MapControllers();

await app.RunAsync();
