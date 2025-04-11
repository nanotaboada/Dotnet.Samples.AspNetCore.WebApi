using Microsoft.EntityFrameworkCore;

namespace Dotnet.Samples.AspNetCore.WebApi.Data;

public class Repository<T>(DbContext dbContext) : IRepository<T>
    where T : class
{
    protected readonly DbSet<T> _dbSet = dbContext.Set<T>();

    public async Task AddAsync(T entity)
    {
        await _dbSet.AddAsync(entity);
        await dbContext.SaveChangesAsync();
    }

    public async Task<List<T>> GetAllAsync() => await _dbSet.AsNoTracking().ToListAsync();

    public async ValueTask<T?> FindByIdAsync(long id) => await _dbSet.FindAsync(id);

    public async Task UpdateAsync(T entity)
    {
        _dbSet.Update(entity);
        await dbContext.SaveChangesAsync();
    }

    public async Task RemoveAsync(long id)
    {
        var entity = await _dbSet.FindAsync(id);
        if (entity != null)
        {
            _dbSet.Remove(entity);
            await dbContext.SaveChangesAsync();
        }
    }
}
