using OpenQA.Selenium;
using System.Text.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LinkBotLogic.DTOs;
using LinkBotLogic.Exceptions;

namespace LinkBotLogic.Services
{
    public class SessionManagement
    {
        private IWebDriver Driver { get; }

        public SessionManagement(IWebDriver driver)
        {
            Driver = driver;
        }

        public void StoreCookies()
        {
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
            };
            var cookies = Driver.Manage().Cookies.AllCookies;
            var cookieDtos = cookies.Select(c => new CookieDto(c)).ToList();
            var json = JsonSerializer.Serialize(cookieDtos, options);
            File.WriteAllText("linkedin_cookies.json", json);
        }

        public string? GetJson()
        {
            if (!File.Exists("linkedin_cookies.json"))
                return null;
            return File.ReadAllText("linkedin_cookies.json");
        }

        public void LoadCookies()
        {
            Driver.Navigate().GoToUrl("https://www.linkedin.com/login");
            var json = GetJson();

            if (string.IsNullOrEmpty(json)) throw new CookieStorageException("Cookie file not found");

            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
            };
            var cookieDtos = JsonSerializer.Deserialize<List<CookieDto>>(json, options);
            if (cookieDtos == null || !cookieDtos.Any()) return;

            foreach (var dto in cookieDtos)
            {
                var cookie = dto.ToCookie();
                Driver.Manage().Cookies.AddCookie(cookie);
            }
        }
        public bool HasValidCookies =>
            File.Exists("linkedin_cookies.json") &&
            File.GetLastWriteTime("linkedin_cookies.json") > DateTime.Now.AddDays(-7);

        public void ResetSession()
        {
            if (this.HasValidCookies)
            {
                File.Delete("linkedin_cookies.json");
                File.Delete("chromedriver.log");
            }
        }
    }
}

