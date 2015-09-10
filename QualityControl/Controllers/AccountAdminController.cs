using Microsoft.AspNet.Identity.Owin;
using QualityControl.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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
            var list=UserManager.Users.Where(e => e.Status == (int)EnumUserStatus.UnRecognized&&e.Type==(int)EnumUserType.User).ToList();
            ViewBag.list = list;
            ViewBag.count = list.Count;      
            return View();
        }

        public async Task<int>  PassOne(string id)
        {
            var user = UserManager.Users.FirstOrDefault(e => e.Id == id);
            user.Status = (int)EnumUserStatus.Normal;
            await UserManager.UpdateAsync(user);
            Db.Messages.Add(new QualityControl.Db.Message
            {
                Content = "恭喜您，您的注册申请已经通过审核！",
                UserId = user.Id,
                Status = 0
            });
            Db.SaveChanges();
            return 1;
        }
        public async Task<int> RefuseOne(string id)
        {
            var user = UserManager.Users.FirstOrDefault(e => e.Id == id);
            await UserManager.SendEmailAsync(user.Id, "账户审核通知", "对不起，您的账户审核未通过！");
            await UserManager.DeleteAsync(user);
            // 有关如何启用帐户确认和密码重置的详细信息，请访问 http://go.microsoft.com/fwlink/?LinkID=320771
            // 发送包含此链接的电子邮件    账户审核通过通知     
            return 1;
        }

        public async Task<ActionResult> Pass(string id)
        {
            await  PassOne(id);
            return Redirect("../ApplyList");
        }

        public async Task<ActionResult> Refuse(string id)
        {

            await RefuseOne(id);
            return Redirect("../ApplyList");
        }

        public async Task<int> PassList(string [] ids)
        {        
            foreach (var id in ids)
            {
              await  PassOne(id);
            }

            return 1;
        }

        public async Task<int> RefuseList(string[] ids)
        {
            foreach (var id in ids)
            {
              await  RefuseOne(id);
            }

            return 1;
        }


        public ActionResult CompanyApplyList()
        {
            var list = UserManager.Users.Where(e => e.Status == (int)EnumUserStatus.UnRecognized && e.Type == (int)EnumUserType.Producer).ToList();
            ViewBag.list = list;
            ViewBag.count = list.Count;
            return View();
        }

    }
}