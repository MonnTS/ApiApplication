using RestWebApp.Contracts;
using RestWebApp.LoggerService;
using RestWebApp.Repository;

namespace RestWebApp.API.Extensions;

public static class ServicesExtension
{
    public static void ConfigureCore(this IServiceCollection services)
    {
        services.AddCors(x =>
        {
            x.AddPolicy("CorePolicy", builder =>
            {
                builder.AllowAnyOrigin()
                    .AllowAnyMethod()
                    .AllowAnyHeader();
            });
        });
    }
    
    public static void ConfigureRepository(this IServiceCollection services)
    {
        services.AddScoped<IRepositoryWrapper, RepositoryWrapper>();
    }

    public static void ConfigureLogging(this IServiceCollection services)
    {
        services.AddSingleton<ILoggerManager, LoggerManager>();
    }
}