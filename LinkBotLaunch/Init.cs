using LinkBotLogic.Exceptions;
using LinkBotLogic.Pages;
using LinkBotLogic.Services;
using OpenQA.Selenium;

namespace LinkBotLaunch
{
    internal class Init()
    {
        public static async Task InitMessagingPage(IWebDriver driver)
        {
            string? targetText = Environment.GetEnvironmentVariable("CONTACT_TARGET");
            string time1 = Environment.GetEnvironmentVariable("TIME_INTERVAL1");
            string time2 = Environment.GetEnvironmentVariable("TIME_INTERVAL2");

            if (targetText != null)
            {
                var msgPage = new MessagingPage(driver, targetText);
                Random rand = new Random();
                int seconds = rand.Next(int.Parse(time1), int.Parse(time2));
                TimeSpan delayTime = TimeSpan.FromSeconds(seconds);

                msgPage.Open();

                while (true)
                {
                    await Task.Delay(delayTime);
                    await msgPage.SendMessage(targetText);
                }
            }
        }
        public static async Task InitLogin2(IWebDriver driver)
        {
            string? password = Environment.GetEnvironmentVariable("LINKEDIN_PASS");
            string? username = Environment.GetEnvironmentVariable("LINKEDIN_USER");
            var loginPage = new LoginPage(driver);
            if (password != null)
            {
                loginPage.Go();
                await Task.Delay(1000);
                loginPage.Login2(username, password);

            }
        }
        public static async Task InitLogin(IWebDriver driver)
        {
            string? username = Environment.GetEnvironmentVariable("LINKEDIN_USER");
            string? password = Environment.GetEnvironmentVariable("LINKEDIN_PASS");

            var loginPage = new LoginPage(driver);
            loginPage.Go();

            if (username != null && password != null)
            {
                loginPage.Login(username, password);
                await Task.Delay(1000);
            }
        }
        public static async Task StartLogin(IWebDriver driver, SessionManagement session)
        {
            await InitLogin(driver);
            session.StoreCookies();
            await InitMessagingPage(driver);
        }
        public static async Task StartMessagingService(IWebDriver driver)
        {
            var timer = new PeriodicTimer(TimeSpan.FromSeconds(2));
            if (await timer.WaitForNextTickAsync())
            {
                await Init.InitMessagingPage(driver);
            }
        }
    }
}