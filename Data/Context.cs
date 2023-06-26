using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using MyBlog.Data.EntityModels;

namespace MyBlog.Data;

public class Context : DbContext
{
    IConfiguration _configuration;
    public Context(DbContextOptions<Context> options, IConfiguration configuration) : base(options)
    {
        _configuration = configuration;
    }
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        
        optionsBuilder.UseSqlServer(_configuration.GetConnectionString("SqlServer"));
    }
    public DbSet<User> Users { get; set; }
    public DbSet<Article> Articles { get; set; }
    public DbSet<ContentBlock> ContentBlocks { get; set; }
    public DbSet<Tag> Tags { get; set; }
    public DbSet<SelectedArticle> SelectedArticles { get; set; }
}
