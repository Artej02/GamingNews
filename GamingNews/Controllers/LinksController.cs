using GamingWeb.Custom.DatabaseHelpers;
using GamingWeb.Custom.Helpers;
using GamingWeb.Custom.Models.Reports;
using Kendo.Mvc.UI;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.IO;
using System.Threading.Tasks;
using System;
using GamingWeb.Custom.Models;
using GamingWeb.Custom.Models.Links;

namespace GamingWeb.Controllers
{
    public class LinksController : Controller
    {
        public IActionResult Index()
        {
            ViewBag.Links = true;
            return View();
        }

        public async Task<IActionResult> AddWindow()
        {
            return View();
        }

        public async Task<IActionResult> EditWindow(int linkId)
        {
            var report = (await new Query().SelectSingle<Link>($"Select * FROM Link where Id={linkId}")).Result;

            return View(report);
        }

        [HttpPost]
        public async Task<IActionResult> Read_Links([DataSourceRequest] DataSourceRequest request)
        {
            //Get paginated list from helper
            var response = await new PaginationHelper<Link>().GetPagginatedList(request, "Link");

            return Json(new { Data = response.Data, Total = response.Total });
        }

        public async Task<ActionResult> CreateUpdateLink(Link link)
        {
            var UserId = new AuthorizeHelper(HttpContext).GetUserID();
            bool HasAffected = false;

            var beforeLogData = (await new Query().SelectSingle<Link>($"SELECT * FROM Link where Id = {link.Id}")).Result;

            if (link.Id.HasValue)
            {
                var createUpdateResult = await new Query().Execute("CreateUpdateDeleteLink @CRUDOperation,@Id,@TitleSq,@TitleEn,@TitleSr,@Icon,@Url,@CreatedUserId,@UpdatedUserId", new
                {
                    @CRUDOperation = (int)CRUDOperation.Update,
                    @Id = link.Id,
                    @TitleSq = link.TitleSq,
                    @TitleEn = link.TitleEn,
                    @TitleSr = link.TitleSr,
                    @Icon = link.Icon,
                    @Url = link.Url,
                    @CreatedUserId = link.CreatedUserId,
                    @UpdatedUserId = UserId
                });
                if (createUpdateResult.HasAffected)
                {
                    HasAffected = true;
                    //var afterLogData = (await new Query().SelectSingle<Service>($"SELECT * FROM Services where Id = {service.Id}")).Result;
                    //var serializedObject = new ChangeLogHelper().SerializeObject(beforeLogData, afterLogData, (int)ChangeLogTable.Service, UserId, (int)ChangeLogAction.Update);
                    //var addLog = new ChangeLogHelper().AddLog(serializedObject);
                }
            }
            else
            {
                var createUpdateResult = await new Query().ExecuteAndGetInsId("CreateUpdateDeleteLink @CRUDOperation,@Id,@TitleSq,@TitleEn,@TitleSr,@Icon,@Url,@CreatedUserId,@UpdatedUserId", new
                {
                    @CRUDOperation = (int)CRUDOperation.Create,
                    @Id = link.Id,
                    @TitleSq = link.TitleSq,
                    @TitleEn = link.TitleEn,
                    @TitleSr = link.TitleSr,
                    @Icon = link.Icon,
                    @Url = link.Url,
                    @CreatedUserId = UserId,
                    @UpdatedUserId = link.UpdatedUserId
                });

                if (createUpdateResult != 0)
                {
                    HasAffected = true;
                    //var afterLogData = (await new Query().SelectSingle<Service>($"SELECT * FROM Services where Id = {createUpdateResult}")).Result;
                    //var serializedObject = new ChangeLogHelper().SerializeObject(beforeLogData, afterLogData, (int)ChangeLogTable.Categories, UserId, (int)ChangeLogAction.Insert);
                    //var addLog = new ChangeLogHelper().AddLog(serializedObject);
                }
            }

            return RedirectToAction("Index", "Links");

        }

        public async Task<IActionResult> DeleteLink([DataSourceRequest] DataSourceRequest request, Link link)
        {
            bool HasAffected = false;
            //var beforeLogData = (await new Query().SelectSingle<Custom.Models.User.Role>($"SELECT TOP 1 * FROM [Role] WHERE Id = @ID", new { @ID = role.Id })).Result;

            var deleteDepartmentResult = await new Query().Execute("CreateUpdateDeleteLink @CRUDOperation,@Id", new
            {
                @CRUDOperation = (int)CRUDOperation.Delete,
                @Id = link.Id

            });
            if (deleteDepartmentResult.HasAffected)
            {
                HasAffected = true;
            }

            return Json(HasAffected);
        }

        [HttpPost]
        public async Task<ActionResult> SaveFile([Bind(Prefix = "FileUrl")] IFormFile file)
        {
            var filepath = file.FileName;
            if (file.Length > 0)
            {
                var uploads = Path.Combine("wwwroot\\LinkFiles\\");
                string guid = Guid.NewGuid().ToString();
                var filename = DateTime.Now.Year + "_" + guid + Path.GetExtension(file.FileName);
                var filePath = Path.Combine(uploads, filename);

                using (var stream = System.IO.File.Create(filePath))
                {
                    await file.CopyToAsync(stream);
                    filePath = "/LinkFiles/" + filename;
                }
                return Json(new { Icon = filePath });
            }
            return Json(new { Icon = "" });
        }
    }
}
