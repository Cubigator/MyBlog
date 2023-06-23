using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MyBlog.Data.EntityModels;
using MyBlog.Data.Repositories;
using MyBlog.Pages.ViewModels;

namespace MyBlog.Pages.Admin
{
    [Authorize(Roles = nameof(UserStatus.Admin))]
    public class EditArticleModel : PageModel
    {
        private readonly ArticlesRepository _articlesRepository;
        private readonly ContentBlocksRepository _contentBlocksRepository;
        public ArticleViewModel Article { get; set; } = null!;
        public List<ContentBlockViewModel> ContentBlocks { get; set; } = null!;

        public EditArticleModel(ArticlesRepository articlesRepository, 
            ContentBlocksRepository contentBlocksRepository)
        {
            _articlesRepository = articlesRepository;
            _contentBlocksRepository = contentBlocksRepository;
        }
        public async Task<ActionResult> OnGet(int articleId)
        {
            var article = await _articlesRepository.GetByIdAsync(articleId);
            if (article is null)
                return Redirect("/Admin");
            Article = new ArticleViewModel()
            {
                Id = article.Id,
                CreationDate = article.CreationDate,
                Header = article.Header,
                LastModifiedDate = article.LastModifiedDate,
                Image = article.Image,
                Introduction = article.Introduction,
                ReadingTime = article.ReadingTime
            };
                
            ContentBlocks = (await _contentBlocksRepository.GetAllAsync())
                .Where(block => block.ArticleId == articleId)
                .Select(block => new ContentBlockViewModel()
                {
                    Id = block.Id,
                    ArticleId = block.ArticleId,
                    Content = block.Content,
                    ContentType = block.ContentType,
                    SerialNumber = block.SerialNumber,
                }).ToList();
            return Page();
        }

        public ActionResult OnPostUp()
        {
            return RedirectToPage();
        }

        public ActionResult OnPostDown()
        {
            return RedirectToPage();
        }

        public ActionResult OnPostDelete()
        {
            return RedirectToPage();
        }
    }
}
