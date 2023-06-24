using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using MyBlog.Data.EntityModels;
using MyBlog.Data.Repositories;

namespace MyBlog.Pages.Admin
{
    public class AddBlockModel : PageModel
    {
        private readonly ContentBlocksRepository _contentBlocksRepository;

        public SelectList ContentTypes { get; set; } = null!;

        [BindProperty]
        public InputBlockModel InputModel { get; set; } = null!;

        [BindProperty(SupportsGet = true)]
        public int ArticleId { get; set; }

        [BindProperty(SupportsGet = true)]
        public int LastSerialNumber { get; set; }

        public AddBlockModel(ContentBlocksRepository contentBlocksRepository)
        {
            _contentBlocksRepository = contentBlocksRepository;
        }

        public void OnGet()
        {
            var types = Enum.GetValues(typeof(ContentType));
            ContentTypes = new SelectList(types);
        }

        public async Task<ActionResult> OnPost()
        {
            ContentBlock block = new()
            {
                ArticleId = ArticleId,
                Content = InputModel.Content,
                SerialNumber = LastSerialNumber + 1,
                ContentType = InputModel.ContentType
            };
            await _contentBlocksRepository.AddAsync(block);

            return RedirectToPage("EditArticle", new { articleId = ArticleId });
        }
    }
}
