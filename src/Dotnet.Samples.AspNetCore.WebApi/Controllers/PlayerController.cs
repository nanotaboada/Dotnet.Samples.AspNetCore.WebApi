using System.Net.Mime;
using Dotnet.Samples.AspNetCore.WebApi.Models;
using Dotnet.Samples.AspNetCore.WebApi.Services;
using Microsoft.AspNetCore.Mvc;

namespace Dotnet.Samples.AspNetCore.WebApi.Controllers;

[ApiController]
[Route("[controller]")]
[Produces("application/json")]
public class PlayerController(IPlayerService playerService, ILogger<PlayerController> logger)
    : ControllerBase
{
    private readonly IPlayerService _playerService = playerService;
    private readonly ILogger<PlayerController> _logger = logger;

    /* -------------------------------------------------------------------------
     * HTTP POST
     * ---------------------------------------------------------------------- */

    /// <summary>
    /// Creates a new Player
    /// </summary>
    /// <param name="player">The PlayerRequestModel</param>
    /// <response code="201">Created</response>
    /// <response code="400">Bad Request</response>
    /// <response code="409">Conflict</response>
    [HttpPost]
    [Consumes(MediaTypeNames.Application.Json)]
    [ProducesResponseType<PlayerResponseModel>(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<IResult> PostAsync([FromBody] PlayerRequestModel player)
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
            var result = await _playerService.CreateAsync(player);
            return TypedResults.CreatedAtRoute(
                routeName: "GetById",
                routeValues: new { id = result.Id },
                value: result
            );
        }
    }

    /* -------------------------------------------------------------------------
     * HTTP GET
     * ---------------------------------------------------------------------- */

    /// <summary>
    /// Retrieves all Players
    /// </summary>
    /// <response code="200">OK</response>
    /// <response code="404">Not Found</response>
    [HttpGet]
    [ProducesResponseType<PlayerResponseModel>(StatusCodes.Status200OK)]
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
    /// Retrieves a Player by its ID
    /// </summary>
    /// <param name="id">The ID of the Player</param>
    /// <response code="200">OK</response>
    /// <response code="404">Not Found</response>
    [HttpGet("{id:long}", Name = "GetById")]
    [ProducesResponseType<PlayerResponseModel>(StatusCodes.Status200OK)]
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

    /// <summary>
    /// Retrieves a Player by its Squad Number
    /// </summary>
    /// <param name="squadNumber">The Squad Number of the Player</param>
    /// <response code="200">OK</response>
    /// <response code="404">Not Found</response>
    [HttpGet("squad/{squadNumber:int}")]
    [ProducesResponseType<PlayerResponseModel>(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IResult> GetBySquadNumberAsync([FromRoute] int squadNumber)
    {
        var player = await _playerService.RetrieveBySquadNumberAsync(squadNumber);

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
    /// Updates (entirely) a Player by its ID
    /// </summary>
    /// <param name="id">The ID of the Player</param>
    /// <param name="player">The PlayerRequestModel</param>
    /// <response code="204">No Content</response>
    /// <response code="400">Bad Request</response>
    /// <response code="404">Not Found</response>
    [HttpPut("{id}")]
    [Consumes(MediaTypeNames.Application.Json)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IResult> PutAsync([FromRoute] long id, [FromBody] PlayerRequestModel player)
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
    /// Deletes a Player by its ID
    /// </summary>
    /// <param name="id">The ID of the Player</param>
    /// <response code="204">No Content</response>
    /// <response code="404">Not Found</response>
    [HttpDelete("{id:long}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
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
