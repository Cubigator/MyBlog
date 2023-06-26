using MyBlog.Data.EntityModels;

namespace MyBlog.Pages.ViewModels;

public class UserViewModel
{
    public int Id { get; set; }
    public string? Name { get; set; }
    public string Email { get; set; } = null!;
    public UserStatus Status { get; set; }
}
