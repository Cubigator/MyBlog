using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using MyBlog.Data.EntityModels;
using MyBlog.Data.Repositories;
using MyBlog.Pages.ViewModels;

namespace MyBlog.Pages.Admin
{
    public class EditBlockModel : PageModel
    {
        private readonly ContentBlocksRepository _contentBlocksRepository;
        public ContentBlockViewModel ContentBlock { get; set; } = null!;
        public SelectList ContentTypes { get; set; } = null!;

        [BindProperty]
        public InputBlockModel InputModel { get; set; } = null!;

        public EditBlockModel(ContentBlocksRepository contentBlocksRepository)
        {
            _contentBlocksRepository = contentBlocksRepository;
        }
        public async Task OnGet(int blockId)
        {
            var block = await _contentBlocksRepository.GetByIdAsync(blockId);

            ContentBlock = new ContentBlockViewModel()
            {
                Id = block!.Id,
                ArticleId = block.ArticleId,
                Content = block.Content,
                ContentType = block.ContentType,
                SerialNumber = block.SerialNumber,
            };
            var types = Enum.GetValues(typeof(ContentType));
            ContentTypes = new SelectList(types);

            InputModel = new InputBlockModel();
            InputModel.Content = ContentBlock.Content;
            InputModel.ContentType = ContentBlock.ContentType;
        }

        public async Task<ActionResult> OnPost(int blockId)
        {
            var block = await _contentBlocksRepository.GetByIdAsync(blockId);
            block!.ContentType = InputModel.ContentType;
            block.Content = InputModel.Content;
            await _contentBlocksRepository.UpdateAsync(block);
            return RedirectToPage("EditArticle", new { articleId = block.ArticleId });
        }
    }

    public class InputBlockModel
    {
        public string Content { get; set; } = null!;
        public ContentType ContentType { get; set; }
    }
}
