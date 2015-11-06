using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Newtonsoft.Json;
using QualityControl.Db;
using QualityControl.Enum;
using QualityControl.Models;
using QualityControl.Util;



namespace QualityControl.Controllers
{
    [Authorize]
    public class UserController : BaseController
    {
        public ActionResult Index()
        {
            var userid = User.Identity.GetUserId();
            var already = Db.Trades.Count(e => (e.UserId == userid || e.SgsUserId == userid)&&e.Status==(int)EnumTradeStatus.Finish);
            var ing= Db.Trades.Count(e => (e.UserId == userid || e.SgsUserId == userid) && e.Status != (int)EnumTradeStatus.Finish);

            ViewBag.a = already;
            ViewBag.i = ing;
            var list = Db.Products
                .OrderBy(a => Guid.NewGuid())
                .Take(3)
                .ToList();
            ViewBag.list = list;
            return View(list);
        }

        /// <summary>
        ///     管控合同列表
        /// </summary>
        /// <returns></returns>
        public ActionResult TradeList()
        {
            var userId = User.Identity.GetUserId();
            var user = Db.Users.Find((userId));
            var type = user.Type;
            ViewBag.Type = type;
            List<Trade> list;
            if (type == (int) EnumUserType.User)
            {
                list = Db.Trades.Where(e => e.UserId == userId).ToList();
            }
            else if (type == (int) EnumUserType.TestingOrg)
            {
                list = Db.Trades.Where(e => e.SgsUserId == userId).ToList();
            }
            else if (type == (int) EnumUserType.Producer)
            {
                list = Db.Trades.Where(e => e.ManufacturerId == userId).ToList();
                list.AddRange(Db.Trades.Where(e=>e.UserId==userId).ToList());
            }
            else
            {
                list = Db.Trades.ToList();
            }
            return View(list);
        }

        public ActionResult GetTrade(long id)
        {
            return View();
        }

        public ActionResult Apply()
        {
            var userid = User.Identity.GetUserId();
            var user = UserManager.FindById(userid);
            if (user.Type != (int) EnumUserType.User&& user.Type != (int)EnumUserType.Producer)
            {
                throw new Exception("您不是用户，无权限查看！");
            }
            var trade = Db.Trades.FirstOrDefault(e => e.UserId == userid && e.Status == (int) (EnumTradeStatus.Create));
            ViewBag.t = trade;
            return View();
        }
    }
}