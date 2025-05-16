using LinkBotLogic.Services;
using LinkBotLogic.Exceptions;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;

namespace LinkBotLaunch
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            var options = new ChromeOptions();

            options.AddArgument("--headless");
            options.AddArgument("--mute-audio");
            options.AddArgument("--enable-unsafe-swiftshader");
            options.AddArgument("--use-gl=angle");
            options.AddArgument("--use-angle=swiftshader-webgl");
            options.AddArgument("--window-size=1920,1080");
            options.AddArgument("--disable-extensions");
            options.AddArgument("--no-sandbox");
            options.AddArgument("--disable-dev-shm-usage");
            options.AddArgument("--profile-directory=Default");

            var service = ChromeDriverService.CreateDefaultService();
            service.EnableVerboseLogging = true;
            service.LogPath = "chromedriver.log";

            IWebDriver driver = new ChromeDriver(service, options);
            var session = new SessionManagement(driver);

            try
            {
                if (session.HasValidCookies)
                {
                    session.LoadCookies();
                    driver.Navigate().Refresh();
                    await Init.InitLogin2(driver);
                    await Init.StartMessagingService(driver);
                } else
                {
                    await Init.StartLogin(driver, session);
                    await Init.StartMessagingService(driver);
                }
            }
            //catch (LoginException ex)
            //{
            //    Console.WriteLine($"Login error: {ex.Message}");
            //    await Init.StartLogin(driver, session);
            //    await Init.StartMessagingService(driver);
            //}
            //catch (MessagePageException ex)
            //{
            //    Console.WriteLine($"Login error: {ex.Message}");
            //}
            //catch (CookieStorageException ex)
            //{
            //    Console.WriteLine($"Cookie error: {ex.Message}");
            //    await Init.StartLogin(driver, session);
            //    await Init.StartMessagingService(driver);
            catch (Exception ex)
            {
                session.ResetSession();
                driver.Navigate().Refresh();
                await Init.StartLogin(driver, session);
                await Init.StartMessagingService(driver);
                Console.WriteLine($"ERROR: {ex}");
            }
        }
    }
}