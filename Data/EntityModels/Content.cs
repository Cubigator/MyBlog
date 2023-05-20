using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MyBlog.Data.EntityModels;

[Table("content_blocks")]
public class ContentBlock
{
    [Key, Required, Column("id")]
    public int Id { get; set; }
    [ForeignKey(nameof(Article)), Required, Column("article_id")]
    public int ArticleId { get; set; }
    [Required, Column("content")]
    public string Content { get; set; } = null!;
    [Required, Column("content_type")]
    public ContentType ContentType { get; set; }

    public Article Article { get; set; } = null!;
}
