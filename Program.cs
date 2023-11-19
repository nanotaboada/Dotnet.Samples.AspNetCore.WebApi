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
builder.Services.AddDbContext<PlayerContext>(options =>
    options.UseInMemoryDatabase("Players")
    );
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddScoped<IPlayerService, PlayerService>();

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

app.Run();
