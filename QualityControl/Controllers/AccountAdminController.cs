using Microsoft.AspNet.Identity.Owin;
using QualityControl.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace QualityControl.Controllers
{
    public class AccountAdminController : BaseController
    {
        private ApplicationUserManager _userManager;
        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }
        // GET: AccountAdmin
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult ApplyList()
        {
            var list=UserManager.Users.Where(e => e.Status == (int)EnumUserStatus.UnRecognized).ToList();
            ViewBag.list = list;
            ViewBag.count = list.Count;
            return View();
        }
        public async System.Threading.Tasks.Task<ActionResult> Pass(string id)
        {
            var user = UserManager.Users.FirstOrDefault(e => e.Id == id);
            user.Status = (int)EnumUserStatus.Normal;
            await UserManager.UpdateAsync(user);               
            return Redirect("../ApplyList");
        }

        public async System.Threading.Tasks.Task<ActionResult> Refuse(string id)
        {
            var user = UserManager.Users.FirstOrDefault(e => e.Id == id);
            await UserManager.SendEmailAsync(user.Id, "账户审核通知", "对不起，您的账户审核未通过！");
            await UserManager.DeleteAsync(user);
            // 有关如何启用帐户确认和密码重置的详细信息，请访问 http://go.microsoft.com/fwlink/?LinkID=320771
            // 发送包含此链接的电子邮件    账户审核通过通知     


            return Redirect("../ApplyList");
        }

    }
}