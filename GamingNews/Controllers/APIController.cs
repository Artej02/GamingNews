using GamingNews.Custom.Helpers;
using GamingWeb.Custom.DatabaseHelpers;
using GamingWeb.Custom.Models.Footer;
using GamingWeb.Custom.Models.News;
using GamingWeb.Custom.Models.Services;
using GamingWeb.Custom.Models.User;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.Linq;
using System.Threading.Tasks;

namespace GamingNews.Controllers
{
    [AllowAnonymous]
    public class APIController : Controller
    {
        private IConfiguration Configuration;
        public APIController(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> GetFooter()
        {
            var footer = (await new Query().SelectSingle<Footer>("Select * from Footer")).Result;

            return Ok(footer);
        }

        public async Task<IActionResult> GetNews()
        {
            var footer = (await new Query().Select<News>("Select * from News")).Result.ToList();

            return Ok(footer);
        }

        public async Task<IActionResult> GetServices()
        {
            var footer = (await new Query().Select<Service>("Select * from Service")).Result.ToList();

            return Ok(footer);
        }

        public async Task<IActionResult> GetUsers()
        {
            var footer = (await new Query().Select<User>("Select * from [User]")).Result.ToList();

            return Ok(footer);
        }

        public ActionResult Email(string name, string username, string msg)
        {
            MailHelper mailHelper = new MailHelper(Configuration);

            return Json(mailHelper.SendEmail(name, username, msg));
        }

    }
}
