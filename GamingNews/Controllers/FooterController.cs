using GamingWeb.Custom.DatabaseHelpers;
using GamingWeb.Custom.Helpers;
using GamingWeb.Custom.Models;
using GamingWeb.Custom.Models.Footer;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace GamingWeb.Controllers
{
    public class FooterController : Controller
    {

        public async Task<IActionResult> Index(Footer footer)
        {
            ViewBag.Footer = true;
            if (footer.Id == null)
            {
                footer = (await new Query().SelectSingle<Footer>("Select * from Footer")).Result;
            }
            return View(footer);
        }

        [HttpPost]
        public async Task<IActionResult> UpdateFooter(Footer footer)
        {
            var UserId = new AuthorizeHelper(HttpContext).GetUserID();
            //var beforeLogData = (await new Query().SelectSingle<Footer>($"SELECT TOP 1 * FROM Footer where Id = {footer.Id}")).Result;
            bool HasAffected = false;

            if (ModelState.IsValid)
            {
                var createUpdateResult = await new Query().Execute("UpdateFooter @CRUDOperation,@Id,@FirstTitleSq,@FirstTitleEn,@FirstTitleSr,@FirstContentSq,@FirstContentEn,@FirstContentSr,@SecondTitleSq,@SecondTitleEn,@SecondTitleSr,@SecondContentSq,@SecondContentEn,@SecondContentSr,@ThirdTitleSq,@ThirdTitleEn,@ThirdTitleSr,@ThirdContentSq,@ThirdContentEn,@ThirdContentSr,@CreatedUserId,@UpdatedUserId ", new
                {
                    CRUDOperation = (int)CRUDOperation.Update,
                    Id = footer.Id,
                    FirstTitleSq = footer.FirstTitleSq,
                    FirstTitleEn = footer.FirstTitleEn,
                    FirstTitleSr = footer.FirstTitleSr,
                    FirstContentSq = footer.FirstContentSq,
                    FirstContentEn = footer.FirstContentEn,
                    FirstContentSr = footer.FirstContentSr,
                    SecondTitleSq = footer.SecondTitleSq,
                    SecondTitleEn = footer.SecondTitleEn,
                    SecondTitleSr = footer.SecondTitleSr,
                    SecondContentSq = footer.SecondContentSq,
                    SecondContentEn = footer.SecondContentEn,
                    SecondContentSr = footer.SecondContentSr,
                    ThirdTitleSq = footer.ThirdTitleSq,
                    ThirdTitleEn = footer.ThirdTitleEn,
                    ThirdTitleSr = footer.ThirdTitleSr,
                    ThirdContentSq = footer.ThirdContentSq,
                    ThirdContentEn = footer.ThirdContentEn,
                    ThirdContentSr = footer.ThirdContentSr,
                    CreatedUserId = footer.CreatedUserId,
                    UpdatedUserId = footer.UpdatedUserId
                });
                if (createUpdateResult.HasAffected)
                {
                    HasAffected = true;
                    //var afterLogData = (await new Query().SelectSingle<Footer>($"SELECT TOP 1 * FROM Footer where Id = {footer.Id}")).Result;
                    //var serializedObject = new ChangeLogHelper().SerializeObject(beforeLogData, afterLogData, (int)ChangeLogTable.Footer, UserId, (int)ChangeLogAction.Update);
                    //var addLog = new ChangeLogHelper().AddLog(serializedObject);
                }
                return RedirectToAction("Index", "Footer");
            }
            else
            {
                return RedirectToAction("Index", "Footer", footer);
            }
        }

    }
}
