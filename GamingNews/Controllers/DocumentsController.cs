using GamingWeb.Custom.Helpers;
using GamingWeb.Custom.Models;
using Kendo.Mvc.UI;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using System;
using GamingWeb.Custom.Models.Documents;
using Microsoft.AspNetCore.Authorization;

namespace GamingWeb.Controllers
{
    [Authorize]
    public class DocumentsController : Controller
    {
        private string rootFolder = "C:\\Users\\User\\Desktop\\GamingNews\\GamingNews\\wwwroot\\DocumentFiles";
        //new ConfigurationBuilder().AddJsonFile("appsettings.json").Build().GetSection("SharedFolders")["RootFolder"];
        private string accessFolder = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build().GetSection("SharedFolders")["AccessFolder"];

        public IActionResult Index()
        {
            //int usersAccess = new AuthorizeHelper(HttpContext).GetPrivilegeLevel((int)Views.Documents);
            //if (usersAccess == (int)RoleAccessLevels.AccessDenied)
            //    return RedirectToAction("NotAuthorized", "NotAuthorized");

            ViewBag.Documents = true;
            return View();
        }

        public async Task<IActionResult> GetDocuments([DataSourceRequest] DataSourceRequest request)
        {
            List<Document> fileInfoList = GetFileInfo(rootFolder);
            return Json(new { Data = fileInfoList, Total = fileInfoList.Count });
        }
        public async Task<IActionResult> DeleteDocument(string fileName)
        {
            bool HasAffected = false;
            string fullPath = Path.Combine(rootFolder + fileName).Replace("\\", "/");

            try
            {
                System.IO.File.Delete(fullPath);
            }
            catch (Exception e)
            {
                string errorMsg = e.Message;
            }

            if (!System.IO.File.Exists(fullPath))
            {
                HasAffected = true;
            }
            return Json(HasAffected);
        }

        static List<Document> GetFileInfo(string folderPath)
        {
            List<Document> fileInfoList = new List<Document>();

            string[] filePaths = Directory.GetFiles(folderPath);

            foreach (string filePath in filePaths)
            {
                FileInfo fileInfo = new FileInfo(filePath);

                string fileName = fileInfo.Name;
                string extension = fileInfo.Extension;
                DateTime lastModified = fileInfo.LastWriteTime;

                Document file = new Document
                {
                    Name = fileName,
                    Extension = extension,
                    LastModifiedDate = lastModified
                };

                fileInfoList.Add(file);
            }

            return fileInfoList;
        }

        public async Task<IActionResult> GetFileUrl(string fileName)
        {
            string fullPath = Path.Combine(accessFolder + fileName);
            return Json(fullPath);
        }

        [HttpPost]
        public async Task<ActionResult> SaveFile(IFormFile file)
        {
            var filepath = file.FileName;
            if (file.Length > 0)
            {
                var uploads = Path.Combine("wwwroot\\DocumentFiles\\");
                var filePath = Path.Combine(uploads, file.FileName);

                using (var stream = System.IO.File.Create(filePath))
                {
                    await file.CopyToAsync(stream);
                }
                return Json(true);
            }
            return Json(true);
        }
    }
}
