using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MyBlog.Data.EntityModels;
using MyBlog.Data.Repositories;
using MyBlog.Pages.ViewModels;

namespace MyBlog.Pages
{
    public class ArticlePageModel : PageModel
    {
        private ArticlesRepository _articlesRepository;
        private ContentBlocksRepository _blocksRepository;
        private TagsRepository _tagsRepository;

        public ArticleViewModel Article { get; set; } = null!;
        public List<ContentBlockViewModel> Blocks { get; set; } = new List<ContentBlockViewModel>();

        public ArticlePageModel(ArticlesRepository articlesRepository,
                                ContentBlocksRepository blocksRepository,
                                TagsRepository tagsRepository)
        {
            _articlesRepository = articlesRepository;
            _blocksRepository = blocksRepository;
            _tagsRepository = tagsRepository;
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

            return Page();
        }
    }
}
