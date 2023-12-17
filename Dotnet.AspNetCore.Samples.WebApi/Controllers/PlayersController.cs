using Microsoft.AspNetCore.Mvc;
using Dotnet.AspNetCore.Samples.WebApi.Models;
using Dotnet.AspNetCore.Samples.WebApi.Services;

namespace Dotnet.AspNetCore.Samples.WebApi.Controllers;

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
        if (await _playerService.RetrieveById(player.Id) != null)
        {
            return Conflict();
        }
        else
        {
            await _playerService.Create(player);

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
        var players = await _playerService.Retrieve();
        
        if (players == null || players.Count == 0)
        {
            return NotFound();
        }
        else
        {
            return players;
        }
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Player>> GetPlayer(long id)
    {
        var player = await _playerService.RetrieveById(id);

        if (player == null)
        {
            return NotFound();
        }
        else
        {
            return player;
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
        else if (await _playerService.RetrieveById(id) == null)
        {
            return NotFound();
        }
        else
        {
            await _playerService.Update(player);

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
        if (await _playerService.RetrieveById(id) == null)
        {
            return NotFound();
        }
        else
        {
            await _playerService.Delete(id);

            return NoContent();
        }
    }
}

