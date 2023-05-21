using Microsoft.EntityFrameworkCore;
using MyBlog.Data.EntityModels;

namespace MyBlog.Data.Repositories;

public class ArticlesRepository : IRepository<Article>
{
    Context _context;
    public ArticlesRepository(Context context)
    {
        _context = context;
    }
    public async Task AddAsync(Article model)
    {
        await _context.Articles.AddAsync(model);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(Article model)
    {
        _context.Articles.Remove(model);
        await _context.SaveChangesAsync();
    }

    public async Task<IEnumerable<Article>> GelAllAsync()
    {
        return await _context.Articles.ToListAsync();
    }

    public async Task<Article?> GetByIdAsync(int id)
    {
        return await _context.Articles.FirstOrDefaultAsync(x => x.Id == id);
    }

    public async Task UpdateAsync(Article model)
    {
        _context.Articles.Update(model);
        await _context.SaveChangesAsync();
    }
}   
