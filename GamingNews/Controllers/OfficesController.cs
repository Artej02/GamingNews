using GamingWeb.Custom.DatabaseHelpers;
using GamingWeb.Custom.Helpers;
using GamingWeb.Custom.Models;
using GamingWeb.Custom.Models.Office;
using GamingWeb.Custom.Models.Schedule;
using GamingWeb.Custom.Models.Sector;
using GamingWeb.Custom.Models.Services;
using Kendo.Mvc.UI;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace GamingWeb.Controllers
{
    public class OfficesController : Controller
    {
        ScheduleCreator scheduler = new ScheduleCreator();

        public async Task<IActionResult> Index()
        {
            //await GetSectors();
            ViewBag.Offices = true;
            ViewBag.SubMenu = "Organisation";
            return View();
        }

        public async Task<IActionResult> AddWindow()
        {
            //await GetSectors();
            //await GetServices();

            return View();
        }

        public async Task<IActionResult> EditWindow(int officeId)
        {
            Office office = (await new Query().SelectSingle<Office>($"select * from Office where Id={officeId}")).Result;
            ViewBag.Schedules = (await new Query().Select<Schedule>($"Select * FROM Schedule WHERE OfficeId={officeId}")).Result.ToList();
            //await GetSectors();
            //await GetServices();

            return View(office);
        }

        //public async Task<dynamic> GetServices()
        //{
        //    ViewBag.ServicesSq = (await new Query().Select<SelectListItem>("Select Id AS Value, NameSq AS Text FROM Service")).Result;
        //    ViewBag.ServicesEn = (await new Query().Select<SelectListItem>("Select Id AS Value, NameEn AS Text FROM Service")).Result;
        //    ViewBag.ServicesSr = (await new Query().Select<SelectListItem>("Select Id AS Value, NameSr AS Text FROM Service")).Result;

        //    return (ViewBag.ServicesSq, ViewBag.ServicesEn, ViewBag.ServicesSr);
        //}

        //public async Task<dynamic> GetSectors()
        //{
        //    ViewBag.SectorsSq = (await new Query().Select<SelectListItem>("Select Id AS Value, NameSq AS Text FROM Sector")).Result;
        //    ViewBag.SectorsEn = (await new Query().Select<SelectListItem>("Select Id AS Value, NameEn AS Text FROM Sector")).Result;
        //    ViewBag.SectorsSr = (await new Query().Select<SelectListItem>("Select Id AS Value, NameSr AS Text FROM Sector")).Result;

        //    return (ViewBag.SectorsSq, ViewBag.SectorsEn, ViewBag.SectorsSr);
        //}

        public async Task<IActionResult> Read_Offices([DataSourceRequest] DataSourceRequest request)
        {
            var language = new LanguageHelper(HttpContext);

            var response = await new PaginationHelper<Office>().GetPagginatedList(request, "[Office]");
            if (response.HasError)
            {
                return this.Json(new DataSourceResult { Errors = language.Get("ErrorOccurred") });
            }

            return Json(new { Data = response.Data, Total = response.Total });
        }

        public async Task<ActionResult> CreateUpdateOffice([DataSourceRequest] DataSourceRequest request, Office office)
        {
            var UserId = new AuthorizeHelper(HttpContext).GetUserID();
            bool HasAffected = false;
            //var beforeLogData = (await new Query().SelectSingle<Office>($"SELECT TOP 1 * FROM Office where Id = {office.Id}")).Result;

            var enHTML = HttpUtility.HtmlDecode(office.DescriptionEn == null ? "" : office.DescriptionEn.Replace("background-color:#ffffff;", ""));
            var sqHTML = HttpUtility.HtmlDecode(office.DescriptionSq == null ? "" : office.DescriptionSq.Replace("background-color:#ffffff;", ""));
            var srHTML = HttpUtility.HtmlDecode(office.DescriptionSr == null ? "" : office.DescriptionSr.Replace("background-color:#ffffff;", ""));

            if (office.Id.HasValue)
            {

                var createUpdateResult = await new Query().Execute("CreateUpdateDeleteOffice @CRUDOperation,@Id,@NameEn,@NameSq,@NameSr,@DescriptionEn,@DescriptionSq,@DescriptionSr,@CreatedUserId,@UpdatedUserId,@Code,@Contact", new
                {
                    @CRUDOperation = office.Id.HasValue ? (int)CRUDOperation.Update : (int)CRUDOperation.Create,
                    @Id = office.Id,
                    @NameEn = office.NameEn,
                    @NameSq = office.NameSq,
                    @NameSr = office.NameSr,
                    @DescriptionEn = enHTML,
                    @DescriptionSq = sqHTML,
                    @DescriptionSr = srHTML,
                    @CreatedUserId = office.CreatedUserId,
                    @UpdatedUserId = UserId,
                    @Code = office.Code,
                    @Contact = office.Contact

                });
                if (createUpdateResult.HasAffected)
                {
                    //Deleting all services and schedules and inserting them all over with the updated values
                    //await scheduler.DeleteSchedulesForOrg(null, null, null, office.Id);
                    //await servicer.DeleteServicesForOrg(null, null, null, office.Id);

                    //Create Schedules
                    if (office.Schedules != null)
                    {
                        foreach (var schedule in office.Schedules)
                        {
                            if (schedule.Id.HasValue)
                            {
                                 scheduler.CreateSchedule(schedule, null, null, null, office.Id);
                            }
                        }
                    }
                    //Create Services
                    //if (office.OrgServices != null)
                    //{
                    //    foreach (var orgService in office.OrgServices)
                    //    {
                    //        if (orgService.Id.HasValue)
                    //        {
                    //            await servicer.AddOrgService(orgService, null, null, null, office.Id);
                    //        }
                    //    }
                    //}

                    HasAffected = true;
                    //var afterLogData = (await new Query().SelectSingle<Office>($"SELECT TOP 1 * FROM Office where Id = {office.Id}")).Result;
                    //var serializedObject = new ChangeLogHelper().SerializeObject(beforeLogData, afterLogData, (int)ChangeLogTable.Offices, UserId, (int)ChangeLogAction.Update);
                    //var addLog = new ChangeLogHelper().AddLog(serializedObject);
                }
            }
            else
            {
                var createUpdateResult = await new Query().ExecuteAndGetInsId("CreateUpdateDeleteOffice @CRUDOperation,@Id,@NameEn,@NameSq,@NameSr,@DescriptionEn,@DescriptionSq,@DescriptionSr,@CreatedUserId,@UpdatedUserId,@Code,@Contact", new
                {
                    @CRUDOperation = office.Id.HasValue ? (int)CRUDOperation.Update : (int)CRUDOperation.Create,
                    @Id = office.Id,
                    @NameEn = office.NameEn,
                    @NameSq = office.NameSq,
                    @NameSr = office.NameSr,
                    @DescriptionEn = enHTML,
                    @DescriptionSq = sqHTML,
                    @DescriptionSr = srHTML,
                    @CreatedUserId = UserId,
                    @UpdatedUserId = office.UpdatedUserId,
                    @Code = office.Code,
                    @Contact = office.Contact
                });

                if (createUpdateResult != 0)
                {
                    //Create Schedules
                    if (office.Schedules != null)
                    {
                        foreach (var schedule in office.Schedules)
                        {
                            if (schedule.Id.HasValue)
                            {
                                 scheduler.CreateSchedule(schedule, null, null, null, createUpdateResult);
                            }
                        }
                    }
                    //Create Services
                    //if (office.OrgServices != null)
                    //{
                    //    foreach (var orgService in office.OrgServices)
                    //    {
                    //        if (orgService.Id.HasValue)
                    //        {
                    //            await servicer.AddOrgService(orgService, null, null, null, createUpdateResult);
                    //        }
                    //    }
                    //}

                    HasAffected = true;
                    //var afterLogData = (await new Query().SelectSingle<Office>($"SELECT TOP 1 * FROM Office where Id = {createUpdateResult}")).Result;
                    //var serializedObject = new ChangeLogHelper().SerializeObject(beforeLogData, afterLogData, (int)ChangeLogTable.Offices, UserId, (int)ChangeLogAction.Update);
                   //var addLog = new ChangeLogHelper().AddLog(serializedObject);
                }
            }
            return Json(HasAffected);

        }

        public async Task<IActionResult> DeleteOffice([DataSourceRequest] DataSourceRequest request, Office office)
        {
            int currentUserID = new AuthorizeHelper(HttpContext).GetUserID();
            //var Schedules = (await new Query().Select<Schedule>($"Select * From Schedule where OfficeId={office.Id}")).Result.ToList();
            //var orgServices = (await new Query().Select<OrgService>($"Select * From OrgServices where OfficeId={office.Id}")).Result.ToList();
            bool HasAffected = false;
            var beforeLogData = (await new Query().SelectSingle<Office>($"SELECT TOP 1 * FROM Office where Id = {office.Id}")).Result;

            var deleteDepartmentResult = await new Query().Execute("CreateUpdateDeleteOffice @CRUDOperation,@Id", new
            {
                @CRUDOperation = (int)CRUDOperation.Delete,
                @Id = office.Id

            });
            if (deleteDepartmentResult.HasAffected)
            {
                  scheduler.DeleteSchedule(office.Id);
            //    await servicer.DeleteServicesForOrg(null, null, null, office.Id);

            //    HasAffected = true;

            //    var serializedObject = new ChangeLogHelper().SerializeObject(beforeLogData, null, (int)ChangeLogTable.Offices, currentUserID, (int)ChangeLogAction.Delete);
            //    var addLog = new ChangeLogHelper().AddLog(serializedObject);
            }

            return Json(HasAffected);
        }

    }
}
