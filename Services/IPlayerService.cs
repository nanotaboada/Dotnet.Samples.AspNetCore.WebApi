
using Dotnet.AspNetCore.Samples.WebApi.Models;

namespace Dotnet.AspNetCore.Samples.WebApi.Services;

public interface IPlayerService
{
    public Task Create(Player player);

    public Task<List<Player>> Retrieve();

    public ValueTask<Player?> RetrieveById(long id);

    public Task Update(Player player);

    public Task Delete(long id);
}
