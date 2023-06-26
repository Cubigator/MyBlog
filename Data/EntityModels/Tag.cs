using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MyBlog.Data.EntityModels;

[Table("tags")]
public class Tag
{
    [Key, Required, Column("id")]
    public int Id { get; set; }
    [Required, Column("name")]
    public string Name { get; set; } = null!;
    [ForeignKey(nameof(Article)), Required, Column("article_id")]
    public int ArticleId { get; set; }

    public Article Article { get; set; } = null!;
}
