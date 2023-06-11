using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MyBlog.Data.EntityModels;
using MyBlog.Data.Repositories;
using MyBlog.Pages.ViewModels;
using System.Security.Claims;

namespace MyBlog.Pages
{
    public class ArticlePageModel : PageModel
    {
        private readonly UsersRepository _usersRepository;
        private readonly ArticlesRepository _articlesRepository;
        private readonly ContentBlocksRepository _blocksRepository;
        private readonly TagsRepository _tagsRepository;
        private readonly SelectedArticlesRepository _selectedArticlesRepository;

        public ArticleViewModel Article { get; set; } = null!;
        public List<ContentBlockViewModel> Blocks { get; set; } = new List<ContentBlockViewModel>();
        public bool? IsSelected { get; set; } = null;

        public ArticlePageModel(ArticlesRepository articlesRepository,
                                ContentBlocksRepository blocksRepository,
                                TagsRepository tagsRepository,
                                SelectedArticlesRepository selectedArticlesRepository,
                                UsersRepository usersRepository)
        {
            _articlesRepository = articlesRepository;
            _blocksRepository = blocksRepository;
            _tagsRepository = tagsRepository;
            _selectedArticlesRepository = selectedArticlesRepository;
            _usersRepository = usersRepository;
        }

        public async Task<ActionResult> OnGet(int id)
        {
            var model = await _articlesRepository.GetByIdAsync(id);

            if (model is null)
                return RedirectToPage("Index");

            Article = new()
            {
                Id = model.Id,
                CreationDate = model.CreationDate,
                Header = model.Header,
                Image = model.Image,
                Introduction = model.Introduction,
                LastModifiedDate = model.LastModifiedDate,
                ReadingTime = model.ReadingTime,
            };

            Blocks = (await _blocksRepository.GelAllAsync())
                .Where(block => block.ArticleId == Article.Id)
                .Select(block => new ContentBlockViewModel()
            {
                Id = block.Id,
                SerialNumber = block.SerialNumber,
                ArticleId = block.Id,
                Content = block.Content,
                ContentType = block.ContentType,
            })
                .OrderBy(block => block.SerialNumber).ToList();

            if (HttpContext.User.Identity.IsAuthenticated)
            {
                string userEmail = HttpContext.User.Claims
                    .FirstOrDefault(claim => claim.Type == ClaimTypes.Email)!.Value;

                var user = await _usersRepository.GetByEmailAsync(userEmail);
                IsSelected = await _selectedArticlesRepository
                    .IsSelectedAsync(Article.Id, user.Id);
            }

            return Page();
        }

        public async Task<ActionResult> OnPost(int id, bool isSelected)
        {
            string userEmail = HttpContext.User.Claims
                    .FirstOrDefault(claim => claim.Type == ClaimTypes.Email)!.Value;

            var user = await _usersRepository.GetByEmailAsync(userEmail);

            if (isSelected)
            {
                var selected = await _selectedArticlesRepository.GetByUserAndArticleAsync(user.Id, id);

                if (selected != null)
                    await _selectedArticlesRepository.DeleteAsync(selected);
            }
            else
            {
                SelectedArticle selected = new()
                {
                    ArticleId = id,
                    UserId = user.Id,
                    Date = DateTime.UtcNow,
                };
                await _selectedArticlesRepository.AddAsync(selected);
            }
            return Redirect($"/article/{id}");
        }
    }
}
