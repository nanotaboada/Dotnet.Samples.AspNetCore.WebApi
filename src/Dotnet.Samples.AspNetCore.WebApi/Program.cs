using Dotnet.Samples.AspNetCore.WebApi.Configurations;
using Dotnet.Samples.AspNetCore.WebApi.Data;
using Dotnet.Samples.AspNetCore.WebApi.Mappings;
using Dotnet.Samples.AspNetCore.WebApi.Models;
using Dotnet.Samples.AspNetCore.WebApi.Repositories;
using Dotnet.Samples.AspNetCore.WebApi.Services;
using Dotnet.Samples.AspNetCore.WebApi.Validators;
using FluentValidation;
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

/* Serilog ------------------------------------------------------------------ */
builder.Host.UseSerilog();

/* -----------------------------------------------------------------------------
 * Services
 * -------------------------------------------------------------------------- */
builder.Services.AddControllers();

/* Entity Framework Core ---------------------------------------------------- */
builder.Services.AddDbContextPool<PlayerDbContext>(options =>
{
    var dataSource = Path.Combine(AppContext.BaseDirectory, "Data", "players-sqlite3.db");
    options.UseSqlite($"Data Source={dataSource}");
    if (builder.Environment.IsDevelopment())
    {
        options.EnableSensitiveDataLogging();
        options.LogTo(Log.Logger.Information, LogLevel.Information);
    }
});

builder.Services.AddScoped<IPlayerRepository, PlayerRepository>();
builder.Services.AddScoped<IPlayerService, PlayerService>();
builder.Services.AddMemoryCache();

/* AutoMapper --------------------------------------------------------------- */
builder.Services.AddAutoMapper(typeof(PlayerMappingProfile));

/* FluentValidation --------------------------------------------------------- */
builder.Services.AddScoped<IValidator<PlayerRequestModel>, PlayerRequestModelValidator>();

if (builder.Environment.IsDevelopment())
{
    /* Swagger UI ----------------------------------------------------------- */
    // https://learn.microsoft.com/en-us/aspnet/core/tutorials/getting-started-with-swashbuckle
    builder.Services.AddSwaggerGen(options =>
    {
        options.SwaggerDoc("v1", builder.Configuration.GetSection("SwaggerDoc").Get<OpenApiInfo>());
        options.IncludeXmlComments(SwaggerGenDefaults.ConfigureXmlCommentsFilePath());
        options.AddSecurityDefinition("Bearer", SwaggerGenDefaults.ConfigureSecurityDefinition());
        options.OperationFilter<AuthorizeCheckOperationFilter>();
    });
}

var app = builder.Build();

/* -----------------------------------------------------------------------------
 * Middlewares
 * https://learn.microsoft.com/en-us/aspnet/core/fundamentals/middleware
 * -------------------------------------------------------------------------- */
app.UseSerilogRequestLogging();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// https://learn.microsoft.com/en-us/aspnet/core/security/enforcing-ssl
app.UseHttpsRedirection();

// https://learn.microsoft.com/en-us/aspnet/core/security/cors
app.UseCors();

// https://learn.microsoft.com/en-us/aspnet/core/fundamentals/routing#endpoints
app.MapControllers();

await app.RunAsync();
