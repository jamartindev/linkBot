using LinkBotLogic.Exceptions;
using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LinkBotLogic.Pages
{
    public class LoginPage : BasePage
    {
        private readonly By _userField = By.Id("username");
        private readonly By _passField = By.Id("password");
        private readonly By _checkBox = By.Id("rememberMeOptIn-checkbox");
        private readonly By _submitBtn = By.CssSelector("button[data-litms-control-urn='login-submit']");
        private readonly By _signInBtn = By.CssSelector(".btn__primary--large.from__button--floating");

        public LoginPage(IWebDriver driver)
            : base(driver, TimeSpan.FromSeconds(15))
        { }

        public void Go()
            => NavigateTo("https://www.linkedin.com/login");

        public void Login(string user, string pass)
        {
            try
            {
                SendKeys(_userField, user);
                SendKeys(_passField, pass);
                IWebElement checkbox = Find(_checkBox);
                if (checkbox.Selected)
                {
                    ((IJavaScriptExecutor)Driver).ExecuteScript("arguments[0].click();", checkbox);
                }
                Click(_submitBtn);
            }
            catch (WebDriverTimeoutException ex)
            {
                Console.WriteLine(ex.Message);
                Login2(user, pass);
            }
            catch (Exception ex)
            {
                throw new LoginException(ex.Message);
            }
        }

        public void Login2(string user, string pass)
        {
            try
            {
                Task.Delay(1000);
                SendKeys(_passField, pass);
                Click(_signInBtn);
            }
            catch (WebDriverTimeoutException ex)
            {
                Console.WriteLine(ex.Message);
                Login(user, pass);
            }
            catch (Exception ex)
            {
                throw new LoginException($"Failed to login {ex.Message}");
            }
        }
        public bool IsInteractable(By element)
        {
            try
            {
                IWebElement el = Find(element);
                if (el.Displayed && el.Enabled)
                {
                    return true;
                } else
                {
                    return false;
                }
            }
            catch (WebDriverTimeoutException ex)
            {
                throw new LoginException($"Failed to login {ex.Message}");
            }
        }
    }
}