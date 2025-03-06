namespace ListaAgil.Infrastructure.ExternalServices;

public class SeleniumService : ISeleniumService
{
    private IWebDriver _driver;
    
    public void StartBrowser()
    {
        ChromeDriverService service = ChromeDriverService.CreateDefaultService();
        service.HideCommandPromptWindow = true;

        ChromeOptions options = new ChromeOptions();
        options.AddArgument("user-data-dir=C:\\Users\\abner\\AppData\\Local\\Google\\Chrome\\User Data");
        options.AddExcludedArgument("enable-automation");
        options.AddAdditionalOption("useAutomationExtension", false);
        options.AddArgument("--start-maximized");

        _driver = new ChromeDriver(service, options);
    }

    public void NavigateToWhatsApp()
    {
        _driver.Navigate().GoToUrl("https://web.whatsapp.com/");
    }

    public void WaitForLoad()
    {
        WebDriverWait wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(60));
        wait.Until(driver => ((IJavaScriptExecutor) driver).ExecuteScript("return document.readyState").ToString() == "complete");
    }

    public IWebElement FindElement(By by)
    {
        return _driver.FindElement(by);
    }

    public ReadOnlyCollection<IWebElement> FindElements(By by)
    {
        return _driver.FindElements(by);
    }

    public void CloseBrowser()
    {
        _driver.Quit();
    }
}