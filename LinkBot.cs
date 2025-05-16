using OpenQA.Selenium.Chrome;
using OpenQA.Selenium;

namespace LinkedInBot
{
    public class LinkBot
    {
        static void Main(string[] args)
        {
            IWebDriver driver = new ChromeDriver("D:/Downloads/chrome - win64/chrome.exe");
            driver.Manage().Window.Maximize();

            driver.Navigate().GoToUrl("https://www.linkedin.com");

            System.Threading.Thread.Sleep(5000);

            //// Close the browser
            //driver.Quit();
        }
    }
}
