using GamingWeb.Custom.DatabaseHelpers;
using GamingWeb.Custom.Helpers;
using GamingWeb.Custom.Models.Account;
using GamingWeb.Custom.Models.User;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using GamingWeb.Custom.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GamingWeb.Controllers
{
    public class AccountController : Controller
    {
        [HttpGet]
        [AllowAnonymous]
        public IActionResult Index()
        {
            if (User.Identity.IsAuthenticated)
                return RedirectToAction(controllerName: "Home", actionName: "Index");
            return RedirectToAction("Login");
        }
        [HttpGet]
        [AllowAnonymous]
        public IActionResult Login()
        {
            if (User.Identity.IsAuthenticated)
                return RedirectToAction(controllerName: "Home", actionName: "Index");
            return View(viewName: "login");
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Login(Login login)
        {
            var language = new LanguageHelper(HttpContext);
            if (ModelState.IsValid)
            {
                var user = (await new Query().SelectSingle<User>("SELECT * FROM [User] WHERE Username = @Username", new { @Username = login.Username }));


                if (user.HasError)
                {
                    ModelState.AddModelError(String.Empty, user.ErrorMessage);
                    return View(login);
                }
                else
                {
                    if (user.HasData && PasswordHelper.Verify(user.Result.Salt, user.Result.Password, login.Password))
                    {
                        await new AuthorizeHelper(HttpContext).SetAuthentication(user.Result, isPersistent: login.RememberLogin, language.Current(), "");

                        return RedirectToAction(controllerName: "Home", actionName: "Index");
                    }
                    else
                    {
                        ModelState.Clear();
                        login.Username = String.Empty;
                        login.Password = String.Empty;
                        ModelState.AddModelError(String.Empty, language.Get("UsernamePasswordIncorrect"));
                        return View(viewName: "login", model: login);
                    }
                }
            }
            else
            {
                ModelState.AddModelError(String.Empty, language.Get("FillUsernameAndPassword"));
                return View(login);
            }
        }

        public async Task<ActionResult> SignOut()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Login");
        }

        public ActionResult AccessDenied()
        {
            return RedirectToAction(actionName: "Index", controllerName: "Home");
        }

        public ActionResult ChangeLanguage(string language, string url)
        {
            new LanguageHelper(HttpContext, language);
            return Redirect(url);
        }
    }
}
