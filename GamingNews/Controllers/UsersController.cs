using GamingWeb.Custom.DatabaseHelpers;
using GamingWeb.Custom.Helpers;
using GamingWeb.Custom.Models.User;
using GamingWeb.Custom.Models;
using Kendo.Mvc.UI;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewEngines;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;

namespace GamingWeb.Controllers
{
    //[Authorize]
    public class UsersController : Controller
    {
        #region Views
        public async Task<IActionResult> TabIndex()
        {
            ViewBag.Users = true;

            var curLang = new LanguageHelper(HttpContext).Current();
            ViewBag.Roles = (await new Query().Select<KeyValue>($"Select * from GetRolesKeyValue(@Lang)", new { @Lang = curLang })).Result;
            ViewBag.AccessTypes = GetRoleAccessKeyValue();
            ViewBag.Views = (await new Query().Select<KeyValue>($"Select Id as [Key], [Name] as [Value] from [View]")).Result;  
            
            return View();
        }
        #endregion

        #region Users
        public async Task<IActionResult> GetAllUsers([DataSourceRequest] DataSourceRequest request)
        {
            var language = new LanguageHelper(HttpContext);

            var response = await new PaginationHelper<User>().GetPagginatedList(request, "[User]");
            if (response.HasError)
            {
                return this.Json(new DataSourceResult { Errors = language.Get("ErrorOccurred") });
            }

            return Json(new { Data = response.Data, Total = response.Total });
        }

        public async Task<IActionResult> CreateUpdateUser([DataSourceRequest] DataSourceRequest request, User user)
        {
            var language = new LanguageHelper(HttpContext);
            //int currentUserID = new AuthorizeHelper(HttpContext).GetUserID();
            bool IsInsert = !user.Id.HasValue;

            bool HasAffected = false;
            string hashedPassword = null, salt = null;
            if (!user.Id.HasValue)
            {
                var password = new PasswordHelper(user.Username);
                hashedPassword = password.Hash;
                salt = password.Salt;
            }
            if (!(await new Query().SelectSingle<bool>("select ISNULL((select 1 from [User] where Username = @Username AND (@Id IS NULL OR Id <> @Id), 0)", new
            {
                @Id = user.Id,
                @Username = user.Username
            })).Result)
            {
                var beforeLogData = (await new Query().SelectSingle<User>("SELECT TOP 1 * FROM [User] WHERE Id = @Id", new { @Id = user.Id })).Result;

                var userResult = await new Query().ExecuteAndGetInsId("CreateUpdateDeleteUsers @CRUDOperation,@Id,@Name,@Surname,@Username,@Password,@Salt,@RoleId,@CreatedUserId,@UpdatedUserId", new
                {
                    @CRUDOperation = IsInsert ? (int)CRUDOperation.Create : (int)CRUDOperation.Update,
                    @Id = user.Id,
                    @Name = user.Name,
                    @Surname = user.Surname,
                    @Username = user.Username,
                    @Password = hashedPassword,
                    @Salt = salt,
                    @RoleId = user.RoleId,
                    @CreatedUserId = "",
                    @UpdatedUserId = ""
                });
                if (userResult > 0 || user.Id.HasValue)
                {
                    HasAffected = true;

                    //int? selectID = IsInsert ? userResult.InsertedId : user.Id;
                    //var afterLogData = (await new Query().SelectSingle<User>("SELECT TOP 1 * FROM [User] WHERE Id = @Id", new { @Id = selectID })).Result;

                    //var serializedObject = new ChangeLogHelper().SerializeObject(beforeLogData, afterLogData, (int)ChangeLogTable.Users, currentUserID, user.Id.HasValue ? (int)ChangeLogAction.Update : (int)ChangeLogAction.Insert);
                    //var addLog = new ChangeLogHelper().AddLog(serializedObject);
                }
            }

            if (!HasAffected)
                return this.Json(new DataSourceResult { Errors = language.Get("ErrorOccurred") });

            return Json(HasAffected);
        }

        public async Task<IActionResult> DeleteUser([DataSourceRequest] DataSourceRequest request, User user)
        {
            var language = new LanguageHelper(HttpContext);
            //int currentUserID = new AuthorizeHelper(HttpContext).GetUserID();
            var beforeLogData = (await new Query().SelectSingle<User>("SELECT TOP 1 * FROM [User] WHERE Id = @Id", new { @Id = user.Id })).Result;
            bool HasAffected = false;

            var deleteResult = await new Query().Execute("CreateUpdateDeleteUsers @CRUDOperation,@Id,@Name,@Surname,@Username,@Password,@Salt,@RoleId,@CreatedUserId,@UpdatedUserId", new
            {
                @CRUDOperation = (int)CRUDOperation.Delete,
                @Id = user.Id,
                @Name = user.Name,
                @Surname = user.Surname,
                @Username = user.Username,
                @Password = user.Password,
                @Salt = user.Salt,
                @RoleId = user.RoleId,
                @CreatedUserId = user.CreatedUserId,
                @UpdatedUserId = user.UpdatedUserId
            });
            if (deleteResult.HasAffected)
            {
                HasAffected = true;

                //var serializedObject = new ChangeLogHelper().SerializeObject(beforeLogData, null, (int)ChangeLogTable.Users, currentUserID, (int)ChangeLogAction.Delete);
                //var addLog = new ChangeLogHelper().AddLog(serializedObject);
            }
            if (!HasAffected)
                return this.Json(new DataSourceResult { Errors = language.Get("ErrorOccurred") });

            return Json(HasAffected);
        }

        public async Task<IActionResult> ChangePassword(int userID)
        {
            var user = (await new Query().SelectSingle<User>($"SELECT TOP 1 (Id) from [User] where Id = {userID}"));
            if (user.HasData)
            {
                return View(user.Result);
            }
            else
                return RedirectToAction("Index", "Home");
        }

        [HttpPost]
        public async Task<IActionResult> ChangePassword(User user)
        {
            if (user.Id > 0)
            {
                var password = new PasswordHelper(user.Password);
                var changePSW = (await new Query().Execute($"UPDATE [User] SET [Password] = '{password.Hash}', [Salt] = '{password.Salt}' WHERE Id = {user.Id}"));
                if (!changePSW.HasError)
                    return Json(new { success = "true" });
            }
            return Json(new { success = "notSame" });
        }
        #endregion

        #region Role
        public async Task<IActionResult> GetAllRoles([DataSourceRequest] DataSourceRequest request)
        {
            var language = new LanguageHelper(HttpContext);

            var response = await new PaginationHelper<Custom.Models.User.Role>().GetPagginatedList(request, "[Role]");
            if (response.HasError)
            {
                return this.Json(new DataSourceResult { Errors = language.Get("ErrorOccurred") });
            }

            return Json(new { Data = response.Data, Total = response.Total });
        }

        public async Task<IActionResult> GetRolesKeyValue()
        {
            var curLang = new LanguageHelper(HttpContext).Current();
            var roles = (await new Query().Select<KeyValue>($"Select * from GetRolesKeyValue(@Lang)", new { @Lang = curLang })).Result;

            return Json(roles);
        }

        public async Task<IActionResult> CreateUpdateRole([DataSourceRequest] DataSourceRequest request, Custom.Models.User.Role role)
        {
            int currentUserID = new AuthorizeHelper(HttpContext).GetUserID();

            bool HasAffected = false;
            //var beforeLogData = (await new Query().SelectSingle<Custom.Models.User.Role>($"SELECT TOP 1 * FROM [Role] WHERE Id = @ID", new { @ID = role.Id })).Result;

            //UPDATE
            if (role.Id.HasValue)
            {
                var roleResult = await new Query().Execute("CreateUpdateDeleteRoles @CRUDOperation,@Id,@NameEN,@NameSQ,@NameSR,@CreatedUserId,@UpdatedUserId", new
                {
                    @CRUDOperation = (int)CRUDOperation.Update,
                    @Id = role.Id,
                    @NameEN = role.NameEn,
                    @NameSQ = role.NameSq,
                    @NameSR = role.NameSr,
                    @CreatedUserId = currentUserID,
                    @UpdatedUserId = currentUserID
                });
                if (roleResult.HasAffected)
                {
                    HasAffected = true;
                    //var afterLogData = (await new Query().SelectSingle<Custom.Models.User.Role>($"SELECT TOP 1 * FROM [Role] WHERE Id = @ID", new { @ID = role.Id })).Result;

                    //var serializedObject = new ChangeLogHelper().SerializeObject(beforeLogData, afterLogData, (int)ChangeLogTable.Role, currentUserID, (int)ChangeLogAction.Update);
                    //var addLog = new ChangeLogHelper().AddLog(serializedObject);
                }
            }
            //INSERT
            else
            {
                var roleResult = await new Query().ExecuteAndGetInsId("CreateUpdateDeleteRoles @CRUDOperation,@Id,@NameEN,@NameSQ,@NameSR,@CreatedUserId,@UpdatedUserId", new
                {
                    @CRUDOperation = (int)CRUDOperation.Create,
                    @Id = role.Id,
                    @NameEN = role.NameEn,
                    @NameSQ = role.NameSq,
                    @NameSR = role.NameSr,
                    @CreatedUserId = currentUserID,
                    @UpdatedUserId = currentUserID
                });
                if (roleResult > 0)
                {
                    HasAffected = true;
                    //var afterLogData = (await new Query().SelectSingle<Custom.Models.User.Role>($"SELECT TOP 1 * FROM [Role] WHERE Id = @ID", new { @ID = roleResult })).Result;

                    //var serializedObject = new ChangeLogHelper().SerializeObject(null, afterLogData, (int)ChangeLogTable.Role, currentUserID, (int)ChangeLogAction.Insert);
                    //var addLog = new ChangeLogHelper().AddLog(serializedObject);

                    if (!role.Id.HasValue)
                    {
                        var views = (await new Query().Select<int>("SELECT Id from [View]")).Result;
                        foreach (int view in views)
                        {
                            var roleAccessResult = await new Query().Execute("CreateUpdateDeleteRoleAccess @CRUDOperation,@Id,@RoleId,@AccessTypeId,@ViewId,@CreatedUserId,@UpdatedUserId", new
                            {
                                @CRUDOperation = (int)CRUDOperation.Create,
                                @Id = role.Id,
                                @RoleId = roleResult,
                                @AccessTypeId = (int)RoleAccessLevels.AccessDenied,
                                @ViewId = view,
                                @CreatedUserId = currentUserID,
                                @UpdatedUserId = currentUserID
                            });
                            if (roleAccessResult.HasError)
                                HasAffected = false;
                        }
                    }
                }
            }
            return Json(HasAffected);
        }

        public async Task<IActionResult> DeleteRole([DataSourceRequest] DataSourceRequest request, Custom.Models.User.Role role)
        {
            int currentUserID = new AuthorizeHelper(HttpContext).GetUserID();

            bool HasAffected = false;
            //var beforeLogData = (await new Query().SelectSingle<Custom.Models.User.Role>($"SELECT TOP 1 * FROM [Role] WHERE Id = @ID", new { @ID = role.Id })).Result;

            var deleteRoleResult = await new Query().Execute("CreateUpdateDeleteRoles @CRUDOperation,@Id,@NameEN,@NameSQ,@NameSR,@CreatedUserId,@UpdatedUserId", new
            {
                @CRUDOperation = (int)CRUDOperation.Delete,
                @Id = role.Id,
                @NameEN = role.NameEn,
                @NameSQ = role.NameSq,
                @NameSR = role.NameSr,
                @CreatedUserId = currentUserID,
                @UpdatedUserId = currentUserID
            });
            if (deleteRoleResult.HasAffected)
            {
                HasAffected = true;

                //var serializedObject = new ChangeLogHelper().SerializeObject(beforeLogData, null, (int)ChangeLogTable.Role, currentUserID, (int)ChangeLogAction.Delete);
                //var addLog = new ChangeLogHelper().AddLog(serializedObject);
            }

            return Json(HasAffected);
        }
        #endregion

        #region RoleAccess
        public async Task<IActionResult> GetAllRoleAccess([DataSourceRequest] DataSourceRequest request, int roleId)
        {
            var query = $"Select * from [RoleAccess] ra ";
            var countQuery = $"Select COUNT(*) from [RoleAccess] ";

            string filters = "";
            foreach (var filter in request.Filters)
            {
                filters += KendoGridHelper.ApplyFilter<RoleAccess>(filter);
            }
            string sort = KendoGridHelper.ApplySort<RoleAccess>(request.Sorts, "ViewId ASC");
            if (filters.Trim().Length > 0)
                filters = $" WHERE RoleId = {roleId} " + filters;
            else
                filters = $" WHERE RoleId = {roleId} ";
            if (sort.Trim().Length > 0)
                sort = " ORDER BY " + sort;

            var total = await new Query().SelectSingle<int>(countQuery + filters);
            var regionsResult = await new Query().Select<RoleAccess>(query + filters + sort);
            if (regionsResult.HasError || total.HasError)
            {
                return this.Json(new DataSourceResult
                {
                    Errors = "Error Occurred"
                });
            }
            return Json(new { Data = regionsResult.Result, Total = total.Result });
        }

        public async Task<IActionResult> UpdateRoleAccess([DataSourceRequest] DataSourceRequest request, Custom.Models.User.RoleAccess roleAccess, int roleId)
        {
            int currentUserID = new AuthorizeHelper(HttpContext).GetUserID();

            //var beforeLogData = (await new Query().SelectSingle<RoleAccess>($"SELECT TOP 1 * FROM [RoleAccess] WHERE Id = @ID", new { @ID = roleAccess.Id })).Result;
            bool HasAffected = false;

            var roleResult = await new Query().Execute("CreateUpdateDeleteRoleAccess @CRUDOperation,@Id,@RoleId,@AccessTypeId,@ViewId,@CreatedUserId,@UpdatedUserId", new
            {
                @CRUDOperation = (int)CRUDOperation.Update,
                @Id = roleAccess.Id,
                @RoleId = roleId,
                @AccessTypeId = roleAccess.AccessTypeId,
                @ViewId = roleAccess.ViewId,
                @CreatedUserId = currentUserID,
                @UpdatedUserId = currentUserID
            });
            if (roleResult.HasAffected)
            {
                HasAffected = true;
                //var afterLogData = (await new Query().SelectSingle<RoleAccess>($"SELECT TOP 1 * FROM [RoleAccess] WHERE Id = @ID", new { @ID = roleAccess.Id })).Result;

                //var serializedObject = new ChangeLogHelper().SerializeObject(beforeLogData, afterLogData, (int)ChangeLogTable.RoleAccess, currentUserID, (int)ChangeLogAction.Update);
                //var addLog = new ChangeLogHelper().AddLog(serializedObject);
            }

            return Json(HasAffected);
        }
        public IEnumerable<KeyValue> GetRoleAccessKeyValue()
        {
            var language = new LanguageHelper(HttpContext);
            var list = new List<KeyValue>();
            list.Add(new KeyValue { Key = 0, Value = language.Get("AccessDenied") });
            list.Add(new KeyValue { Key = 1, Value = language.Get("ViewAccess") });
            list.Add(new KeyValue { Key = 2, Value = language.Get("FullAccess") });

            return list.AsEnumerable();
        }
        #endregion
    }
}