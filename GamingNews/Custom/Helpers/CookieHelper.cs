using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;

namespace GamingWeb.Custom.Helpers
{
    public class CookieHelper
    {
        HttpContext httpContext;

        public CookieHelper(HttpContext httpContext)
        {
            this.httpContext = httpContext;
        }

        public void Set(string key, string data)
        {
            CookieOptions option = new CookieOptions();
            option.Expires = DateTime.Now.AddYears(1);
            option.IsEssential = true;
            option.SameSite = SameSiteMode.None;
            option.HttpOnly = false;
            option.Secure = true;
            httpContext.Response.Cookies.Append(key, data, option);
        }

        public void Set(string key, Dictionary<string, string> values)
        {
            CookieOptions option = new CookieOptions();
            option.Expires = DateTime.Now.AddYears(1);
            option.IsEssential = true;
            option.SameSite = SameSiteMode.None;
            option.HttpOnly = false;
            option.Secure = true;
            foreach (var item in values)
            {
                httpContext.Response.Cookies.Append(item.Key, item.Value, option);
            }
        }

        public string Get(string key)
        {
            if (httpContext.Request.Cookies[key] != null)
                return httpContext.Request.Cookies[key];
            else
                return string.Empty;
        }

        public void Remove(string key)
        {
            httpContext.Response.Cookies.Delete(key);
        }
    }
}
