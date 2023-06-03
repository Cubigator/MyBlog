using MyBlog.Data.EntityModels;

namespace MyBlog.Pages.ViewModels;

public class ContentBlockViewModel
{
    public int Id { get; set; }
    public int ArticleId { get; set; }
    public string Content { get; set; } = null!;
    public ContentType ContentType { get; set; }
    public int SerialNumber { get; set; }
}
