using System.Net.Mime;
using Dotnet.Samples.AspNetCore.WebApi.Models;
using Dotnet.Samples.AspNetCore.WebApi.Services;
using Microsoft.AspNetCore.Mvc;

namespace Dotnet.Samples.AspNetCore.WebApi.Controllers;

[ApiController]
[Route("[controller]")]
public class PlayersController(IPlayerService playerService, ILogger<PlayersController> logger)
    : ControllerBase
{
    private readonly IPlayerService _playerService = playerService;
    private readonly ILogger<PlayersController> _logger = logger;

    /* -------------------------------------------------------------------------
     * HTTP POST
     * ---------------------------------------------------------------------- */

    [HttpPost]
    [Consumes(MediaTypeNames.Application.Json)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    [ProducesResponseType<Player>(StatusCodes.Status201Created)]
    public async Task<IResult> PostAsync(Player player)
    {
        if (!ModelState.IsValid)
        {
            return Results.BadRequest();
        }
        else if (await _playerService.RetrieveByIdAsync(player.Id) != null)
        {
            return Results.Conflict();
        }
        else
        {
            await _playerService.CreateAsync(player);
            var location = Url.Action(nameof(PostAsync), new { id = player.Id }) ?? $"/{player.Id}";

            return Results.Created(location, player);
        }
    }

    /* -------------------------------------------------------------------------
     * HTTP GET
     * ---------------------------------------------------------------------- */

    [HttpGet]
    [ProducesResponseType<Player>(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IResult> GetAsync()
    {
        var players = await _playerService.RetrieveAsync();

        if (players.Count > 0)
        {
            return Results.Ok(players);
        }
        else
        {
            return Results.NotFound();
        }
    }

    [HttpGet("{id}")]
    [ProducesResponseType<Player>(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IResult> GetByIdAsync(long id)
    {
        var player = await _playerService.RetrieveByIdAsync(id);

        if (player != null)
        {
            return Results.Ok(player);
        }
        else
        {
            return Results.NotFound();
        }
    }

    /* -------------------------------------------------------------------------
     * HTTP PUT
     * ---------------------------------------------------------------------- */

    [HttpPut("{id}")]
    [Consumes(MediaTypeNames.Application.Json)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IResult> PutAsync(long id, Player player)
    {
        if (!ModelState.IsValid)
        {
            return Results.BadRequest();
        }
        else if (await _playerService.RetrieveByIdAsync(id) == null)
        {
            return Results.NotFound();
        }
        else
        {
            await _playerService.UpdateAsync(player);

            return Results.NoContent();
        }
    }

    /* -------------------------------------------------------------------------
     * HTTP DELETE
     * ---------------------------------------------------------------------- */

    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IResult> DeleteAsync(long id)
    {
        if (await _playerService.RetrieveByIdAsync(id) == null)
        {
            return Results.NotFound();
        }
        else
        {
            await _playerService.DeleteAsync(id);

            return Results.NoContent();
        }
    }
}
