using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MyBlog.Data.EntityModels;
using MyBlog.Data.Repositories;

namespace MyBlog.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;
        private IRepository<Article> _articleRepository;

        public List<Article> Articles { get; set; } = new List<Article>();

        public IndexModel(ILogger<IndexModel> logger, IRepository<Article> repository)
        {
            _logger = logger;
            _articleRepository = repository;
        }

        public async Task OnGet()
        {
            Articles = (await _articleRepository.GelAllAsync()).ToList();
        }
    }
}