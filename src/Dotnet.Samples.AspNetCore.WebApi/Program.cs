using Dotnet.Samples.AspNetCore.WebApi.Extensions;
using Serilog;

/* -----------------------------------------------------------------------------
 * Web Application
 * https://learn.microsoft.com/en-us/aspnet/core/fundamentals/startup
 * -------------------------------------------------------------------------- */

var builder = WebApplication.CreateBuilder(args);

/* Configurations ----------------------------------------------------------- */

builder
    .Configuration.SetBasePath(AppContext.BaseDirectory)
    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
    .AddEnvironmentVariables();

/* Logging ------------------------------------------------------------------ */

Log.Logger = new LoggerConfiguration().ReadFrom.Configuration(builder.Configuration).CreateLogger();
builder.Host.UseSerilog();

/* Controllers -------------------------------------------------------------- */

builder.Services.AddHealthChecks();
builder.Services.AddControllers();
builder.Services.AddValidators();
builder.Services.AddCorsDefaultPolicy(builder.Environment);
builder.Services.AddFixedWindowRateLimiter();

if (builder.Environment.IsDevelopment())
{
    builder.Services.AddSwaggerConfiguration(builder.Configuration);
}

/* Services ----------------------------------------------------------------- */

builder.Services.RegisterPlayerService();
builder.Services.AddMemoryCache();
builder.Services.AddMappings();

/* Repositories ------------------------------------------------------------- */

builder.Services.RegisterPlayerRepository();

/* Data --------------------------------------------------------------------- */

builder.Services.AddDbContextPoolWithSqlite(builder.Environment);

var app = builder.Build();

/* -----------------------------------------------------------------------------
 * Middlewares
 * https://learn.microsoft.com/en-us/aspnet/core/fundamentals/middleware
 * -------------------------------------------------------------------------- */

app.UseSerilogRequestLogging();
app.UseHttpsRedirection();
app.MapHealthChecks("/health");
app.UseRateLimiter();
app.MapControllers();

if (app.Environment.IsDevelopment())
{
    app.UseCors();
    app.UseSwagger();
    app.UseSwaggerUI();
}

await app.RunAsync();
