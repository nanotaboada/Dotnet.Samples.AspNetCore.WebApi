using Dotnet.AspNetCore.Samples.WebApi;
using Dotnet.AspNetCore.Samples.WebApi.Models;
using Dotnet.AspNetCore.Samples.WebApi.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

/*
-----------------------------------------------------------------------------------------------
Services
-----------------------------------------------------------------------------------------------
*/

builder.Services.AddControllers();

// var localApplicationData = Environment.SpecialFolder.LocalApplicationData;
// var folderPath = Environment.GetFolderPath(localApplicationData);
var projectDataFolder = "Data";
var path = Path.Join(projectDataFolder, "players-sqlite3.db");

builder.Services.AddDbContext<PlayerContext>(options =>
    options.UseSqlite($"Data Source={path}")
    );

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddScoped<IPlayerService, PlayerService>();
builder.Services.AddMemoryCache();

var app = builder.Build();

/*
-----------------------------------------------------------------------------------------------
Middlewares
https://learn.microsoft.com/en-us/aspnet/core/fundamentals/middleware
-----------------------------------------------------------------------------------------------
*/

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
