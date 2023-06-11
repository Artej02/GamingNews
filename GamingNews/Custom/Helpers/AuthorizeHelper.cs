using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using Newtonsoft.Json;
using GamingWeb.Custom.Models.Account;
using GamingWeb.Custom.Models.User;
using GamingWeb.Custom.DatabaseHelpers;

namespace GamingWeb.Custom.Helpers
{
    public class AuthorizeHelper
    {
        private const string IDUser = "IDUser";
        private const string IDOrganization = "organization_id";
        private const string ViewAuthorization = "ViewAuthorization";
        private const string Language = "Language";
        private const string ImagePath = "photo_url";
        private const string FullName = "FullName";


        private HttpContext httpContext;

        public AuthorizeHelper(HttpContext httpContext) => this.httpContext = httpContext;

        public async Task SetAuthentication(User user, bool isPersistent, string language, string imagePath)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Username),
                new Claim(ClaimTypes.Name, String.Join(' ',user.Name,user.Surname)),
                new Claim(IDUser, user.Id.ToString()),
                //new Claim(IDOrganization, user.organization_id.ToString()),
                new Claim(ClaimTypes.Role, user.RoleId.ToString()),
                new Claim(Language, language),
                new Claim(ImagePath, imagePath),
                new Claim(FullName, String.Join(' ',user.Name,user.Surname))
            };
            var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            var authProperties = new AuthenticationProperties
            {
                IsPersistent = false,
                ExpiresUtc = DateTime.Now.AddHours(8),
                AllowRefresh = true
            };

            await httpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                new ClaimsPrincipal(claimsIdentity),
                authProperties);
        }

        public Dictionary<int, int> GetViewAuthorization()
        {
            return JsonConvert.DeserializeObject<Dictionary<int, int>>(httpContext.User.FindFirst(ViewAuthorization)?.Value);
        }

        public int GetUserOrganization()
        {
            return int.Parse(httpContext.User.FindFirst(IDOrganization)?.Value);
        }

        public string getUserFirstLastName()
        {
            return httpContext.User.FindFirst(FullName)?.Value.ToString();
        }

        public string GetUserName()
        {
            return httpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value.ToString();
        }

        public string GetUserFullName()
        {
            return httpContext.User.FindFirst(ClaimTypes.Name)?.Value.ToString();
        }

        public int GetUserRole()
        {
            return int.Parse(httpContext.User.FindFirst(ClaimTypes.Role)?.Value);
        }

        public int GetCurYear()
        {
            return DateTime.Now.Year;
        }

        public int GetUserID()
        {
            var usri = System.Security.Claims.ClaimsPrincipal.Current;
            return int.Parse(httpContext.User.FindFirst(IDUser)?.Value);
        }

        public int GetUserOrganizationID()
        {
            var usri = System.Security.Claims.ClaimsPrincipal.Current;
            return int.Parse(httpContext.User.FindFirst(IDOrganization)?.Value);
        }

        public string GetOrganizationImage()
        {
            int organizationID = GetUserOrganizationID();
            return (new Query().SelectSingle<string>($"select top 1 FilePath from Organizations where id = {organizationID}").Result.Result);
        }
        public string GetUserOrganizationName()
        {
            int organizationID = this.GetUserOrganizationID();
            string organizationName = new Query().SelectSingle<string>($"select top 1 organization_name from Organizations where id = {organizationID}").Result.Result;
            return organizationName;
        }

        public int GetUserRoleID()
        {
            var userID = GetUserID();
            return (new Query().SelectSingle<int>($"select top 1 role_id from [Users] where id = {userID}").Result.Result);
        }

        public string GetUserLanguage()
        {
            return httpContext.User.FindFirst(Language)?.Value.ToString();
        }

        public string GetUserImagePath()
        {
            return httpContext.User.FindFirst(ImagePath)?.Value.ToString();
        }
    }
}
