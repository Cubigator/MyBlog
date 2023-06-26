using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MyBlog.Data.EntityModels;

[Table("users")]
public class User
{
    [Key, Required, Column("id")]
    public int Id { get; set; }
    [MaxLength(100), Column("name")]
    public string? Name { get; set; }
    [Required, Column("email")]
    public string Email { get; set; } = null!;
    [Required, Column("password")]
    public string Password { get; set; } = null!;
    [Column("salt")]
    public Guid Salt { get; set; }
    [Column("status")]
    public UserStatus Status { get; set; }

    public IEnumerable<SelectedArticle> SelectedArticles { get; set; } = null!;
}
