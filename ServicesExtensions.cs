using MyBlog.Data.EntityModels;
using MyBlog.Data.Repositories;

namespace MyBlog;

public static class ServicesExtensions
{
    public static IServiceCollection AddDbRepositories(this IServiceCollection services)
    {
        services.AddScoped<IRepository<Article>, Repository<Article>>();
        services.AddScoped<IRepository<ContentBlock>, Repository<ContentBlock>>();
        services.AddScoped<IRepository<Tag>, Repository<Tag>>();
        services.AddScoped<IRepository<User>, Repository<User>>();
        services.AddScoped<IRepository<SelectedArticle>, Repository<SelectedArticle>>();

        return services;
    }
}
