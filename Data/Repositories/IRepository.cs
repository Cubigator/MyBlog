namespace MyBlog.Data.Repositories;

public interface IRepository<T>
{
    public Task AddAsync(T model);
    public Task UpdateAsync(T model);
    public Task DeleteAsync(T model);
    public Task<T> GetByIdAsync(int id);
    public Task<IEnumerable<T>> GelAllAsync();
}
