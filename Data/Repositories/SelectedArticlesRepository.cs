using Microsoft.EntityFrameworkCore;
using MyBlog.Data.EntityModels;

namespace MyBlog.Data.Repositories;

public class SelectedArticlesRepository
{
    private readonly Context _context;
    public SelectedArticlesRepository(Context context)
    {
        _context = context;
    }

    public async Task AddArticleAsync(SelectedArticle model)
    {
        await _context.SelectedArticles.AddAsync(model);
        await _context.SaveChangesAsync();
    }
    public async Task Update(SelectedArticle model)
    {
        _context.SelectedArticles.Update(model);
        await _context.SaveChangesAsync();
    }
    public async Task DeleteAsync(SelectedArticle model)
    {
        _context.SelectedArticles.Remove(model);
        await _context.SaveChangesAsync();
    }
    public async Task<SelectedArticle?> GetByIdAsync(int id)
    {
        return await _context.SelectedArticles.FirstOrDefaultAsync(selectedArticle => selectedArticle.Id == id);
    }
    public async Task<IEnumerable<SelectedArticle>> GelAllAsync()
    {
        return await _context.SelectedArticles.ToListAsync();
    }
}
