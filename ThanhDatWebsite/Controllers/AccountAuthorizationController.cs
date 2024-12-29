using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ThanhDatWebsite.Controllers
{
    public class AccountAuthorizationController : Controller
    {
        thanhdatEntities1 db = new thanhdatEntities1();
        // GET: AccountAuthorization
        public ActionResult Index()
        {
            var account = db.Accounts.ToList();
            return View(account);
        }
        [HttpPost]
        public JsonResult UpdateRole(string accountId, string newRole)
        {
            var account = db.Accounts.FirstOrDefault(a => a.AccountID == accountId);
            if (account != null)
            {
                account.Role = newRole;
                db.SaveChanges();
                return Json(new { success = true, message = "Vai trò đã được cập nhật thành công." });
            }

            return Json(new { success = false, message = "Không tìm thấy tài khoản." });
        }
    }
}