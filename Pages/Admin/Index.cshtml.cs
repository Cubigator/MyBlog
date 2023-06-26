using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MyBlog.Data.EntityModels;
using MyBlog.Data.Repositories;
using MyBlog.Pages.ViewModels;

namespace MyBlog.Pages.Admin
{
    [Authorize(Roles = nameof(UserStatus.Admin))]
    public class IndexModel : PageModel
    {
        private readonly ArticlesRepository _articlesRepository;

        public List<ArticleViewModel> Articles { get; set; } = null!;
        public IndexModel(ArticlesRepository articlesRepository)
        {
            _articlesRepository = articlesRepository;
        }
        public async Task OnGet()
        {
            Articles = (await _articlesRepository.GetAllAsync())
                .OrderByDescending(article => article.CreationDate)
                .Select(article => new ArticleViewModel()
                {
                    Id = article.Id,
                    CreationDate = article.CreationDate,
                    LastModifiedDate = article.LastModifiedDate,
                    Header = article.Header,
                    Image = article.Image,
                    Introduction = (article.Introduction.Length > 100) ? 
                        article.Introduction.Substring(0, 100) : article.Introduction,
                    ReadingTime = article.ReadingTime
                }).ToList();
        }

        public async Task<ActionResult> OnPostDeleteArticle(int articleId)
        {
            await _articlesRepository.DeleteAsync(new Article() { Id = articleId });
            return RedirectToPage();
        }
    }
}
