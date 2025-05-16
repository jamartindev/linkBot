using OpenQA.Selenium.Support.UI;
using OpenQA.Selenium;
using Polly;
using Polly.Retry;
using System.Collections.ObjectModel;
using LinkBotLogic.Services;

namespace LinkBotLogic.Pages
{
    public abstract class BasePage(IWebDriver driver, TimeSpan timeout)
    {
        protected IWebDriver Driver { get; } = driver;
        protected WebDriverWait Wait { get; } = new WebDriverWait(driver, timeout);
        
        protected IWebElement Find(By locator)
        => RetryPolicies.SeleniumRetryPolicy.Execute(() => Wait.Until(d => d.FindElement(locator)));

        protected ReadOnlyCollection<IWebElement> FindListNested(By locator, IWebElement parent)
            => RetryPolicies.SeleniumRetryPolicy.Execute(() => Wait.Until(_ => parent.FindElements(locator)));

        protected IWebElement FindNested(IWebElement parent, By locator)
        => RetryPolicies.SeleniumRetryPolicy.Execute(() => Wait.Until(_ => parent.FindElement(locator)));

        protected void Click(By locator)
            => RetryPolicies.SeleniumRetryPolicy.Execute(() => Find(locator).Click());

        protected void SendKeys(By locator, string text)
            => RetryPolicies.SeleniumRetryPolicy.Execute(() => Find(locator).SendKeys(text));

        public void NavigateTo(string url)
                => Driver.Navigate().GoToUrl(url);
    }
}