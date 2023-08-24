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

            ViewBag.ReceiptsCountThisMonth = (await new Query().SelectSingle<int>($"Select count(*) from Receipts")).Result;
            //var response1 = new CRUDProcedureGenerator().GenerateProcedure("Qualification");
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
