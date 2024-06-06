using AluraLibrary.Controllers;

namespace AluraLibrary.Interfaces;

public interface IWebDriverService
{
    WebDriverController WebDriverInstance { get; }
    int RunTimeSeconds { get; set; }
    CancellationToken CurrentToken { get; }

    Task RunWebDriverAsync(string searchText, CancellationToken token);
}