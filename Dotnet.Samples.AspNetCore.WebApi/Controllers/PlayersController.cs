using Dotnet.Samples.AspNetCore.WebApi.Models;
using Dotnet.Samples.AspNetCore.WebApi.Services;
using Microsoft.AspNetCore.Mvc;

namespace Dotnet.Samples.AspNetCore.WebApi.Controllers;

/*
---------------------------------------------------------------------------------------------------
Scaffolded with dotnet-aspnet-codegenerator
https://learn.microsoft.com/en-us/aspnet/core/fundamentals/tools/dotnet-aspnet-codegenerator

dotnet aspnet-codegenerator controller -name PlayersController \
-async -api --model Player --dataContext PlayerContext \
-outDir Controllers
---------------------------------------------------------------------------------------------------
*/

[Route("api/[controller]")]
[ApiController]
public class PlayersController : ControllerBase
{
    private readonly IPlayerService _playerService;
    private readonly ILogger<PlayersController> _logger;

    public PlayersController(IPlayerService playerService, ILogger<PlayersController> logger)
    {
        _playerService = playerService;
        _logger = logger;
    }

    /*
    -----------------------------------------------------------------------------------------------
    HTTP POST
    To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    -----------------------------------------------------------------------------------------------
    */

    [HttpPost]
    public async Task<ActionResult<Player>> PostPlayer(Player player)
    {
        if (await _playerService.RetrieveByIdAsync(player.Id) != null)
        {
            return Conflict();
        }
        else
        {
            await _playerService.CreateAsync(player);

            return CreatedAtAction(nameof(GetPlayer), new { id = player.Id }, player);
        }
    }

    /*
    -----------------------------------------------------------------------------------------------
    HTTP GET
    -----------------------------------------------------------------------------------------------
    */

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Player>>> GetPlayers()
    {
        var players = await _playerService.RetrieveAsync();

        if (players.Any())
        {
            return players;
        }
        else
        {
            return NotFound();
        }
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Player>> GetPlayer(long id)
    {
        var player = await _playerService.RetrieveByIdAsync(id);

        if (player != null)
        {
            return player;
        }
        else
        {
            return NotFound();
        }
    }

    /*
    -----------------------------------------------------------------------------------------------
    HTTP PUT
    To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    -----------------------------------------------------------------------------------------------
    */

    [HttpPut("{id}")]
    public async Task<IActionResult> PutPlayer(long id, Player player)
    {
        if (id != player.Id)
        {
            return BadRequest();
        }
        else if (await _playerService.RetrieveByIdAsync(id) == null)
        {
            return NotFound();
        }
        else
        {
            await _playerService.UpdateAsync(player);

            return NoContent();
        }
    }

    /*
    -----------------------------------------------------------------------------------------------
    HTTP DELETE
    -----------------------------------------------------------------------------------------------
    */

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeletePlayer(long id)
    {
        if (await _playerService.RetrieveByIdAsync(id) == null)
        {
            return NotFound();
        }
        else
        {
            await _playerService.DeleteAsync(id);

            return NoContent();
        }
    }
}
