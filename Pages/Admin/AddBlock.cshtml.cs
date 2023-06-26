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
        private readonly ArticlesRepository _articlesRepository;
        private readonly IWebHostEnvironment _environment;

        public SelectList ContentTypes { get; set; } = null!;

        [BindProperty]
        public InputBlockModel InputModel { get; set; } = null!;

        [BindProperty(SupportsGet = true)]
        public int ArticleId { get; set; }

        [BindProperty(SupportsGet = true)]
        public int LastSerialNumber { get; set; }

        [BindProperty]
        public IFormFile InputFile { get; set; } = null!;

        public AddBlockModel(ContentBlocksRepository contentBlocksRepository, 
            ArticlesRepository articlesRepository,
            IWebHostEnvironment environment)
        {
            _contentBlocksRepository = contentBlocksRepository;
            _articlesRepository = articlesRepository;
            _environment = environment;
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

            if (InputModel.ContentType == ContentType.Image)
            {
                if (InputFile is null)
                {
                    ModelState.TryAddModelError("File", "Файл не загружен");
                    return RedirectToPage();
                }

                string path = "/Files/" + InputFile.FileName;
                using (FileStream fileStream = new(_environment.WebRootPath + path, FileMode.Create))
                {
                    await InputFile.CopyToAsync(fileStream);
                }
                block.Content = path;
            }

            await _contentBlocksRepository.AddAsync(block);
            var article = await _articlesRepository.GetByIdAsync(ArticleId);
            article!.LastModifiedDate = DateTime.UtcNow;
            await _articlesRepository.UpdateAsync(article);

            return RedirectToPage("EditArticle", new { articleId = ArticleId });
        }
    }
}
