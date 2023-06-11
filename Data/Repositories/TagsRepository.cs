using Microsoft.EntityFrameworkCore;
using MyBlog.Data.EntityModels;

namespace MyBlog.Data.Repositories;

public class TagsRepository
{
    private readonly Context _context;
    public TagsRepository(Context context)
    {
        _context = context;
    }

    public async Task AddAsync(Tag model)
    {
        await _context.Tags.AddAsync(model);
        await _context.SaveChangesAsync();
    }
    public async Task UpdateAsync(Tag model)
    {
        _context.Tags.Update(model);
        await _context.SaveChangesAsync();
    }
    public async Task DeleteAsync(Tag model)
    {
        _context.Tags.Remove(model);
        await _context.SaveChangesAsync();
    }
    public async Task<Tag?> GetByIdAsync(int id)
    {
        return await _context.Tags.FirstOrDefaultAsync(tag => tag.Id == id);
    }
    public async Task<IEnumerable<Tag>> GelAllAsync()
    {
        return await _context.Tags.ToListAsync();
    }
}
