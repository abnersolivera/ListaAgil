using OpenQA.Selenium;
using System.Collections.ObjectModel;

namespace ListaAgil.Domain.Interfaces;

public interface ISeleniumService
{
    void StartBrowser();
    void NavigateToWhatsApp();
    void WaitForLoad();
    IWebElement FindElement(By by);
    ReadOnlyCollection<IWebElement> FindElements(By by);
    void CloseBrowser();
}