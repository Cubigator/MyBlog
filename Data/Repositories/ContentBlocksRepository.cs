using Microsoft.EntityFrameworkCore;
using MyBlog.Data.EntityModels;

namespace MyBlog.Data.Repositories;

public class ContentBlocksRepository
{
    private readonly Context _context;
    public ContentBlocksRepository(Context context)
    {
        _context = context;
    }

    public async Task AddAsync(ContentBlock model)
    {
        await _context.ContentBlocks.AddAsync(model);
        await _context.SaveChangesAsync();
    }
    public async Task UpdateAsync(ContentBlock model)
    {
        _context.ContentBlocks.Update(model);
        await _context.SaveChangesAsync();
    }
    public async Task DeleteAsync(ContentBlock model)
    {
        _context.ContentBlocks.Remove(model);
        await _context.SaveChangesAsync();
    }
    public async Task<ContentBlock?> GetByIdAsync(int id)
    {
        return await _context.ContentBlocks.FirstOrDefaultAsync(contentBlock => contentBlock.Id == id);
    }
    public async Task<IEnumerable<ContentBlock>> GetAllAsync()
    {
        return await _context.ContentBlocks.AsNoTracking().ToListAsync();
    }
}
