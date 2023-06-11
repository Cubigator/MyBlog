using Microsoft.EntityFrameworkCore;
using MyBlog.Data.EntityModels;

namespace MyBlog.Data.Repositories;

public class UsersRepository
{
    private readonly Context _context;
    public UsersRepository(Context context)
    {
        _context = context;
    }

    public async Task AddAsync(User model)
    {
        await _context.Users.AddAsync(model);
        await _context.SaveChangesAsync();
    }
    public async Task UpdateAsync(User model)
    {
        _context.Users.Update(model);
        await _context.SaveChangesAsync();
    }
    public async Task DeleteAsync(User model)
    {
        _context.Users.Remove(model);
        await _context.SaveChangesAsync();
    }
    public async Task<User?> GetByIdAsync(int id)
    {
        return await _context.Users.FirstOrDefaultAsync(user => user.Id == id);
    }
    public async Task<User?> GetByEmailAsync(string email)
    {
        return await _context.Users.FirstOrDefaultAsync(user => user.Email == email);
    }
    public async Task<IEnumerable<User>> GelAllAsync()
    {
        return await _context.Users.ToListAsync();
    }
}
