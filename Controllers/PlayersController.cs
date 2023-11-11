namespace Dotnet.AspNetCore.Samples.WebApi.Controllers;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Dotnet.AspNetCore.Samples.WebApi.Models;

/*
    Scaffolded with dotnet-aspnet-codegenerator
    https://learn.microsoft.com/en-us/aspnet/core/fundamentals/tools/dotnet-aspnet-codegenerator?view=aspnetcore-7.0

    dotnet aspnet-codegenerator controller -name PlayersController \
    -async -api --model Player --dataContext PlayerContext \ 
    -outDir Controllers
*/
[Route("api/[controller]")]
[ApiController]
public class PlayersController : ControllerBase
{
    private readonly PlayerContext _context;

    public PlayersController(PlayerContext context)
    {
        _context = context;
    }

    // GET: api/Players
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Player>>> GetPlayers()
    {
        if (_context.Players == null)
        {
            return NotFound();
        }
        return await _context.Players.ToListAsync();
    }

    // GET: api/Players/5
    [HttpGet("{id}")]
    public async Task<ActionResult<Player>> GetPlayer(long id)
    {
        if (_context.Players == null)
        {
            return NotFound();
        }
        var player = await _context.Players.FindAsync(id);

        if (player == null)
        {
            return NotFound();
        }

        return player;
    }

    // PUT: api/Players/5
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    [HttpPut("{id}")]
    public async Task<IActionResult> PutPlayer(long id, Player player)
    {
        if (id != player.Id)
        {
            return BadRequest();
        }

        _context.Entry(player).State = EntityState.Modified;

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!PlayerExists(id))
            {
                return NotFound();
            }
            else
            {
                throw;
            }
        }

        return NoContent();
    }

    // POST: api/Players
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    [HttpPost]
    public async Task<ActionResult<Player>> PostPlayer(Player player)
    {
        if (_context.Players == null)
        {
            return Problem("Entity set 'PlayerContext.Players'  is null.");
        }
        _context.Players.Add(player);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetPlayer), new { id = player.Id }, player);
    }

    // DELETE: api/Players/5
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeletePlayer(long id)
    {
        if (_context.Players == null)
        {
            return NotFound();
        }
        var player = await _context.Players.FindAsync(id);
        if (player == null)
        {
            return NotFound();
        }

        _context.Players.Remove(player);
        await _context.SaveChangesAsync();

        return NoContent();
    }

    private bool PlayerExists(long id)
    {
        return (_context.Players?.Any(e => e.Id == id)).GetValueOrDefault();
    }
}

