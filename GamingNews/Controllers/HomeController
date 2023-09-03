using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using GamingWeb.Custom.DatabaseHelpers;
using GamingWeb.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using GamingWeb.Custom.Helpers;
using GamingWeb.Custom.Models.User;

namespace GamingWeb.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        //private readonly ILogger<HomeController> _logger;

        //public HomeController(ILogger<HomeController> logger)
        //{
        //    _logger = logger;
        //}

        public async Task<IActionResult> Index()
        {
            ViewBag.Current = "Home";

            int thisYear = DateTime.Now.Year, thisMonth = DateTime.Now.Month;
            int lastDayOfThisMonth = DateTime.DaysInMonth(thisYear, thisMonth);

            int userID = new AuthorizeHelper(HttpContext).GetUserID();

            ViewBag.ReceiptsCountThisMonth = (await new Query().SelectSingle<int>($"Select count(*) from Receipts")).Result;
            //var response1 = new CRUDProcedureGenerator().GenerateProcedure("Qualification");

            ViewBag.NewsCount = (await new Query().SelectSingle<int>("SELECT COUNT(*) FROM News WHERE CreatedUserId = @UserID AND MONTH(ReleaseDate) = @Month AND YEAR(ReleaseDate) = @Year", new
            {
                @UserID = userID,
                @Month = DateTime.Now.Month,
                @Year = DateTime.Now.Year
            })).Result;

            ViewBag.AnnouncementsCount = (await new Query().SelectSingle<int>("SELECT COUNT(*) FROM Announcement WHERE CreatedUserId = @UserID AND MONTH(DisplayDate) = @Month AND YEAR(DisplayDate) = @Year", new
            {
                @UserID = userID,
                @Month = DateTime.Now.Month,
                @Year = DateTime.Now.Year
            })).Result;

            ViewBag.DocumentsCount = (await new Query().SelectSingle<int>("SELECT COUNT(*) FROM Documents WHERE CreatedUserId = @UserID AND MONTH(DisplayDate) = @Month AND YEAR(DisplayDate) = @Year", new
            {
                @UserID = userID,
                @Month = DateTime.Now.Month,
                @Year = DateTime.Now.Year
            })).Result;

            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
