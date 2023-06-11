using Microsoft.AspNetCore.Http;
using System;
using System.Globalization;
using System.Reflection;
using System.Resources;
using System.Threading;

namespace GamingWeb.Custom.Helpers
{
    public class LanguageHelper
    {
        private HttpContext httpContext;
        public string CurrentCulture { get; set; }

        public LanguageHelper(HttpContext httpContext)
        {
            this.httpContext = httpContext;
            var selectedLanguage = new CookieHelper(httpContext).Get("PreferedLanguage");
            var isSelectedLanguageNull = string.IsNullOrEmpty(selectedLanguage);
            if (isSelectedLanguageNull) {
                new CookieHelper(httpContext).Set("PreferedLanguage", "en");
                selectedLanguage = "en";
            }

            CurrentCulture = selectedLanguage;
            CultureInfo cultureInfo = new CultureInfo(CurrentCulture);
            Thread.CurrentThread.CurrentUICulture = cultureInfo;
            Thread.CurrentThread.CurrentCulture = CultureInfo.CreateSpecificCulture(cultureInfo.Name);
            CultureInfo.DefaultThreadCurrentCulture = new CultureInfo(CurrentCulture);
        }

        public LanguageHelper(HttpContext httpContext,string value)
        {
            this.httpContext = httpContext;
            new CookieHelper(httpContext).Set("PreferedLanguage", value);
            CultureInfo cultureInfo = new CultureInfo(value);
            CurrentCulture = value == null ? "en" : value;
            Thread.CurrentThread.CurrentUICulture = cultureInfo;
            Thread.CurrentThread.CurrentCulture = CultureInfo.CreateSpecificCulture(cultureInfo.Name);
            CultureInfo.DefaultThreadCurrentCulture = new CultureInfo(value);
        }

        public string Current()
        {
            return new CookieHelper(httpContext).Get("PreferedLanguage");
        }

        public string Get(string value)
        {
            if (string.IsNullOrEmpty(value))
                return null;
            CultureInfo cultureInfo;
            try
            {
                cultureInfo = CultureInfo.CreateSpecificCulture(CurrentCulture);
            }
            catch (Exception e)
            {
                cultureInfo = CultureInfo.CreateSpecificCulture("en");
            }

            ResourceManager resourceManager = new ResourceManager("GamingNews.Resources.Resources", Assembly.GetExecutingAssembly());
            return resourceManager.GetString(value, cultureInfo);
        }
    }
}
