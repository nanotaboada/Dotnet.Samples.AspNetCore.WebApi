using System.Net.Mime;
using Dotnet.Samples.AspNetCore.WebApi.Models;
using Dotnet.Samples.AspNetCore.WebApi.Services;
using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Dotnet.Samples.AspNetCore.WebApi.Controllers;

[ApiController]
[Route("players")]
[Produces("application/json")]
public class PlayerController(
    IPlayerService playerService,
    ILogger<PlayerController> logger,
    IValidator<PlayerRequestModel> validator
) : ControllerBase
{
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
    [HttpPost(Name = "Create")]
    [Consumes(MediaTypeNames.Application.Json)]
    [ProducesResponseType<PlayerResponseModel>(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<IResult> PostAsync([FromBody] PlayerRequestModel player)
    {
        var validation = await validator.ValidateAsync(player);

        if (!validation.IsValid)
        {
            var errors = validation
                .Errors.Select(error => new { error.PropertyName, error.ErrorMessage })
                .ToArray();

            logger.LogWarning("POST /players validation failed: {@Errors}", errors);
            return TypedResults.BadRequest(errors);
        }

        if (await playerService.RetrieveBySquadNumberAsync(player.SquadNumber) != null)
        {
            logger.LogWarning(
                "POST /players failed: Player with Squad Number {SquadNumber} already exists",
                player.SquadNumber
            );
            return TypedResults.Conflict();
        }

        var result = await playerService.CreateAsync(player);

        logger.LogInformation("POST /players created: {@Player}", result);
        return TypedResults.CreatedAtRoute(
            routeName: "RetrieveBySquadNumber",
            routeValues: new { squadNumber = result.Dorsal },
            value: result
        );
    }

    /* -------------------------------------------------------------------------
     * HTTP GET
     * ---------------------------------------------------------------------- */

    /// <summary>
    /// Retrieves all Players
    /// </summary>
    /// <response code="200">OK</response>
    /// <response code="404">Not Found</response>
    [HttpGet(Name = "Retrieve")]
    [ProducesResponseType<PlayerResponseModel>(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IResult> GetAsync()
    {
        var players = await playerService.RetrieveAsync();

        if (players.Count > 0)
        {
            logger.LogInformation("GET /players retrieved");
            return TypedResults.Ok(players);
        }
        else
        {
            logger.LogWarning("GET /players not found");
            return TypedResults.NotFound();
        }
    }

    /// <summary>
    /// Retrieves a Player by its internal Id (GUID)
    /// </summary>
    /// <param name="id">The internal Id (GUID) of the Player</param>
    /// <response code="200">OK</response>
    /// <response code="404">Not Found</response>
    [Authorize]
    [HttpGet("{id:Guid}", Name = "RetrieveById")]
    [ProducesResponseType<PlayerResponseModel>(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IResult> GetByIdAsync([FromRoute] Guid id)
    {
        var player = await playerService.RetrieveByIdAsync(id);
        if (player != null)
        {
            logger.LogInformation("GET /players/{Id} retrieved: {@Player}", id, player);
            return TypedResults.Ok(player);
        }
        else
        {
            logger.LogWarning("GET /players/{Id} not found", id);
            return TypedResults.NotFound();
        }
    }

    /// <summary>
    /// Retrieves a Player by its Squad Number
    /// </summary>
    /// <param name="squadNumber">The Squad Number of the Player</param>
    /// <response code="200">OK</response>
    /// <response code="404">Not Found</response>
    [HttpGet("{squadNumber:int}", Name = "RetrieveBySquadNumber")]
    [ProducesResponseType<PlayerResponseModel>(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IResult> GetBySquadNumberAsync([FromRoute] int squadNumber)
    {
        var player = await playerService.RetrieveBySquadNumberAsync(squadNumber);
        if (player != null)
        {
            logger.LogInformation(
                "GET /players/{SquadNumber} retrieved: {@Player}",
                squadNumber,
                player
            );
            return TypedResults.Ok(player);
        }
        else
        {
            logger.LogWarning("GET /players/{SquadNumber} not found", squadNumber);
            return TypedResults.NotFound();
        }
    }

    /* -------------------------------------------------------------------------
     * HTTP PUT
     * ---------------------------------------------------------------------- */

    /// <summary>
    /// Updates (entirely) a Player by its Squad Number
    /// </summary>
    ///
    /// <param name="player">The PlayerRequestModel</param>
    /// <param name="squadNumber">The Squad Number of the Player</param>
    /// <response code="204">No Content</response>
    /// <response code="400">Bad Request</response>
    /// <response code="404">Not Found</response>
    [HttpPut("{squadNumber:int}", Name = "Update")]
    [Consumes(MediaTypeNames.Application.Json)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IResult> PutAsync(
        [FromRoute] int squadNumber,
        [FromBody] PlayerRequestModel player
    )
    {
        var validation = await validator.ValidateAsync(player);
        if (!validation.IsValid)
        {
            var errors = validation
                .Errors.Select(error => new { error.PropertyName, error.ErrorMessage })
                .ToArray();

            logger.LogWarning(
                "PUT /players/{squadNumber} validation failed: {@Errors}",
                squadNumber,
                errors
            );
            return TypedResults.BadRequest(errors);
        }
        if (await playerService.RetrieveBySquadNumberAsync(squadNumber) == null)
        {
            logger.LogWarning("PUT /players/{SquadNumber} not found", squadNumber);
            return TypedResults.NotFound();
        }
        await playerService.UpdateAsync(player);
        logger.LogInformation("PUT /players/{SquadNumber} updated: {@Player}", squadNumber, player);
        return TypedResults.NoContent();
    }

    /* -------------------------------------------------------------------------
     * HTTP DELETE
     * ---------------------------------------------------------------------- */

    /// <summary>
    /// Deletes a Player by its Squad Number
    /// </summary>
    /// <param name="squadNumber">The Squad Number of the Player</param>
    /// <response code="204">No Content</response>
    /// <response code="404">Not Found</response>
    [HttpDelete("{squadNumber:int}", Name = "Delete")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IResult> DeleteAsync([FromRoute] int squadNumber)
    {
        if (await playerService.RetrieveBySquadNumberAsync(squadNumber) == null)
        {
            logger.LogWarning("DELETE /players/{SquadNumber} not found", squadNumber);
            return TypedResults.NotFound();
        }
        else
        {
            await playerService.DeleteAsync(squadNumber);
            logger.LogInformation("DELETE /players/{SquadNumber} deleted", squadNumber);
            return TypedResults.NoContent();
        }
    }
}
