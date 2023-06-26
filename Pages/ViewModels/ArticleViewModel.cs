namespace MyBlog.Pages.ViewModels;

public class ArticleViewModel
{
    public int Id { get; set; }
    public string Header { get; set; } = null!;
    public string Introduction { get; set; } = null!;
    public string? Image { get; set; }
    public DateTime CreationDate { get; set; }
    public DateTime LastModifiedDate { get; set; }
    public int? ReadingTime { get; set; }
}
