using Microsoft.EntityFrameworkCore;

namespace MyBlog.Data.Repositories
{
    public class Repository<T> : IRepository<T>
        where T : class
    {
        Context _context;
        public Repository(Context context)
        {
            _context = context;
        }
        public async Task AddAsync(T model)
        {
            if (model is null)
                return;

            await _context.AddAsync(model);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(T model)
        {
            if (model is null)
                return;

            _context.Remove(model);
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<T>> GelAllAsync()
        {
            return await _context.Set<T>().ToListAsync();
        }

        public async Task<T?> GetByIdAsync(int id)
        {
            return await _context.FindAsync<T>();
        }

        public async Task UpdateAsync(T model)
        {
            _context.Update(model);
            await _context.SaveChangesAsync();
        }
    }
}
