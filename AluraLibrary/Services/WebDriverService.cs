using AluraLibrary.Controllers;
using AluraLibrary.Interfaces;

namespace AluraLibrary.Services;

public class WebDriverService : IWebDriverService
{
    public WebDriverController WebDriverInstance => throw new NotImplementedException();

    public int RunTimeSeconds { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

    public CancellationToken CurrentToken => throw new NotImplementedException();

    public Task RunWebDriverAsync(string searchText, CancellationToken token)
    {
        Console.WriteLine("Search Text: " + searchText);
        return Task.Delay(1000, token);
    }
}
