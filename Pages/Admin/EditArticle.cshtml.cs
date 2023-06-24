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
            await GetContentBlocks(articleId);

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
}
