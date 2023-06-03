using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MyBlog.Data.EntityModels;
using MyBlog.Data.Repositories;
using MyBlog.Pages.ViewModels;

namespace MyBlog.Pages
{
    public class ArticlePageModel : PageModel
    {
        private IRepository<Article> _articleRepository;
        private IRepository<ContentBlock> _blockRepository;
        private IRepository<Tag> _tagRepository;

        public ArticleViewModel Article { get; set; } = null!;
        public List<ContentBlockViewModel> Blocks { get; set; } = new List<ContentBlockViewModel>();

        public ArticlePageModel(IRepository<Article> articleRepository, 
                                IRepository<ContentBlock> blockRepository, 
                                IRepository<Tag> tagRepository)
        {
            _articleRepository = articleRepository;
            _blockRepository = blockRepository;
            _tagRepository = tagRepository;
        }

        public async Task<ActionResult> OnGet(int id)
        {
            var model = await _articleRepository.GetByIdAsync(id);

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

            Blocks = (await _blockRepository.GelAllAsync())
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

            return Page();
        }
    }
}
