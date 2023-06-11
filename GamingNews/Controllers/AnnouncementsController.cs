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

namespace GamingWeb.Controllers
{
    public class AnnouncementsController : Controller
    {

        public async Task<IActionResult> Index()
        {
            //int usersAccess = new AuthorizeHelper(HttpContext).GetPrivilegeLevel((int)Views.Services);
            //if (usersAccess == (int)RoleAccessLevels.AccessDenied)
            //return RedirectToAction("NotAuthorized", "NotAuthorized");

            var language = new LanguageHelper(HttpContext);
            ViewBag.Announcements = true;

            return View();
        }

        public async Task<IActionResult> AddWindow()
        {
            return View();
        }

        public async Task<IActionResult> EditWindow(int announcementId)
        {
            var announcement = (await new Query().SelectSingle<Announcement>($"Select * FROM Announcement where Id={announcementId}")).Result;

            return View(announcement);
        }

        [HttpPost]
        public async Task<IActionResult> Read_Announcements([DataSourceRequest] DataSourceRequest request)
        {
            //Get paginated list from helper
            var response = await new PaginationHelper<Announcement>().GetPagginatedList(request, "Announcement");

            return Json(new { Data = response.Data, Total = response.Total });
        }

        public async Task<ActionResult> CreateUpdateAnnouncement(Announcement announcement)
        {
            var UserId = new AuthorizeHelper(HttpContext).GetUserID();
            bool HasAffected = false;

            var beforeLogData = (await new Query().SelectSingle<Announcement>($"SELECT * FROM Announcement where Id = {announcement.Id}")).Result;

            if (announcement.Id.HasValue)
            {
                var createUpdateResult = await new Query().Execute("CreateUpdateDeleteAnnouncement @CRUDOperation,@Id,@TitleSq,@TitleEn,@TitleSr,@DescriptionSq,@DescriptionEn,@DescriptionSr,@FileUrl,@DisplayDate,@CreatedUserId,@UpdatedUserId", new
                {
                    @CRUDOperation = (int)CRUDOperation.Update,
                    @Id = announcement.Id,
                    @TitleSq = announcement.TitleSq,
                    @TitleEn = announcement.TitleEn,
                    @TitleSr = announcement.TitleSr,
                    @DescriptionSq = announcement.DescriptionSq,
                    @DescriptionEn = announcement.DescriptionEn,
                    @DescriptionSr = announcement.DescriptionSr,
                    @FileUrl = announcement.FileUrl,
                    @DisplayDate = announcement.DisplayDate,
                    @CreatedUserId = announcement.CreatedUserId,
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
                var createUpdateResult = await new Query().ExecuteAndGetInsId("CreateUpdateDeleteAnnouncement @CRUDOperation,@Id,@TitleSq,@TitleEn,@TitleSr,@DescriptionSq,@DescriptionEn,@DescriptionSr,@FileUrl,@DisplayDate,@CreatedUserId,@UpdatedUserId", new
                {
                    @CRUDOperation = (int)CRUDOperation.Create,
                    @Id = announcement.Id,
                    @TitleSq = announcement.TitleSq,
                    @TitleEn = announcement.TitleEn,
                    @TitleSr = announcement.TitleSr,
                    @DescriptionSq = announcement.DescriptionSq,
                    @DescriptionEn = announcement.DescriptionEn,
                    @DescriptionSr = announcement.DescriptionSr,
                    @FileUrl = announcement.FileUrl,
                    @DisplayDate = announcement.DisplayDate,
                    @CreatedUserId = UserId,
                    @UpdatedUserId = announcement.UpdatedUserId
                });

                if (createUpdateResult != 0)
                {
                    HasAffected = true;
                    //var afterLogData = (await new Query().SelectSingle<Service>($"SELECT * FROM Services where Id = {createUpdateResult}")).Result;
                    //var serializedObject = new ChangeLogHelper().SerializeObject(beforeLogData, afterLogData, (int)ChangeLogTable.Categories, UserId, (int)ChangeLogAction.Insert);
                    //var addLog = new ChangeLogHelper().AddLog(serializedObject);
                }
            }

            return RedirectToAction("Index", "Announcements");

        }

        public async Task<IActionResult> DeleteAnnouncement([DataSourceRequest] DataSourceRequest request, Announcement announcement)
        {
            bool HasAffected = false;
            //var beforeLogData = (await new Query().SelectSingle<Custom.Models.User.Role>($"SELECT TOP 1 * FROM [Role] WHERE Id = @ID", new { @ID = role.Id })).Result;

            var deleteDepartmentResult = await new Query().Execute("CreateUpdateDeleteAnnouncement @CRUDOperation,@Id", new
            {
                @CRUDOperation = (int)CRUDOperation.Delete,
                @Id = announcement.Id

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
                var uploads = Path.Combine("wwwroot\\AnnouncementFiles\\");
                string guid = Guid.NewGuid().ToString();
                var filename = DateTime.Now.Year + "_" + guid + Path.GetExtension(file.FileName);
                var filePath = Path.Combine(uploads, filename);

                using (var stream = System.IO.File.Create(filePath))
                {
                    await file.CopyToAsync(stream);
                    filePath = "/AnnouncementFiles/" + filename;
                }
                return Json(new { FileUrl = filePath });
            }
            return Json(new { FileUrl = "" });
        }

    }

}
