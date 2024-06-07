using Microsoft.Extensions.Logging;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using OpenQA.Selenium;
using SeleniumExtras.WaitHelpers;
using AluraLibrary.Models;
using AluraLibrary.Utils;
using AluraLibrary.Interfaces;
using Newtonsoft.Json;

namespace AluraLibrary.Controllers;

public sealed class WebDriverController : IDisposable
{
    private readonly IDataService _dataService;
    private readonly ILogger _logger;
    private readonly string _url;

    public WebDriverController(ILogger logger, IDataService dataService)
    {
        _url = "https://www.alura.com.br/busca";
        _logger = logger;
        _dataService = dataService;
    }

    public void Dispose()
    {

    }

    public WebElement? WaitUntilElementExists(WebDriver driver, By elementLocator, int timeout = 10)
    {
        try
        {
            var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(timeout));
            return (WebElement)wait.Until(ExpectedConditions.ElementExists(elementLocator));
        }
        catch (Exception e) when (e is NoSuchElementException || e is WebDriverTimeoutException)
        {
            _logger.LogWarning("Elemento ({locator}) não encontrado dentro do contexto da página.", elementLocator);
            return null;
        }
    }

    public WebElement? WaitUntilElementVisible(WebDriver driver, By elementLocator, int timeout = 10)
    {
        try
        {
            var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(timeout));
            return (WebElement)wait.Until(ExpectedConditions.ElementIsVisible(elementLocator));
        }
        catch (Exception e) when (e is NoSuchElementException || e is WebDriverTimeoutException)
        {
            _logger.LogWarning("Elemento ({locator}) não está visível.", elementLocator);
            return null;
        }
    }

    public WebElement? WaitUntilElementClickable(WebDriver driver, By elementLocator, int timeout = 10)
    {
        try
        {
            var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(timeout));
            return (WebElement)wait.Until(ExpectedConditions.ElementToBeClickable(elementLocator));
        }
        catch (Exception e) when (e is NoSuchElementException || e is WebDriverTimeoutException)
        {
            _logger.LogWarning("Elemento ({locator}) não é clicável.", elementLocator);
            return null;
        }
    }

    public WebDriver? CreateDriver(bool isHeadless = false)
    {
        try
        {
            //Returns a new BrowserPage
            ChromeOptions options = new()
            {
                //BinaryLocation = "C:\\Program Files (x86)\\Google\\Chrome\\Application\\chrome.exe"
            };

            var driverService = ChromeDriverService.CreateDefaultService();

            //Change options depending on the case
            if (isHeadless)
            {
                options.AddArguments(new List<string>() { "headless", "disable-gpu", "no-sandbox", "disable-extensions",
                "disable-application-cache", "disable-notifications", "disable-infobars", "log-level=3", "mute-audio" });

                driverService.HideCommandPromptWindow = true;
            }
            else
            {
                options.AddArguments(new List<string>() { /*"headless",*/ "disable-gpu", "no-sandbox", "disable-extensions",
                "disable-application-cache", "disable-notifications", "disable-infobars", "log-level=3", "mute-audio" });
            }

            return new ChromeDriver(driverService, options);
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Não foi possível criar o Webdriver");
            return null;
        }
    }

    public async Task<CourseInformation?> GetInfoAsync(string searchText, CancellationToken token)
    {
        CourseInformation? response = null;
        try
        {
            using WebDriver? driver = CreateDriver();
            if (driver != null)
            {
                if (OpenPage(driver) && PreparePage(driver, searchText))
                {
                    Task<CourseInformation?> taskGetCourses = Task.Run(() => ReadFirstCourse(driver), token);

                    response = await taskGetCourses;
                        

                    if (response != null)
                    {
                        string json = JsonConvert.SerializeObject(response, Formatting.Indented);
                        _logger.LogDebug("Dados coletados: \n{json}", json);
                        FlushData(searchText, response, true);
                    }
                }
            }
            
        }
        catch (Exception)
        {
            response = null;
        }
        return response;
    }

    private bool OpenPage(WebDriver driver)
    {
        driver.Navigate().GoToUrl(_url);
        By selector = By.CssSelector("#busca-form-input");
        if (selector != null && WaitUntilElementExists(driver, selector) == null)
        {
            return false;
        }
        else
        {
            return true;
        }
    }

    private bool PreparePage(WebDriver driver, string searchText)
    {
        bool result = false;
        try
        {
            if (driver != null)
            {
                By selectorSearch = By.CssSelector("#busca-form-input");
                By selectorSubmit = By.CssSelector("#busca-form > form > input.busca-form-botao.--desktop");
                WebElement? webElementSubmit = WaitUntilElementClickable(driver, selectorSubmit);

                if (webElementSubmit != null &&
                    WaitUntilElementVisible(driver, selectorSearch) != null)
                {
                    driver.FindElement(selectorSearch).SendKeys(searchText);
                    webElementSubmit.Click();
                    result = true;
                }
                else
                {
                    result = false;
                }
            }
        }
        catch (Exception ex)
        {
            _logger.LogError("Preparação da página falhou: {ex}", ex.Message);
            result = false;
        }
        return result;
    }

    private CourseInformation? ReadFirstCourse(WebDriver driver)
    {
        try
        {
            By selectorTitle = By.CssSelector("#busca-resultados > ul > li:nth-child(1) > a > div > h4");
            By selectorDescription = By.CssSelector("#busca-resultados > ul > li:nth-child(1) > a > div > p");

            string courseTitle = driver.FindElement(selectorTitle).Text;
            string courseDescription = driver.FindElement(selectorDescription).Text;
            string courseHours = "";
            string courseInstructors = "";

            By selectorCourse = By.CssSelector("#busca-resultados > ul > li:nth-child(1)");
            driver.FindElement(selectorCourse).Click();

            By selectorHours = By.CssSelector("body > main > section.formacao-container-color > div > div.formacao__info-conclusao > div.formacao__info-content > div");
            if (WaitUntilElementVisible(driver, selectorHours) != null)
            {
                courseHours = driver.FindElement(selectorHours).Text;
                
                List<string> courseInstructorsList = new();
                int countInstructors = 2;
                By selectorInstructor = By.CssSelector($"#instrutores > div > ul > li:nth-child({countInstructors}) > div > h3");
                var elementInstuctor = WaitUntilElementVisible(driver, selectorInstructor);
                while (elementInstuctor != null)
                {
                    courseInstructorsList.Add(elementInstuctor.Text);
                    countInstructors += 2;
                    selectorInstructor = By.CssSelector($"#instrutores > div > ul > li:nth-child({countInstructors}) > div > h3");
                    elementInstuctor = WaitUntilElementVisible(driver, selectorInstructor, 2);
                }

                courseInstructors = string.Join(" . ", courseInstructorsList);
            }

            CourseInformation data = new(courseTitle, courseInstructors, courseHours, courseDescription);

            return data;
        }
        catch (Exception ex)
        {
            _logger.LogError("Dados não foram capturados: {ex}", ex.Message);
            return null;
        }
    }

    private void FlushData(string search, object data, bool startNew = false)
    {
        string file = $"{DataUtils.RemoveSpecial(search).ToLower()}.json";
        _dataService.WriteData("files/json", file, data, startNew);
    }
}
