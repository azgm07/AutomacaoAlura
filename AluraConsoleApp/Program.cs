using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using AluraLibrary;
using AluraLibrary.Services;
using AluraLibrary.Interfaces;
using Microsoft.Extensions.Configuration;



namespace Alura.Main;

public static class Program
{
    public async static Task Main(string[] args)
    {
        var configuration = new ConfigurationBuilder()
                                .SetBasePath(Directory.GetCurrentDirectory())
                                .AddJsonFile($"appsettings.json", false, true)
                                .AddEnvironmentVariables()
                                .Build();

        var builder = Host.CreateDefaultBuilder(args);
        builder.ConfigureServices(services =>
        {
            ServiceConfiguration.ConfigureServices(services);
        });

        builder.ConfigureLogging(logging =>
        {
            logging.AddConfiguration(configuration.GetSection("Logging"));
            ServiceConfiguration.ConfigureLogging(logging);
        });


        var app = builder.Build();

        Task appTask = app.RunAsync();
        
        await appTask;
    }
}

