using Dotnet.Samples.AspNetCore.WebApi;
using Dotnet.Samples.AspNetCore.WebApi.Models;
using Dotnet.Samples.AspNetCore.WebApi.Services;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

/* -----------------------------------------------------------------------------
 * Services
 * -------------------------------------------------------------------------- */

builder.Services.AddControllers();

var dataSource =
    $"{AppDomain.CurrentDomain.SetupInformation.ApplicationBase}/Data/players-sqlite3.db";

builder.Services.AddDbContext<PlayerContext>(options =>
    options.UseSqlite($"Data Source={dataSource}")
);

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
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

// Data seeding
// https://learn.microsoft.com/en-us/ef/core/modeling/data-seeding
PlayerContextInitializer.Seed(app);

app.Run();
