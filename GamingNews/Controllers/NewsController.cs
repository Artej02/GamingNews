using GamingWeb.Custom.DatabaseHelpers;
using GamingWeb.Custom.Helpers;
using GamingWeb.Custom.Models;
using GamingWeb.Custom.Models.News;
using Kendo.Mvc.UI;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewEngines;
using System.IO;
using System;
using System.Threading.Tasks;
using System.Web;
using ImageProcessor;
using ImageProcessor.Plugins.WebP.Imaging.Formats;
using Microsoft.AspNetCore.Hosting;

namespace GamingWeb.Controllers
{
    public class NewsController : Controller
    {
        private readonly IWebHostEnvironment _webHostEnvironment;
        public NewsController(IWebHostEnvironment webHostEnvironment)
        {
            _webHostEnvironment = webHostEnvironment;
        }
        public string GetWebRootPath()
        {
            return _webHostEnvironment.WebRootPath;
        }

        public IActionResult Index()
        {
            ViewBag.News = true;
            return View();
        }

        public async Task<IActionResult> AddArticle()
        {
            //int usersAccess = new AuthorizeHelper(HttpContext).GetPrivilegeLevel((int)Views.News);
            //if (usersAccess != (int)RoleAccessLevels.FullAccess)
                //return RedirectToAction("NotAuthorized", "NotAuthorized");

            //ViewBag.Category = (await new Query().Select<KeyValue>("select Id as [Key], CategoryName as [Value] from [Categories]")).Result;
            ViewBag.Department = (await new Query().Select<KeyValue>("select Id as [Key], DepartmentName as [Value] from [Departments]")).Result;

            var newArticle = new News();

            return View(newArticle);
        }

        public async Task<IActionResult> EditArticle(int articleId)
        {
            //int usersAccess = new AuthorizeHelper(HttpContext).GetPrivilegeLevel((int)Views.News);
            //if (usersAccess != (int)RoleAccessLevels.FullAccess)
                //return RedirectToAction("NotAuthorized", "NotAuthorized");

            //ViewBag.Category = (await new Query().Select<KeyValue>("select Id as [Key], CategoryName as [Value] from [Categories]")).Result;
            ViewBag.Department = (await new Query().Select<KeyValue>("select Id as [Key], DepartmentName as [Value] from [Departments]")).Result;

            var newArticle = (await new Query().SelectSingle<News>("SELECT TOP 1 * FROM News WHERE Id = @ID", new { @ID = articleId })).Result;

            return View(newArticle);
        }

        public async Task<ActionResult> Read_News([DataSourceRequest] DataSourceRequest request)
        {
            var language = new LanguageHelper(HttpContext);

            var response = await new PaginationHelper<News>().GetPagginatedList(request, "[News]");
            if (response.HasError)
            {
                return this.Json(new DataSourceResult { Errors = language.Get("ErrorOccurred") });
            }

            return Json(new { Data = response.Data, Total = response.Total });
        }

        [HttpPost]
        public async Task<IActionResult> AddArticle(News news)
        {
            string sqHTML = HttpUtility.HtmlDecode(news.ContentSq == null ? "" : news.ContentSq.Replace("background-color:#ffffff;", ""));
            string enHTML = HttpUtility.HtmlDecode(news.ContentEn == null ? "" : news.ContentEn.Replace("background-color:#ffffff;", ""));
            string srHTML = HttpUtility.HtmlDecode(news.ContentSr == null ? "" : news.ContentSr.Replace("background-color:#ffffff;", ""));

            int UserID = new AuthorizeHelper(HttpContext).GetUserID();
            //var beforeLogData = (await new Query().SelectSingle<News>($"SELECT * FROM News where Id = {news.Id}")).Result;
            bool HasAffected = false;

            int? afterID = news.Id;

            var createUpdateResult = await new Query().ExecuteAndGetInsId("CreateUpdateDeleteNews @CRUDOperation,@Id,@TitleSq,@TitleEn,@TitleSr,@ContentSq,@ContentEn,@ContentSr,@NewsDate,@ReleaseDate,@Expire," +
                "@ExpireDate,@DepartmentId,@Photo,@Place,@FileUrl,@CreatedUserId,@UpdatedUserId", new
                {
                    CRUDOperation = news.Id.HasValue ? (int)CRUDOperation.Update : (int)CRUDOperation.Create,
                    Id = news.Id,
                    TitleSq = news.TitleSq,
                    TitleEn = news.TitleEn,
                    TitleSr = news.TitleSr,
                    ContentSq = sqHTML,
                    ContentEn = enHTML,
                    ContentSr = srHTML,
                    NewsDate = news.NewsDate,
                    ReleaseDate = news.ReleaseDate,
                    Expire = news.Expire,
                    ExpireDate = news.ExpireDate,
                    DepartmentId = news.DepartmentId,
                    Photo = news.Photo,
                    Place = news.Place,
                    FileUrl = news.FileUrl,
                    CreatedUserId = UserID,
                    UpdatedUserId = UserID
                });
            if (createUpdateResult > 0)
            {
                afterID = createUpdateResult;
                news.ContentEn = enHTML;
                news.ContentSq = sqHTML;
                news.ContentSr = srHTML;

                HasAffected = true;
            }

            //if (HasAffected)
            //{
                //var afterLogData = (await new Query().SelectSingle<News>($"SELECT * FROM News where Id = {afterID}")).Result;
                //var serializedObject = new ChangeLogHelper().SerializeObject(beforeLogData, afterLogData, (int)ChangeLogTable.News, UserID, news.ID.HasValue ? (int)CRUDOperation.Update : (int)CRUDOperation.Create);
                //var addLog = new ChangeLogHelper().AddLog(serializedObject);
            //}
            return RedirectToAction("Index", "News");
        }


        [HttpPost]
        public async Task<IActionResult> EditArticle(News news)
        {
            string sqHTML = HttpUtility.HtmlDecode(news.ContentSq == null ? "" : news.ContentSq.Replace("background-color:#ffffff;", ""));
            string enHTML = HttpUtility.HtmlDecode(news.ContentEn == null ? "" : news.ContentEn.Replace("background-color:#ffffff;", ""));
            string srHTML = HttpUtility.HtmlDecode(news.ContentSr == null ? "" : news.ContentSr.Replace("background-color:#ffffff;", ""));

            int UserID = new AuthorizeHelper(HttpContext).GetUserID();
            //var beforeLogData = (await new Query().SelectSingle<News>($"SELECT * FROM News where Id = {news.Id}")).Result;
            bool HasAffected = false;

            int? afterID = news.Id;

            var createUpdateResult = await new Query().Execute("CreateUpdateDeleteNews @CRUDOperation,@Id,@TitleSq,@TitleEn,@TitleSr,@ContentSq,@ContentEn,@ContentSr,@NewsDate,@ReleaseDate,@Expire," +
                "@ExpireDate,@DepartmentId,@Photo,@Place,@FileUrl,@CreatedUserId,@UpdatedUserId", new
                {
                    CRUDOperation = news.Id.HasValue ? (int)CRUDOperation.Update : (int)CRUDOperation.Create,
                    ID = news.Id,
                    TitleSq = news.TitleSq,
                    TitleEn = news.TitleEn,
                    TitleSr = news.TitleSr,
                    ContentSq = sqHTML,
                    ContentEn = enHTML,
                    ContentSr = srHTML,
                    NewsDate = news.NewsDate,
                    ReleaseDate = news.ReleaseDate,
                    Expire = news.Expire,
                    ExpireDate = news.ExpireDate,
                    DepartmentId = news.DepartmentId,
                    Photo = news.Photo,
                    Place = news.Place,
                    FileUrl = news.FileUrl,
                    CreatedUserId = UserID,
                    UpdatedUserId = UserID
                });
            if (createUpdateResult.HasAffected)
            {
                news.ContentEn = enHTML;
                news.ContentSq = sqHTML;
                news.ContentSr = srHTML;

                HasAffected = true;
            }

            //if (HasAffected)
            //{
                //var afterLogData = (await new Query().SelectSingle<News>($"SELECT * FROM News where Id = {afterID}")).Result;
                //var serializedObject = new ChangeLogHelper().SerializeObject(beforeLogData, afterLogData, (int)ChangeLogTable.News, UserID, news.ID.HasValue ? (int)CRUDOperation.Update : (int)CRUDOperation.Create);
                //var addLog = new ChangeLogHelper().AddLog(serializedObject);
            //}
            return RedirectToAction("Index", "News");
        }

        public async Task<IActionResult> Delete([DataSourceRequest] DataSourceRequest request, int? Id)
        {
            //int usersAccess = new AuthorizeHelper(HttpContext).GetPrivilegeLevel((int)Views.News);
            //if (usersAccess == (int)RoleAccessLevels.AccessDenied)
                //return RedirectToAction("NotAuthorized", "NotAuthorized");

            var HasAffected = true;
            var beforeLogData = (await new Query().SelectSingle<News>($"SELECT * FROM News where Id = {Id}")).Result;
            var news = beforeLogData;

            var createUpdateResult = await new Query().Execute("CreateUpdateDeleteNews @CRUDOperation,@Id", new
                {
                    CRUDOperation = (int)CRUDOperation.Delete,
                    ID = news.Id,
                });
            if (createUpdateResult == null)
            {
                return this.Json(new DataSourceResult
                {
                    Errors = "Error Occurred"
                });

            }

            var UserId = new AuthorizeHelper(HttpContext).GetUserID();
            //var afterLogData = (await new Query().SelectSingle<News>($"SELECT * FROM News where Id = {createUpdateResult}")).Result;
            //var serializedObject = new ChangeLogHelper().SerializeObject(beforeLogData, afterLogData, (int)ChangeLogTable.News, UserId, (int)CRUDOperation.Delete);
            //var addLog = new ChangeLogHelper().AddLog(serializedObject);

            return RedirectToAction("Index");
        }


        [HttpPost]
        public async Task<ActionResult> SaveFile([Bind(Prefix = "FileUrl")] IFormFile file)
        {
            var filepath = file.FileName;
            if (file.Length > 0)
            {
                var uploads = Path.Combine("wwwroot\\ArticleImages\\");
                string guid = Guid.NewGuid().ToString();
                var filename = DateTime.Now.Year + "_" + guid + Path.GetExtension(file.FileName);
                var filePath = Path.Combine(uploads, filename);

                using (var stream = System.IO.File.Create(filePath))
                {
                    await file.CopyToAsync(stream);
                }

                string fileExtension = Path.GetExtension(file.FileName);
                using (var webPFileStream = new FileStream(filePath.Replace(fileExtension, ".webp"), FileMode.Create))
                {
                    using (ImageFactory imageFactory = new ImageFactory(preserveExifData: false))
                    {
                        try
                        {
                            imageFactory.Load(file.OpenReadStream())
                                        .Format(new WebPFormat())
                                        .Quality(60)
                                        .Save(webPFileStream);
                        }
                        catch (Exception e)
                        {
                            string error = e.Message;
                            StreamWriter sw = new StreamWriter("C://logs//dogana.txt");
                            sw.Write(e.ToString());
                            sw.Close();
                        }
                    }
                }

                filePath = "/ArticleImages/" + filename;

                return Json(new { FileUrl = filePath });
            }
            return Json(new { FileUrl = "" });
        }

    }
}
