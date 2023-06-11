using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
using System.Threading.Tasks;

namespace GamingWeb.Custom.Helpers
{
    public static class GenericPrincipalExtensions
    {
        public static string ImagePath(this IPrincipal user, HttpContext httpContext)
        {
            if (user.Identity.IsAuthenticated)
            {
                //ClaimsIdentity claimsIdentity = user.Identity as ClaimsIdentity;
                //var imagePath = claimsIdentity.Claims.First(f => f.Type == "ImagePath");
                var imagePath = new CookieHelper(httpContext).Get("ImagePath");
                if (imagePath == null || imagePath == "")
                {
                    return "/images/photos/profile_user.png";
                }
                else
                {
                    return imagePath;
                }
            }
            else
                return "/images/photos/profile_user.png";
        }

        public static string ImagePath(string imagePath)
        {
            if (imagePath == null || imagePath == "")
            {
                return "~/images/photos/profile_user.png";
            }
            else
            {
                return imagePath;
            }
        }
    }
}
