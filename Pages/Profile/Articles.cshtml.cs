using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MyBlog.Data.Repositories;
using MyBlog.Pages.ViewModels;
using System.Security.Claims;

namespace MyBlog.Pages.Profile
{
    [Authorize]
    public class ArticlesModel : PageModel
    {
        private readonly SelectedArticlesRepository _selectedArticlesRepository;
        private readonly UsersRepository _usersRepository;
        private readonly ArticlesRepository _articlesRepository;
        public List<ArticleViewModel> Articles { get; set; } = null!;
        public ArticlesModel(SelectedArticlesRepository selectedArticlesRepository,
                             UsersRepository usersRepository,
                             ArticlesRepository articlesRepository)
        {
            _selectedArticlesRepository = selectedArticlesRepository;
            _usersRepository = usersRepository;
            _articlesRepository = articlesRepository;
        }
        public async Task OnGet()
        {
            string email = HttpContext.User.Claims
                .FirstOrDefault(claim => claim.Type == ClaimTypes.Email)!.Value;
            var user = await _usersRepository.GetByEmailAsync(email);

            var selectedArticles = await _selectedArticlesRepository.GetByUserIdAsync(user.Id);

            Articles = (from article in await _articlesRepository.GelAllAsync()
                        join selectedArticle in selectedArticles on article.Id equals selectedArticle.ArticleId
                        select article)
                       .Select(selectedArticle => new ArticleViewModel()
                       {
                           Id = selectedArticle.Id,
                           CreationDate = selectedArticle.CreationDate,
                           Header = selectedArticle.Header,
                           LastModifiedDate = selectedArticle.LastModifiedDate,
                           Image = selectedArticle.Image,
                           Introduction = selectedArticle.Introduction,
                           ReadingTime = selectedArticle.ReadingTime
                       }).ToList();
        }
    }
}
