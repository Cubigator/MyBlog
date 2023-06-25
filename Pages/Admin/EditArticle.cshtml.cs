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
        private readonly IWebHostEnvironment _environment;
        public ArticleViewModel Article { get; set; } = null!;
        public List<ContentBlockViewModel> ContentBlocks { get; set; } = null!;

        [BindProperty]
        public ArticleInputModel InputModel { get; set; } = null!;
        [BindProperty]
        public IFormFile InputFile { get; set; } = null!;

        public EditArticleModel(ArticlesRepository articlesRepository, 
            ContentBlocksRepository contentBlocksRepository,
            IWebHostEnvironment environment)
        {
            _articlesRepository = articlesRepository;
            _contentBlocksRepository = contentBlocksRepository;
            _environment = environment;
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
            await GetContentBlocks(articleId);

            InputModel = new ArticleInputModel();
            InputModel.Header = article.Header;
            InputModel.ReadingTime = article.ReadingTime;
            InputModel.Introduction = article.Introduction;

            return Page();
        }

        public async Task<ActionResult> OnPostUp(int blockId, int articleId)
        {
            await GetContentBlocks(articleId);
            var block = ContentBlocks.First(block => block.Id == blockId);
            int index = ContentBlocks.IndexOf(block);
            if(index != 0)
            {
                ContentBlock upBlock = new()
                {
                    Id = block.Id,
                    Content = block.Content,
                    ContentType = block.ContentType,
                    SerialNumber = ContentBlocks[index - 1].SerialNumber,
                    ArticleId = block.ArticleId,
                };
                await _contentBlocksRepository.UpdateAsync(upBlock);
                ContentBlock downBlock = new()
                {
                    Id = ContentBlocks[index - 1].Id,
                    Content = ContentBlocks[index - 1].Content,
                    ContentType = ContentBlocks[index - 1].ContentType,
                    SerialNumber = block.SerialNumber,
                    ArticleId = ContentBlocks[index - 1].ArticleId,
                };

                await _contentBlocksRepository.UpdateAsync(downBlock);
            }
            return RedirectToPage();
        }

        public async Task<ActionResult> OnPostDown(int blockId, int articleId)
        {
            await GetContentBlocks(articleId);
            var block = ContentBlocks.First(block => block.Id == blockId);
            int index = ContentBlocks.IndexOf(block);
            if (index < ContentBlocks.Count - 1)
            {
                ContentBlock downBlock = new()
                {
                    Id = block.Id,
                    Content = block.Content,
                    ContentType = block.ContentType,
                    SerialNumber = ContentBlocks[index + 1].SerialNumber,
                    ArticleId = block.ArticleId,
                };
                await _contentBlocksRepository.UpdateAsync(downBlock);
                ContentBlock upBlock = new()
                {
                    Id = ContentBlocks[index + 1].Id,
                    Content = ContentBlocks[index + 1].Content,
                    ContentType = ContentBlocks[index + 1].ContentType,
                    SerialNumber = block.SerialNumber,
                    ArticleId = ContentBlocks[index + 1].ArticleId,
                };

                await _contentBlocksRepository.UpdateAsync(upBlock);
            }
            return RedirectToPage();
        }

        public async Task<ActionResult> OnPostDelete(int blockId)
        {
            await _contentBlocksRepository.DeleteAsync(new ContentBlock() { Id = blockId });
            return RedirectToPage();
        }

        public async Task<ActionResult> OnPostUpdateArticle(int articleId)
        {
            var article = await _articlesRepository.GetByIdAsync(articleId);
            article!.LastModifiedDate = DateTime.UtcNow;
            article.Header = InputModel.Header;
            article.Introduction = InputModel.Introduction;
            article.ReadingTime = InputModel.ReadingTime;

            if(InputFile != null)
            {
                string path = "/Files/" + InputFile.FileName;
                using(FileStream fileStream = new(_environment.WebRootPath + path, FileMode.Create))
                {
                    await InputFile.CopyToAsync(fileStream);
                }
                article.Image = path;
            }
            await _articlesRepository.UpdateAsync(article);

            return RedirectToPage();
        }

        private async Task GetContentBlocks(int articleId)
        {
            ContentBlocks = (await _contentBlocksRepository.GetAllAsync())
                .Where(block => block.ArticleId == articleId)
                .Select(block => new ContentBlockViewModel()
                {
                    Id = block.Id,
                    ArticleId = block.ArticleId,
                    Content = block.Content,
                    ContentType = block.ContentType,
                    SerialNumber = block.SerialNumber,
                }).OrderBy(block => block.SerialNumber).ToList();
        }
    }

    public class ArticleInputModel
    {
        public string Header { get; set; } = null!;
        public string Introduction { get; set; } = null!;
        public int? ReadingTime { get; set; }
    }
}
