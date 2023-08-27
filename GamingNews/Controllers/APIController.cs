using GamingWeb.Custom.DatabaseHelpers;
using GamingWeb.Custom.Models.Footer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace GamingNews.Controllers
{
    [AllowAnonymous]
    public class APIController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> GetFooter()
        {
            var footer = (await new Query().SelectSingle<Footer>("Select * from Footer")).Result;

            return Ok(footer);
        }
    }
}
