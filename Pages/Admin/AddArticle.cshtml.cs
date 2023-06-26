using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MyBlog.Data.Repositories;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using MyBlog.Data.EntityModels;

namespace MyBlog.Pages.Admin;

public class AddArticleModel : PageModel
{
    private readonly ArticlesRepository _articlesRepository;

    public AddArticleModel(ArticlesRepository articlesRepository)
    {
        _articlesRepository = articlesRepository;
    }

    [BindProperty]
    public ArticleModel InputModel { get; set; } = null!;

    public void OnGet()
    {

    }

    public async Task<ActionResult> OnPost()
    {
        Article article = new Article()
        {
            Header = InputModel.Header,
            Introduction = InputModel.Introduction,
            CreationDate = DateTime.UtcNow,
            LastModifiedDate = DateTime.UtcNow
        };
        await _articlesRepository.AddAsync(article);
        return RedirectToPage("Index");
    }

    public class ArticleModel
    {
        public string Header { get; set; } = null!;
        public string Introduction { get; set; } = null!;
    }
}
