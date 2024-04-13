using Dotnet.Samples.AspNetCore.WebApi.Models;

namespace Dotnet.Samples.AspNetCore.WebApi.Services;

public interface IPlayerService
{
    public Task CreateAsync(Player player);

    public Task<List<Player>> RetrieveAsync();

    public ValueTask<Player?> RetrieveByIdAsync(long id);

    public Task UpdateAsync(Player player);

    public Task DeleteAsync(long id);
}
