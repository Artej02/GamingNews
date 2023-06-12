using GamingWeb.Custom.DatabaseHelpers;
using GamingWeb.Custom.Helpers;
using GamingWeb.Custom.Models.Product;
using GamingWeb.Custom.Models.User;
using Kendo.Mvc.UI;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace GamingWeb.Controllers
{
    [Authorize]
    public class ProductsController : Controller
    {
        public IActionResult Index()
        {
            ViewBag.Products = true;
            return View();
        }

        public async Task<IActionResult> Read_Products([DataSourceRequest] DataSourceRequest request) 
        {
            var query = $"Select Id,Code,Description from [Product] WHERE ParentId IS NULL ";
            var countQuery = $"Select COUNT(*) from [Product] WHERE ParentId IS NULL ";

            string filters = "";
            foreach (var filter in request.Filters)
            {
                filters += KendoGridHelper.ApplyFilter<ProductParentViewModel>(filter);
            }

            var total = await new Query().SelectSingle<int>(countQuery + filters);
            var regionsResult = await new Query().Select<ProductParentViewModel>(query + filters);
            if (regionsResult.HasError || total.HasError)
            {
                return this.Json(new DataSourceResult
                {
                    Errors = "Error Occurred"
                });
            }
            return Json(new { Data = regionsResult.Result, Total = total.Result });
        }

        public async Task<ActionResult> GetAllProducts([DataSourceRequest] DataSourceRequest request)
        {
            var query = $"Select * from [Product]";
            var countQuery = $"Select COUNT(*) from [Product]";
            string filters = "";
            foreach (var filter in request.Filters)
            {
                filters += KendoGridHelper.ApplyFilter<ProductParentViewModel>(filter);
            }
            string sort = KendoGridHelper.ApplySort<ProductParentViewModel>(request.Sorts, "ID Asc");
            if (filters.Trim().Length > 0)
                filters = " WHERE " + filters;
            if (sort.Trim().Length > 0)
                sort = " ORDER BY " + sort;
            var total = await new Query().SelectSingle<int>(countQuery + filters);
            var regionsResult = await new Query().Select<ProductParentViewModel>(query + filters + sort);

            return Json(new { Data = regionsResult.Result, Total = total.Result });
        }

        public async Task<IActionResult> Read_ChildProducts([DataSourceRequest] DataSourceRequest request, int ParentId)
        {
            var query = $"Select ParentId,Code,Description,Percentage,CEFTA,MSA,TRMTL,TVSH,Excise from [Product] WHERE ParentId={ParentId} ";
            var countQuery = $"Select COUNT(*) from [Product] WHERE ParentId={ParentId} ";

            string filters = "";
            foreach (var filter in request.Filters)
            {
                filters += KendoGridHelper.ApplyFilter<ProductChildViewModel>(filter);
            }
              
            var total = await new Query().SelectSingle<int>(countQuery + filters);
            var regionsResult = await new Query().Select<ProductChildViewModel>(query + filters);
            if (regionsResult.HasError || total.HasError)
            {
                return this.Json(new DataSourceResult
                {
                    Errors = "Error Occurred"
                });
            }
            return Json(new { Data = regionsResult.Result, Total = total.Result });
        }
    }
}
