using GamingWeb.Custom.DatabaseHelpers;
using GamingWeb.Custom.Helpers;
using GamingWeb.Custom.Models.Services;
using GamingWeb.Custom.Models;
using Kendo.Mvc.UI;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using GamingWeb.Custom.DatabaseHelpers;
using GamingWeb.Custom.Helpers;
using GamingWeb.Custom.Models;
using System;
using System.IO;
using System.Threading.Tasks;
using GamingWeb.Custom.Models.Department;
using GamingWeb.Custom.Models.User;
using Microsoft.AspNetCore.Mvc.Rendering;
using GamingWeb.Custom.Models.Announcements;

namespace GamingWeb.Controllers
{
    public class ServicesController : Controller
    {
        public async Task<IActionResult> Index()
        {
            //int usersAccess = new AuthorizeHelper(HttpContext).GetPrivilegeLevel((int)Views.Services);
            //if (usersAccess == (int)RoleAccessLevels.AccessDenied)
            //return RedirectToAction("NotAuthorized", "NotAuthorized");

            var language = new LanguageHelper(HttpContext);
            ViewBag.SubMenu = "Organisation";
            ViewBag.Services = true;

            return View();
        }

        public async Task<IActionResult> CustomsServices()
        {
            //int usersAccess = new AuthorizeHelper(HttpContext).GetPrivilegeLevel((int)Views.Services);
            //if (usersAccess == (int)RoleAccessLevels.AccessDenied)
            //return RedirectToAction("NotAuthorized", "NotAuthorized");

            var language = new LanguageHelper(HttpContext);
            ViewBag.CustomsServices = true;

            return View();
        }

        #region Services
        [HttpPost]
        public async Task<IActionResult> Read_Services([DataSourceRequest] DataSourceRequest request)
        {
            //Get paginated list from helper
            var response = await new PaginationHelper<Service>().GetPagginatedList(request, "Service");

            return Json(new { Data = response.Data, Total = response.Total });
        }
        public async Task<ActionResult> CreateUpdateService([DataSourceRequest] DataSourceRequest request, Service service)
        {
            var UserId = new AuthorizeHelper(HttpContext).GetUserID();
            bool HasAffected = false;

            var beforeLogData = (await new Query().SelectSingle<Service>($"SELECT * FROM Service where Id = {service.Id}")).Result;

            if (service.Id.HasValue)
            {
                var createUpdateResult = await new Query().Execute("CreateUpdateDeleteService @CRUDOperation,@Id,@NameSq,@NameEn,@NameSr,@IconPath,@Url,@CreatedUserId,@UpdatedUserId", new
                {
                    @CRUDOperation = (int)CRUDOperation.Update,
                    @Id = service.Id,
                    @NameSq = service.NameSq,
                    @NameEn = service.NameEn,
                    @NameSr = service.NameSr,
                    @IconPath = service.IconPath,
                    @Url = service.Url,
                    @CreatedUserId = service.CreatedUserId,
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
                var createUpdateResult = await new Query().ExecuteAndGetInsId("CreateUpdateDeleteService @CRUDOperation,@Id,@NameSq,@NameEn,@NameSr,@IconPath,@Url,@CreatedUserId,@UpdatedUserId", new
                {
                    @CRUDOperation = (int)CRUDOperation.Create,
                    @Id = service.Id,
                    @NameSq = service.NameSq,
                    @NameEn = service.NameEn,
                    @NameSr = service.NameSr,
                    @IconPath = service.IconPath,
                    @Url = service.Url,
                    @CreatedUserId = UserId,
                    @UpdatedUserId = service.UpdatedUserId
                });

                if (createUpdateResult != 0)
                {
                    HasAffected = true;
                    //var afterLogData = (await new Query().SelectSingle<Service>($"SELECT * FROM Services where Id = {createUpdateResult}")).Result;
                    //var serializedObject = new ChangeLogHelper().SerializeObject(beforeLogData, afterLogData, (int)ChangeLogTable.Categories, UserId, (int)ChangeLogAction.Insert);
                    //var addLog = new ChangeLogHelper().AddLog(serializedObject);
                }
            }


            return Json(HasAffected);

        }

        public async Task<IActionResult> DeleteService([DataSourceRequest] DataSourceRequest request, Service service)
        {
            bool HasAffected = false;
            //var beforeLogData = (await new Query().SelectSingle<Custom.Models.User.Role>($"SELECT TOP 1 * FROM [Role] WHERE Id = @ID", new { @ID = role.Id })).Result;

            var deleteDepartmentResult = await new Query().Execute("CreateUpdateDeleteService @CRUDOperation,@Id", new
            {
                @CRUDOperation = (int)CRUDOperation.Delete,
                @Id = service.Id

            });
            if (deleteDepartmentResult.HasAffected)
            {
                HasAffected = true;
            }

            return Json(HasAffected);
        }

        [HttpPost]
        public async Task<ActionResult> SaveFile([Bind(Prefix = "IconPath.file")] IFormFile file)
        {
            var filepath = file.FileName;
            if (file.Length > 0)
            {
                var uploads = Path.Combine("wwwroot\\ServiceIcon\\");
                string guid = Guid.NewGuid().ToString();
                var filename = DateTime.Now.Year + "_" + guid + Path.GetExtension(file.FileName);
                var filePath = Path.Combine(uploads, filename);

                using (var stream = System.IO.File.Create(filePath))
                {
                    await file.CopyToAsync(stream);
                    filePath = "/ServiceIcon/" + filename;
                }
                return Json(new { FileUrl = filePath });
            }
            return Json(new { FileUrl = "" });
        }
        #endregion

        #region OrgServices
        public async Task<IActionResult> Read_OrgServices([DataSourceRequest] DataSourceRequest request)
        {
            var userId = new AuthorizeHelper(HttpContext).GetUserID();
            var skip = (request.Page - 1) * request.PageSize;
            var query = "select * from OrgServices WHERE DirectoryId IS NULL AND DepartmentId IS NULL AND SectorId IS NULL AND OfficeId IS NULL";
            var countQuery = $"select count(*) from OrgServices WHERE DirectoryId IS NULL AND DepartmentId IS NULL AND SectorId IS NULL AND OfficeId IS NULL";

            string filters = "";
            foreach (var filter in request.Filters)
            {
                filters += KendoGridHelper.ApplyFilter<OrgService>(filter);
            }
            string sort = KendoGridHelper.ApplySort<OrgService>(request.Sorts, "ID desc");
            string pagination = " OFFSET " + skip + " ROWS FETCH NEXT " + request.PageSize + " ROWS ONLY";
            if (filters.Trim().Length > 0)
                filters = " WHERE " + filters;
            if (sort.Trim().Length > 0)
                sort = " ORDER BY " + sort;
            var total = await new Query().SelectSingle<int>(countQuery + filters);
            var result = await new Query().Select<OrgService>(query + filters + sort + pagination);

            return Json(new { Data = result.Result, Total = total.Result });
        }

        public async Task<IActionResult> Read_DepOrgServices([DataSourceRequest] DataSourceRequest request, int? dirId, int? depId, int? secId, int? offId)
        {
            var userId = new AuthorizeHelper(HttpContext).GetUserID();
            var skip = (request.Page - 1) * request.PageSize;
            var query = "";
            var countQuery = "";
            if (dirId.HasValue)
            {
                query = $"select * from OrgServices WHERE DirectoryId={dirId}";
                countQuery = $"select count(*) from OrgServices WHERE DirectoryId={dirId}";
            }
            else if (depId.HasValue)
            {
                query = $"select * from OrgServices WHERE DepartmentId={depId}";
                countQuery = $"select count(*) from OrgServices WHERE DepartmentId={depId}";
            }
            else if (secId.HasValue)
            {
                query = $"select * from OrgServices WHERE SectorId={secId}";
                countQuery = $"select count(*) from OrgServices WHERE SectorId={secId}";
            }
            else if (offId.HasValue)
            {
                query = $"select * from OrgServices WHERE OfficeId={offId}";
                countQuery = $"select count(*) from OrgServices WHERE OfficeId={offId}";
            }
            string filters = "";
            foreach (var filter in request.Filters)
            {
                filters += KendoGridHelper.ApplyFilter<OrgService>(filter);
            }
            string sort = KendoGridHelper.ApplySort<OrgService>(request.Sorts, "ID desc");
            string pagination = " OFFSET " + skip + " ROWS FETCH NEXT " + request.PageSize + " ROWS ONLY";
            if (filters.Trim().Length > 0)
                filters = " WHERE " + filters;
            if (sort.Trim().Length > 0)
                sort = " ORDER BY " + sort;
            var total = await new Query().SelectSingle<int>(countQuery + filters);
            var result = await new Query().Select<OrgService>(query + filters + sort + pagination);

            return Json(new { Data = result.Result, Total = total.Result });
        }

        public async Task<IActionResult> CreateUpdateOrgService([DataSourceRequest] DataSourceRequest request, OrgService orgservice, int? dirId, int? depId, int? secId, int? offId, int? Service)
        {
            bool HasAffected = false;
            if (orgservice.Id.HasValue)
            {
                var createUpdateResult = await new Query().Execute("CreateUpdateDeleteOrgService @CRUDOperation,@Id,@ServiceId,@DirectoryId,@DepartmentId,@SectorId,@OfficeId", new
                {
                    @CRUDOperation = (int)CRUDOperation.Update,
                    @Id = orgservice.Id,
                    @ServiceId = orgservice.ServiceId,
                    @DirectoryId = dirId,
                    @DepartmentId = depId,
                    @SectorId = secId,
                    @OfficeId = offId
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
                var createUpdateResult = await new Query().ExecuteAndGetInsId("CreateUpdateDeleteOrgService @CRUDOperation,@Id,@ServiceId,@DirectoryId,@DepartmentId,@SectorId,@OfficeId", new
                {
                    @CRUDOperation = (int)CRUDOperation.Create,
                    @Id = orgservice.Id,
                    @ServiceId = Service,
                    @DirectoryId = dirId,
                    @DepartmentId = depId,
                    @SectorId = secId,
                    @OfficeId = offId
                });
                if (createUpdateResult != 0)
                {
                    HasAffected = true;
                    //var afterLogData = (await new Query().SelectSingle<Service>($"SELECT * FROM Services where Id = {createUpdateResult}")).Result;
                    //var serializedObject = new ChangeLogHelper().SerializeObject(beforeLogData, afterLogData, (int)ChangeLogTable.Categories, UserId, (int)ChangeLogAction.Insert);
                    //var addLog = new ChangeLogHelper().AddLog(serializedObject);
                }
            }
            return Json(HasAffected);
        }

        public async Task<IActionResult> DeleteOrgService([DataSourceRequest] DataSourceRequest request, OrgService orgService)
        {
            bool HasAffected = false;
            //var beforeLogData = (await new Query().SelectSingle<Custom.Models.User.Role>($"SELECT TOP 1 * FROM [Role] WHERE Id = @ID", new { @ID = role.Id })).Result;

            var deleteDepartmentResult = await new Query().Execute("CreateUpdateDeleteOrgService @CRUDOperation,@Id", new
            {
                @CRUDOperation = (int)CRUDOperation.Delete,
                @Id = orgService.Id

            });
            if (deleteDepartmentResult.HasAffected)
            {
                HasAffected = true;
            }

            return Json(HasAffected);
        }
        #endregion

        #region CustomsServices

        public async Task<IActionResult> AddCSWindow()
        {
            return View();
        }

        public async Task<IActionResult> EditCSWindow(int cserviceId)
        {
            var cservice = (await new Query().SelectSingle<CustomsService>($"Select * FROM CustomsService where Id={cserviceId}")).Result;

            return View(cservice);
        }

        [HttpPost]
        public async Task<IActionResult> Read_CustomsServices([DataSourceRequest] DataSourceRequest request)
        {
            //Get paginated list from helper
            var response = await new PaginationHelper<Service>().GetPagginatedList(request, "CustomsService");

            return Json(new { Data = response.Data, Total = response.Total });
        }
        public async Task<ActionResult> CreateUpdateCustomsService([DataSourceRequest] DataSourceRequest request, CustomsService cservice)
        {
            var UserId = new AuthorizeHelper(HttpContext).GetUserID();
            bool HasAffected = false;

            var beforeLogData = (await new Query().SelectSingle<Service>($"SELECT * FROM Service where Id = {cservice.Id}")).Result;

            if (cservice.Id.HasValue)
            {
                var createUpdateResult = await new Query().Execute("CreateUpdateDeleteCustomsService @CRUDOperation,@Id,@NameSq,@NameEn,@NameSr,@DescriptionSq,@DescriptionEn,@DescriptionSr,@IconPath,@CreatedUserId,@UpdatedUserId", new
                {
                    @CRUDOperation = (int)CRUDOperation.Update,
                    @Id = cservice.Id,
                    @NameSq = cservice.NameSq,
                    @NameEn = cservice.NameEn,
                    @NameSr = cservice.NameSr,
                    @DescriptionSq = cservice.DescriptionSq,
                    @DescriptionEn = cservice.DescriptionEn,
                    @DescriptionSr = cservice.DescriptionSr,
                    @IconPath = cservice.IconPath,
                    @CreatedUserId = cservice.CreatedUserId,
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
                var createUpdateResult = await new Query().ExecuteAndGetInsId("CreateUpdateDeleteCustomsService @CRUDOperation,@Id,@NameSq,@NameEn,@NameSr,@DescriptionSq,@DescriptionEn,@DescriptionSr,@IconPath,@CreatedUserId,@UpdatedUserId", new
                {
                    @CRUDOperation = (int)CRUDOperation.Create,
                    @Id = cservice.Id,
                    @NameSq = cservice.NameSq,
                    @NameEn = cservice.NameEn,
                    @NameSr = cservice.NameSr,
                    @DescriptionSq = cservice.DescriptionSq,
                    @DescriptionEn = cservice.DescriptionEn,
                    @DescriptionSr = cservice.DescriptionSr,
                    @IconPath = cservice.IconPath,
                    @CreatedUserId = UserId,
                    @UpdatedUserId = cservice.UpdatedUserId
                });

                if (createUpdateResult != 0)
                {
                    HasAffected = true;
                    //var afterLogData = (await new Query().SelectSingle<Service>($"SELECT * FROM Services where Id = {createUpdateResult}")).Result;
                    //var serializedObject = new ChangeLogHelper().SerializeObject(beforeLogData, afterLogData, (int)ChangeLogTable.Categories, UserId, (int)ChangeLogAction.Insert);
                    //var addLog = new ChangeLogHelper().AddLog(serializedObject);
                }
            }


            return RedirectToAction("CustomsServices");

        }

        public async Task<IActionResult> DeleteCustomsService([DataSourceRequest] DataSourceRequest request, CustomsService cservice)
        {
            bool HasAffected = false;
            //var beforeLogData = (await new Query().SelectSingle<Custom.Models.User.Role>($"SELECT TOP 1 * FROM [Role] WHERE Id = @ID", new { @ID = role.Id })).Result;

            var deleteDepartmentResult = await new Query().Execute("CreateUpdateDeleteCustomsService @CRUDOperation,@Id", new
            {
                @CRUDOperation = (int)CRUDOperation.Delete,
                @Id = cservice.Id

            });
            if (deleteDepartmentResult.HasAffected)
            {
                HasAffected = true;
            }

            return Json(HasAffected);
        }

        [HttpPost]
        public async Task<ActionResult> SaveCSFile([Bind(Prefix = "IconPath")] IFormFile file)
        {
            var filepath = file.FileName;
            if (file.Length > 0)
            {
                var uploads = Path.Combine("wwwroot\\CServiceIcon\\");
                string guid = Guid.NewGuid().ToString();
                var filename = DateTime.Now.Year + "_" + guid + Path.GetExtension(file.FileName);
                var filePath = Path.Combine(uploads, filename);

                using (var stream = System.IO.File.Create(filePath))
                {
                    await file.CopyToAsync(stream);
                    filePath = "/CServiceIcon/" + filename;
                }
                return Json(new { FileUrl = filePath });
            }
            return Json(new { FileUrl = "" });
        }

        #endregion

    }

}
