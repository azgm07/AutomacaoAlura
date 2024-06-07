using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using AluraLibrary.Interfaces;

namespace AluraLibrary.Services;

public class HostService : BackgroundService
{
    private readonly ILogger<HostService> _logger;
    private readonly IHostApplicationLifetime _hostApplicationLifetime;
    private readonly IWebDriverService _webDriver;

    private const string SearchMenu = "Por favor escreva o termo que deseja buscar:";
    private const string SearchError = "Texto capturado é nulo";

    public HostService(IHostApplicationLifetime hostApplicationLifetime, ILogger<HostService> logger, IWebDriverService webDriver)
    {
        _hostApplicationLifetime = hostApplicationLifetime;
        _logger = logger;
        _webDriver = webDriver;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        await Task.Delay(1000, stoppingToken);
        await Task.Run(
            async () => {
                while (!stoppingToken.IsCancellationRequested)
                {
                    Console.WriteLine(SearchMenu);
                    string? searchText = Console.ReadLine();
                    if (searchText != null)
                    {
                        await _webDriver.RunWebDriverAsync(searchText, stoppingToken);
                    }
                    else
                    {
                        _logger.LogWarning(SearchError);
                    }
                }
                _hostApplicationLifetime.StopApplication();
            }, CancellationToken.None
        );
    }
}
