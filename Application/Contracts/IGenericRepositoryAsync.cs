namespace Application.Contracts;
public interface IGenericRepositoryAsync<T> where T : class
{
    Task<T> CreateAsync(T entity);
    Task<T> UpdateAsync(T entity);
    Task DeleteAsync(ValueType id);
    Task<T?> GetByIdAsync(ValueType id);
    Task<List<T>> GetListAsync();
}
