using LinkBotLogic.Utilities;
using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace LinkBotLogic.DTOs
{
    public class CookieDto
    {
        public string Name { get; set; }
        public string Value { get; set; }
        public string Domain { get; set; }
        public string Path { get; set; }
        public bool Secure { get; set; }
        public bool IsHttpOnly { get; set; }
        public string SameSite { get; set; }
        [JsonConverter(typeof(JSONDateTimeConverter))]
        public DateTime? Expiry { get; set; }
        

        public CookieDto() { }

        public CookieDto(Cookie cookie)
        {
            Name = cookie.Name;
            Value = cookie.Value;
            Domain = cookie.Domain;
            Path = cookie.Path;
            Secure = cookie.Secure;
            IsHttpOnly = cookie.IsHttpOnly;
            SameSite = cookie.SameSite;
            Expiry = cookie.Expiry;
        }

        public Cookie ToCookie()
        {
            return new Cookie(Name, Value, Domain, Path, Expiry, Secure, IsHttpOnly, SameSite);
        }
    }
}
