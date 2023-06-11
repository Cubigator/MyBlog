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

    public async Task AddAsync(SelectedArticle model)
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

    public async Task<IEnumerable<SelectedArticle>> GetByUserIdAsync(int userId)
    {
        return await _context.SelectedArticles
            .Where(article => article.UserId == userId)
            .ToListAsync();
    }

    public async Task<SelectedArticle?> GetByUserAndArticleAsync(int userId, int articleId)
    {
        return await _context.SelectedArticles
            .Where(article => article.UserId == userId && article.ArticleId == articleId)
            .FirstOrDefaultAsync();
    }

    public async Task<bool> IsSelectedAsync(int articleId, int userId)
    {
        var selectedArticle = await _context.SelectedArticles
            .FirstOrDefaultAsync(selectedArticle => selectedArticle.UserId == userId
                                                    && selectedArticle.ArticleId == articleId);
        return selectedArticle != null;
    }
}
