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
    private const string NotFoundTitle = "Not Found";

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
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status400BadRequest)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status409Conflict)]
    public async Task<IResult> PostAsync([FromBody] PlayerRequestModel player)
    {
        // Use the "Create" rule set, which includes BeUniqueSquadNumber.
        var validation = await validator.ValidateAsync(
            player,
            options => options.IncludeRuleSets("Create")
        );

        if (!validation.IsValid)
        {
            var errors = validation
                .Errors.GroupBy(e => e.PropertyName)
                .ToDictionary(g => g.Key, g => g.Select(e => e.ErrorMessage).ToArray());

            logger.LogWarning("POST /players validation failed: {@Errors}", errors);
            return TypedResults.ValidationProblem(
                errors,
                detail: "See the errors field for details.",
                instance: HttpContext?.Request?.Path.ToString()
            );
        }

        if (await playerService.RetrieveBySquadNumberAsync(player.SquadNumber) != null)
        {
            logger.LogWarning(
                "POST /players failed: Player with Squad Number {SquadNumber} already exists",
                player.SquadNumber
            );
            return TypedResults.Conflict(
                new ProblemDetails
                {
                    Type = "https://developer.mozilla.org/en-US/docs/Web/HTTP/Status/409",
                    Title = "Conflict",
                    Status = StatusCodes.Status409Conflict,
                    Detail = $"Player with Squad Number '{player.SquadNumber}' already exists.",
                    Instance = HttpContext?.Request?.Path.ToString()
                }
            );
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
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status404NotFound)]
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
            return TypedResults.Problem(
                statusCode: StatusCodes.Status404NotFound,
                title: NotFoundTitle,
                detail: "No players were found.",
                instance: HttpContext?.Request?.Path.ToString()
            );
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
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status404NotFound)]
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
            return TypedResults.Problem(
                statusCode: StatusCodes.Status404NotFound,
                title: NotFoundTitle,
                detail: $"Player with Id '{id}' was not found.",
                instance: HttpContext?.Request?.Path.ToString()
            );
        }
    }

    /// <summary>
    /// Retrieves a Player by its Squad Number
    /// </summary>
    /// <param name="squadNumber">The Squad Number of the Player</param>
    /// <response code="200">OK</response>
    /// <response code="404">Not Found</response>
    [HttpGet("squadNumber/{squadNumber:int}", Name = "RetrieveBySquadNumber")]
    [ProducesResponseType<PlayerResponseModel>(StatusCodes.Status200OK)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status404NotFound)]
    public async Task<IResult> GetBySquadNumberAsync([FromRoute] int squadNumber)
    {
        var player = await playerService.RetrieveBySquadNumberAsync(squadNumber);
        if (player != null)
        {
            logger.LogInformation(
                "GET /players/squadNumber/{SquadNumber} retrieved: {@Player}",
                squadNumber,
                player
            );
            return TypedResults.Ok(player);
        }
        else
        {
            logger.LogWarning("GET /players/squadNumber/{SquadNumber} not found", squadNumber);
            return TypedResults.Problem(
                statusCode: StatusCodes.Status404NotFound,
                title: NotFoundTitle,
                detail: $"Player with Squad Number '{squadNumber}' was not found.",
                instance: HttpContext?.Request?.Path.ToString()
            );
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
    [HttpPut("squadNumber/{squadNumber:int}", Name = "Update")]
    [Consumes(MediaTypeNames.Application.Json)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status400BadRequest)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status404NotFound)]
    public async Task<IResult> PutAsync(
        [FromRoute] int squadNumber,
        [FromBody] PlayerRequestModel player
    )
    {
        // Use the "Update" rule set, which omits BeUniqueSquadNumber.
        // The player being updated already exists in the database, so a
        // uniqueness check on its own squad number would always fail.
        var validation = await validator.ValidateAsync(
            player,
            options => options.IncludeRuleSets("Update")
        );
        if (!validation.IsValid)
        {
            var errors = validation
                .Errors.GroupBy(e => e.PropertyName)
                .ToDictionary(g => g.Key, g => g.Select(e => e.ErrorMessage).ToArray());

            logger.LogWarning(
                "PUT /players/squadNumber/{SquadNumber} validation failed: {@Errors}",
                squadNumber,
                errors
            );
            return TypedResults.ValidationProblem(
                errors,
                detail: "See the errors field for details.",
                instance: HttpContext?.Request?.Path.ToString()
            );
        }
        if (player.SquadNumber != squadNumber)
        {
            logger.LogWarning(
                "PutAsync squad number mismatch: route {SquadNumber} != body {PlayerSquadNumber}",
                squadNumber,
                player.SquadNumber
            );
            return TypedResults.Problem(
                statusCode: StatusCodes.Status400BadRequest,
                title: "Bad Request",
                detail: "Squad number in the route does not match squad number in the request body.",
                instance: HttpContext?.Request?.Path.ToString()
            );
        }
        if (await playerService.RetrieveBySquadNumberAsync(squadNumber) == null)
        {
            logger.LogWarning("PUT /players/squadNumber/{SquadNumber} not found", squadNumber);
            return TypedResults.Problem(
                statusCode: StatusCodes.Status404NotFound,
                title: NotFoundTitle,
                detail: $"Player with Squad Number '{squadNumber}' was not found.",
                instance: HttpContext?.Request?.Path.ToString()
            );
        }
        await playerService.UpdateAsync(player);
        // Sanitize user-provided player data before logging to prevent log forging
        var sanitizedPlayerString = player
            .ToString()
            ?.Replace(Environment.NewLine, string.Empty)
            .Replace("\r", string.Empty)
            .Replace("\n", string.Empty);
        logger.LogInformation(
            "PUT /players/squadNumber/{SquadNumber} updated: {Player}",
            squadNumber,
            sanitizedPlayerString
        );
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
    [HttpDelete("squadNumber/{squadNumber:int}", Name = "Delete")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status404NotFound)]
    public async Task<IResult> DeleteAsync([FromRoute] int squadNumber)
    {
        if (await playerService.RetrieveBySquadNumberAsync(squadNumber) == null)
        {
            logger.LogWarning("DELETE /players/squadNumber/{SquadNumber} not found", squadNumber);
            return TypedResults.Problem(
                statusCode: StatusCodes.Status404NotFound,
                title: "Not Found",
                detail: $"Player with Squad Number '{squadNumber}' was not found.",
                instance: HttpContext?.Request?.Path.ToString()
            );
        }
        else
        {
            await playerService.DeleteAsync(squadNumber);
            logger.LogInformation("DELETE /players/squadNumber/{SquadNumber} deleted", squadNumber);
            return TypedResults.NoContent();
        }
    }
}
