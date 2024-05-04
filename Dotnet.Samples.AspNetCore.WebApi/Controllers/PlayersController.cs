using System.Net.Mime;
using Dotnet.Samples.AspNetCore.WebApi.Models;
using Dotnet.Samples.AspNetCore.WebApi.Services;
using Microsoft.AspNetCore.Mvc;

namespace Dotnet.Samples.AspNetCore.WebApi.Controllers;

[ApiController]
[Route("[controller]")]
[Produces("application/json")]
public class PlayersController(IPlayerService playerService, ILogger<PlayersController> logger)
    : ControllerBase
{
    private readonly IPlayerService _playerService = playerService;
    private readonly ILogger<PlayersController> _logger = logger;

    /* -------------------------------------------------------------------------
     * HTTP POST
     * ---------------------------------------------------------------------- */

    /// <summary>
    /// Creates a new Player
    /// </summary>
    /// <param name="player">The Player to create</param>
    /// <response code="409">The Player already exists</response>
    /// <response code="201">The Player was successfully created</response>
    [HttpPost]
    [Consumes(MediaTypeNames.Application.Json)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    [ProducesResponseType<Player>(StatusCodes.Status201Created)]
    public async Task<IResult> PostAsync([FromBody] Player player)
    {
        if (!ModelState.IsValid)
        {
            return TypedResults.BadRequest();
        }
        else if (await _playerService.RetrieveByIdAsync(player.Id) != null)
        {
            return TypedResults.Conflict();
        }
        else
        {
            await _playerService.CreateAsync(player);
            return TypedResults.Created($"/players/{player.Id}", player);
        }
    }

    /* -------------------------------------------------------------------------
     * HTTP GET
     * ---------------------------------------------------------------------- */

    /// <summary>
    /// Retrieves all Players
    /// </summary>
    /// <response code="404">Players not found</response>
    /// <response code="200">Players successfully retrieved</response>
    [HttpGet]
    [ProducesResponseType<Player>(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IResult> GetAsync()
    {
        var players = await _playerService.RetrieveAsync();

        if (players.Count > 0)
        {
            return TypedResults.Ok(players);
        }
        else
        {
            return TypedResults.NotFound();
        }
    }

    /// <summary>
    /// Retrieves a Player by Id
    /// </summary>
    /// <param name="id">The Id of the Player</param>
    /// <response code="404">Player not found</response>
    /// <response code="200">Player successfully retrieved</response>
    [HttpGet("{id}")]
    [ProducesResponseType<Player>(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IResult> GetByIdAsync([FromRoute] long id)
    {
        var player = await _playerService.RetrieveByIdAsync(id);

        if (player != null)
        {
            return TypedResults.Ok(player);
        }
        else
        {
            return TypedResults.NotFound();
        }
    }

    /* -------------------------------------------------------------------------
     * HTTP PUT
     * ---------------------------------------------------------------------- */

    /// <summary>
    /// Updates a Player by Id
    /// </summary>
    /// <param name="id">The Id of the Player</param>
    /// <param name="player">The Player to update</param>
    /// <response code="400">Player invalid</response>
    /// <response code="404">Player not found</response>
    /// <response code="204">Player successfully updated</response>
    [HttpPut("{id}")]
    [Consumes(MediaTypeNames.Application.Json)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IResult> PutAsync([FromRoute] long id, [FromBody] Player player)
    {
        if (!ModelState.IsValid)
        {
            return TypedResults.BadRequest();
        }
        else if (await _playerService.RetrieveByIdAsync(id) == null)
        {
            return TypedResults.NotFound();
        }
        else
        {
            await _playerService.UpdateAsync(player);

            return TypedResults.NoContent();
        }
    }

    /* -------------------------------------------------------------------------
     * HTTP DELETE
     * ---------------------------------------------------------------------- */

    /// <summary>
    /// Deletes a Player by Id
    /// </summary>
    /// <param name="id">The Id of the Player</param>
    /// <response code="404">Player not found</response>
    /// <response code="204">Player successfully deleted</response>
    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IResult> DeleteAsync([FromRoute] long id)
    {
        if (await _playerService.RetrieveByIdAsync(id) == null)
        {
            return TypedResults.NotFound();
        }
        else
        {
            await _playerService.DeleteAsync(id);

            return TypedResults.NoContent();
        }
    }
}
