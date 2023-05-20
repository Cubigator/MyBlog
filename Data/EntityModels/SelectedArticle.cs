using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MyBlog.Data.EntityModels;

[Table("selected_articles")]
public class SelectedArticle
{
    [Key, Required, Column("id")]
    public int Id { get; set; }
    [ForeignKey(nameof(Article)), Required, Column("article_id")]
    public int ArticleId { get; set; }
    [ForeignKey(nameof(User)), Required, Column("user_id")]
    public int UserId { get; set; }
    [Required, Column("date")]
    public DateTime Date { get; set; }

    public Article Article { get; set; } = null!;
    public User User { get; set; } = null!;
}
