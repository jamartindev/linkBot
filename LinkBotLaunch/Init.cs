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

            if (targetText != null)
            {
                var msgPage = new MessagingPage(driver, targetText);
                Random rand = new Random();
                int seconds = rand.Next(60, 300);
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
            try
            {
                await RetryPolicies.SeleniumAsyncRetryPolicy.ExecuteAsync(async () =>
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
                    });
            }
            catch (TimeoutException ex)
            {

                throw new LoginException($"Failed to login {ex.Message}");
            }
            catch (Exception ex)
            {

                throw new LoginException($"Failed to login {ex.Message}");
            }
        }
        public static async Task InitLogin(IWebDriver driver)
        {
            try
            {
                await RetryPolicies.SeleniumAsyncRetryPolicy.ExecuteAsync(async () =>
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
                    });
            }
            catch (TimeoutException ex)
            {

                throw new LoginException($"Failed to login {ex.Message}");
            }
            catch (Exception ex)
            {

                throw new LoginException($"Failed to login {ex.Message}");
            }
        }
        public static async Task StartLogin(IWebDriver driver, SessionManagement session)
        {
            try
            {
                await RetryPolicies.SeleniumAsyncRetryPolicy.ExecuteAsync(async () =>
                    {
                        await InitLogin(driver);
                        session.StoreCookies();
                        await InitMessagingPage(driver);
                    });
            }
            catch (TimeoutException ex)
            {

                throw new LoginException($"Failed to login {ex.Message}");
            }
            catch (Exception ex)
            {

                throw new LoginException($"Failed to login {ex.Message}");
            }

        }
        public static async Task StartMessagingService(IWebDriver driver)
        {
            try
            {
                var timer = new PeriodicTimer(TimeSpan.FromSeconds(2));
                if (await timer.WaitForNextTickAsync())
                {
                    await RetryPolicies.SeleniumAsyncRetryPolicy.ExecuteAsync(async () =>
                    {
                        await Init.InitMessagingPage(driver);
                    });
                }
            }
            catch (TimeoutException ex)
            {
                throw new MessagePageException(ex.Message);
            }
            catch (Exception ex)
            {

                throw new MessagePageException(ex.Message);
            }
        }
    }
}