using System.Reflection;
using RestWebApp.Contracts;
using RestWebApp.Repository;
using Serilog;
using ILogger = Serilog.ILogger;

namespace RestWebApp.API.Extensions;

public static class ServicesExtension
{
    private static ILogger Logger { get; set; }

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
        var configuration = new ConfigurationBuilder()
            .SetBasePath(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location))
            .AddJsonFile("appsettings.json", false, reloadOnChange: true)
            .AddJsonFile($"appsettings.{Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Production"}.json",
                true, reloadOnChange: true)
            .Build();

        Logger = new LoggerConfiguration()
            .ReadFrom.Configuration(configuration)
            .CreateLogger();

        services.AddLogging(x =>
        {
            x.AddSerilog(Logger);
        });
    }
}