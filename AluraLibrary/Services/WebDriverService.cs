using AluraLibrary.Controllers;
using AluraLibrary.Interfaces;
using Microsoft.Extensions.Logging;

namespace AluraLibrary.Services;

public class WebDriverService : IWebDriverService
{
    public WebDriverController WebDriverInstance { get; private set; }

    public CancellationToken CurrentToken { get; private set; }

    public WebDriverService(ILogger<WebDriverService> logger)
    {
        WebDriverInstance = new(logger);
        CurrentToken = new();
    }

    public Task RunWebDriverAsync(string searchText, CancellationToken token)
    {
        CurrentToken = token;
        return WebDriverInstance.GetInfoAsync(searchText, token);
    }
}
