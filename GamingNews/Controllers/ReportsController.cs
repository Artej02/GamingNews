using GamingWeb.Custom.DatabaseHelpers;
using GamingWeb.Custom.Helpers;
using GamingWeb.Custom.Models;
using Kendo.Mvc.UI;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.IO;
using System.Threading.Tasks;
using GamingWeb.Custom.Models.Announcements;
using GamingWeb.Custom.Models.Reports;

namespace GamingWeb.Controllers
{
    public class ReportsController : Controller
    {

        public async Task<IActionResult> Index()
        {
            //int usersAccess = new AuthorizeHelper(HttpContext).GetPrivilegeLevel((int)Views.Services);
            //if (usersAccess == (int)RoleAccessLevels.AccessDenied)
            //return RedirectToAction("NotAuthorized", "NotAuthorized");

            var language = new LanguageHelper(HttpContext);
            ViewBag.Reports = true;

            return View();
        }

        public async Task<IActionResult> AddWindow()
        {
            return View();
        }

        public async Task<IActionResult> EditWindow(int reportId)
        {
            var report = (await new Query().SelectSingle<Report>($"Select * FROM Report where Id={reportId}")).Result;

            return View(report);
        }

        [HttpPost]
        public async Task<IActionResult> Read_Reports([DataSourceRequest] DataSourceRequest request)
        {
            //Get paginated list from helper
            var response = await new PaginationHelper<Report>().GetPagginatedList(request, "Report");

            return Json(new { Data = response.Data, Total = response.Total });
        }

        public async Task<ActionResult> CreateUpdateReport(Report report)
        {
            var UserId = new AuthorizeHelper(HttpContext).GetUserID();
            bool HasAffected = false;

            var beforeLogData = (await new Query().SelectSingle<Report>($"SELECT * FROM Report where Id = {report.Id}")).Result;

            if (report.Id.HasValue)
            {
                var createUpdateResult = await new Query().Execute("CreateUpdateDeleteReport @CRUDOperation,@Id,@TitleSq,@TitleEn,@TitleSr,@FileUrl,@DisplayDate,@CreatedUserId,@UpdatedUserId", new
                {
                    @CRUDOperation = (int)CRUDOperation.Update,
                    @Id = report.Id,
                    @TitleSq = report.TitleSq,
                    @TitleEn = report.TitleEn,
                    @TitleSr = report.TitleSr,
                    @FileUrl = report.FileUrl,
                    @DisplayDate = report.DisplayDate,
                    @CreatedUserId = report.CreatedUserId,
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
                var createUpdateResult = await new Query().ExecuteAndGetInsId("CreateUpdateDeleteReport @CRUDOperation,@Id,@TitleSq,@TitleEn,@TitleSr,@FileUrl,@DisplayDate,@CreatedUserId,@UpdatedUserId", new
                {
                    @CRUDOperation = (int)CRUDOperation.Create,
                    @Id = report.Id,
                    @TitleSq = report.TitleSq,
                    @TitleEn = report.TitleEn,
                    @TitleSr = report.TitleSr,
                    @FileUrl = report.FileUrl,
                    @DisplayDate = report.DisplayDate,
                    @CreatedUserId = UserId,
                    @UpdatedUserId = report.UpdatedUserId
                });

                if (createUpdateResult != 0)
                {
                    HasAffected = true;
                    //var afterLogData = (await new Query().SelectSingle<Service>($"SELECT * FROM Services where Id = {createUpdateResult}")).Result;
                    //var serializedObject = new ChangeLogHelper().SerializeObject(beforeLogData, afterLogData, (int)ChangeLogTable.Categories, UserId, (int)ChangeLogAction.Insert);
                    //var addLog = new ChangeLogHelper().AddLog(serializedObject);
                }
            }

            return RedirectToAction("Index", "Reports");

        }

        public async Task<IActionResult> DeleteReport([DataSourceRequest] DataSourceRequest request, Report report)
        {
            bool HasAffected = false;
            //var beforeLogData = (await new Query().SelectSingle<Custom.Models.User.Role>($"SELECT TOP 1 * FROM [Role] WHERE Id = @ID", new { @ID = role.Id })).Result;

            var deleteDepartmentResult = await new Query().Execute("CreateUpdateDeleteReport @CRUDOperation,@Id", new
            {
                @CRUDOperation = (int)CRUDOperation.Delete,
                @Id = report.Id

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
                var uploads = Path.Combine("wwwroot\\ReportFiles\\");
                string guid = Guid.NewGuid().ToString();
                var filename = DateTime.Now.Year + "_" + guid + Path.GetExtension(file.FileName);
                var filePath = Path.Combine(uploads, filename);

                using (var stream = System.IO.File.Create(filePath))
                {
                    await file.CopyToAsync(stream);
                    filePath = "/ReportFiles/" + filename;
                }
                return Json(new { FileUrl = filePath });
            }
            return Json(new { FileUrl = "" });
        }

    }

}
