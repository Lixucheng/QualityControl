﻿using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using QualityControl.Enum;

namespace QualityControl.Controllers
{
    [Authorize]
    public class AccountAdminController : BaseController
    {
        /// <summary>
        ///     账户审核部分
        /// </summary>
        private ApplicationUserManager _userManager;

        public ApplicationUserManager UserManager
        {
            get { return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>(); }
            private set { _userManager = value; }
        }

        public ActionResult AccountIndex()
        {
            var list = UserManager.Users.Where(e => e.Type != (int) EnumUserType.SuperAdmin && e.Del == 0).ToList();
            return View(list);
        }

        public int Del(string id)
        {
            var list = Db.Trades.Where(e => e.SgsUserId == id || e.UserId == id || e.ManufacturerId == id).ToList();
            if (list.Count > 0)
            {
                return 0;
            }

            var u = UserManager.FindById(id);
            u.Del = 1;
            UserManager.Update(u);
            if (u.Type == (int) EnumUserType.Producer)
            {
                var c = Db.Companies.FirstOrDefault(e => e.UserId == id);
                if (c != null)
                {
                    c.Status = EnumStatus.Del;
                    c.Products.ForEach(e => e.Status = EnumStatus.Del);
                }
            }
            else if (u.Type == (int) EnumUserType.TestingOrg)
            {
                var s = Db.SGSs.FirstOrDefault(e => e.UserId == id);
                if (s != null)
                {
                    s.Status = EnumStatus.Del;
                }
            }
            Db.SaveChanges();
            return 1;
        }

        // GET: AccountAdmin

        #region  普通账户审核部分

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult ApplyList()
        {
            var list =
                UserManager.Users.Where(
                    e => e.Status == (int) EnumUserStatus.UnRecognized).ToList();
            ViewBag.list = list;
            ViewBag.count = list.Count;
            return View();
        }

        public async Task<int> PassOne(string id)
        {
            var user = UserManager.Users.FirstOrDefault(e => e.Id == id);
            user.Status = (int) EnumUserStatus.Normal;
            await UserManager.UpdateAsync(user);
            //todo status

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
            await PassOne(id);
            var type = UserManager.Users.FirstOrDefault(e => e.Id == id).Type;
            if (type == (int) EnumUserType.User)
                return Redirect("../ApplyList");
            if (type == (int) EnumUserType.Producer)
                return Redirect("../CompanyApplyList");
            return Redirect("../ApplyList");
            //todo: 这块肯定要改
        }

        public async Task<ActionResult> Refuse(string id)
        {
            var type = UserManager.Users.FirstOrDefault(e => e.Id == id).Type;
            await RefuseOne(id);

            if (type == (int) EnumUserType.User)
                return Redirect("../ApplyList");
            if (type == (int) EnumUserType.Producer)
            {
                var comp = Db.Companies.FirstOrDefault(e => e.UserId == id);
                Db.Companies.Remove(comp);
                Db.SaveChanges();
                return Redirect("../CompanyApplyList");
            }
            return Redirect("../ApplyList");
        }

        public async Task<int> PassList(string[] ids)
        {
            foreach (var id in ids)
            {
                await PassOne(id);
            }

            return 1;
        }

        public async Task<int> RefuseList(string[] ids)
        {
            foreach (var id in ids)
            {
                await RefuseOne(id);
            }

            return 1;
        }

        #endregion

        #region  生产商账户审核部分

        public ActionResult CompanyApplyList()
        {
            var list =
                UserManager.Users.Where(
                    e => e.Status == (int) EnumUserStatus.UnRecognized && e.Type == (int) EnumUserType.Producer)
                    .ToList();
            ViewBag.list = list;
            ViewBag.count = list.Count;
            return View();
        }

        public JsonResult GetCompanyInfo(string userid)
        {
            var c = Db.Companies.FirstOrDefault(e => e.UserId == userid);


            return Json(new {c, time = c.EstablishedTime.ToString()}, JsonRequestBehavior.AllowGet);
        }

        #endregion
    }
}