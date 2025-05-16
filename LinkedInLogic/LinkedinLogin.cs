using Docker.DotNet.Models;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;

namespace LinkedInLogic
{
    public class LinkedinLogin
    {
        IWebDriver driver = new ChromeDriver("D:/Downloads/chromedriver-win64/");
        driver.Manage().Window.Maximize();

        driver.Navigate().GoToUrl("https://www.linkedin.com/login");

        System.Threading.Thread.Sleep(2000);

         string? username = Environment.GetEnvironmentVariable("LINKEDIN_USER");
        string? password = Environment.GetEnvironmentVariable("LINKEDIN_PASS");

                if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
                {
                    throw new Exception("Error: LinkedIn username or password not found in environment variables");
    }

    driver.FindElement(By.Id("username")).SendKeys(username);
    driver.FindElement(By.Id("password")).SendKeys(password);

    IWebElement checkbox = driver.FindElement(By.XPath("//input[@name='rememberMeOptIn']"));

                if (checkbox.Selected)
                {
                    ((IJavaScriptExecutor) driver).ExecuteScript("arguments[0].click();", checkbox);
}

System.Threading.Thread.Sleep(2000);
driver.FindElement(By.XPath("//button[@data-litms-control-urn='login-submit']")).Click();
    }
}
