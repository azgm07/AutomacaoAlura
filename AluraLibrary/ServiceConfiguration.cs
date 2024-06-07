using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using AluraLibrary.Services;
using AluraLibrary.Interfaces;

namespace AluraLibrary;

public static class ServiceConfiguration
{
    public static void ConfigureServices(IServiceCollection services)
    {
        services.AddHostedService<HostService>();
        services.AddSingleton<IWebDriverService, WebDriverService>();
        services.AddSingleton<IDataService, DataService>();

        services.Configure<HostOptions>(options =>
        {
            options.ShutdownTimeout = TimeSpan.FromSeconds(10);
        });
    }

    public static void ConfigureLogging(ILoggingBuilder logging)
    {
        logging.ClearProviders();
        logging.AddDebug();
        logging.AddConsole();
    }
}
