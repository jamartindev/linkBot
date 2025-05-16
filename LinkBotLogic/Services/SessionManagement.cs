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
            try
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
            catch (Exception ex)
            {
                Console.WriteLine($"Error storing cookies: {ex.Message}");
            }
        }

        public string? GetJson()
        {
            if (!File.Exists("linkedin_cookies.json"))
                return null;

            try
            {
                return File.ReadAllText("linkedin_cookies.json");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error reading cookies: {ex.Message}");
                return null;
            }
        }

        public void LoadCookies()
        {
            Driver.Navigate().GoToUrl("https://www.linkedin.com/login");
            var json = GetJson();

            if (string.IsNullOrEmpty(json)) throw new CookieStorageException("Cookie file not found");

            try
            {
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
            catch (FileNotFoundException ex)
            {
                throw new CookieStorageException("Cookie file not found.", ex);
            }
            catch (JsonException ex)
            {
                throw new CookieStorageException("Invalid cookie data.", ex);
            }
            catch (Exception ex)
            {
                throw new CookieStorageException($"Error loading cookies: {ex.Message}");
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

