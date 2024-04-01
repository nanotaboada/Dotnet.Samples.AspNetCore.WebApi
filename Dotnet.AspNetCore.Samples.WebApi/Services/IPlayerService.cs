using Dotnet.AspNetCore.Samples.WebApi.Models;

namespace Dotnet.AspNetCore.Samples.WebApi.Services;

public interface IPlayerService
{
    public Task CreateAsync(Player player);

    public Task<List<Player>> RetrieveAsync();

    public ValueTask<Player?> RetrieveByIdAsync(long id);

    public Task UpdateAsync(Player player);

    public Task DeleteAsync(long id);
}
