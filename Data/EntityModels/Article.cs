using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MyBlog.Data.EntityModels;

[Table("articles")]
public class Article
{
    [Key, Required, Column("id")]
    public int Id { get; set; }
    [Required, Column("header")]
    public string Header { get; set; } = null!;
    [Required, Column("introduction")]
    public string Introduction { get; set; } = null!;
    [Column("image")]
    public string? Image { get; set; }
    [Required, Column("creation_date")]
    public DateTime CreationDate { get; set; }
    [Required, Column("last_mofified_date")]
    public DateTime LastModifiedDate { get; set; }
    [Column("reading_time")]
    public int? ReadingTime { get; set; }

    public IEnumerable<Tag> Tags { get; set; } = null!;
    public IEnumerable<ContentBlock> Blocks { get; set; } = null!;
}
