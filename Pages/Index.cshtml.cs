using Microsoft.AspNetCore.Mvc.RazorPages;
using MyBlog.Data.EntityModels;
using MyBlog.Data.Repositories;
using MyBlog.Pages.ViewModels;

namespace MyBlog.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;
        private IRepository<Article> _articleRepository;

        public List<ArticleViewModel> Articles { get; set; } = new List<ArticleViewModel>();
        public List<ContentBlock> Blocks { get; set; }

        public IndexModel(ILogger<IndexModel> logger, IRepository<Article> repository)
        {
            _logger = logger;
            _articleRepository = repository;
        }

        public async Task OnGet()
        {
            Articles = (await _articleRepository.GelAllAsync()).Select(item =>
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
