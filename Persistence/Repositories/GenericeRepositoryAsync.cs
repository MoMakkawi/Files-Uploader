using Application.Contracts;

using Microsoft.EntityFrameworkCore;

using Persistence.Data;
namespace Persistence.Services.Repositories;

public class GenericRepository<T>(MySQLDBContext dbContext) : IGenericRepositoryAsync<T> where T : class
{
    public async Task<T> CreateAsync(T entity)
    {
        await dbContext.Set<T>().AddAsync(entity);
        await dbContext.SaveChangesAsync();

        return entity;
    }

    public async Task DeleteAsync(ValueType id)
    {
        var entity = await GetByIdAsync(id);

        if (entity is not null)
             dbContext.Set<T>().Remove(entity);

        return;
    }

    public async Task<T?> GetByIdAsync(ValueType id)
        => await dbContext.Set<T>().FindAsync(id);

    public async Task<List<T>> GetListAsync()
        => await dbContext.Set<T>().ToListAsync();

    public async Task<T> UpdateAsync(T entity)
    {
        dbContext.Entry(entity).State = EntityState.Modified;
        await dbContext.SaveChangesAsync();

        return entity;
    }
}
