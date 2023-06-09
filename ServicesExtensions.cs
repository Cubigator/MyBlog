using MyBlog.Data.Repositories;

namespace MyBlog;

public static class ServicesExtensions
{
    public static IServiceCollection AddDbRepositories(this IServiceCollection services)
    {
        services.AddScoped<ArticlesRepository>();
        services.AddScoped<ContentBlocksRepository>();
        services.AddScoped<TagsRepository>();
        services.AddScoped<UsersRepository>();
        services.AddScoped<SelectedArticlesRepository>();

        return services;
    }
}
