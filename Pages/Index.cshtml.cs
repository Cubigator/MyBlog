using Microsoft.AspNetCore.Mvc.RazorPages;
using MyBlog.Data.EntityModels;
using MyBlog.Data.Repositories;
using MyBlog.Pages.ViewModels;

namespace MyBlog.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;
        private ArticlesRepository _articlesRepository;

        public List<ArticleViewModel> Articles { get; set; } = new List<ArticleViewModel>();
        public List<ContentBlock> Blocks { get; set; }

        public IndexModel(ILogger<IndexModel> logger, ArticlesRepository repository)
        {
            _logger = logger;
            _articlesRepository = repository;
        }

        public async Task OnGet()
        {
            Articles = (await _articlesRepository.GelAllAsync()).Select(item =>
                new ArticleViewModel()
                {
                    Id = item.Id,
                    Header = item.Header,
                    Introduction = item.Introduction,
                    Image = item.Image,
                    CreationDate = item.CreationDate,
                    LastModifiedDate = item.LastModifiedDate,
                    ReadingTime = item.ReadingTime
                }).ToList();
        }
    }
}
