using LinkBotLogic.Services;
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

            do
            {
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
                        await RetryPolicies.SeleniumAsyncRetryPolicy.ExecuteAsync(async () =>
                        {
                            await Init.InitLogin2(driver);
                            await Init.StartMessagingService(driver);
                        });
                    }
                    else
                    {
                        await RetryPolicies.SeleniumAsyncRetryPolicy.ExecuteAsync(async () =>
                        {
                            await Init.StartLogin(driver, session);
                            await Init.StartMessagingService(driver);
                        });
                    }
                }
                catch (Exception ex)
                {
                    driver.Quit();
                    driver.Dispose();
                    Thread.Sleep(10000);
                    RetryPolicies.SeleniumRetryPolicy.Execute(() =>
                    {
                        SessionManagement.ResetSession();
                    });
                    Console.WriteLine($"ERROR: {ex}");
                } 
            } while (true);
        }
    }
}