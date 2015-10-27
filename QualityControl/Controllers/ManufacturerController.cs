using System.Web.Mvc;
using QualityControl.Db;
using System;
using Microsoft.AspNet.Identity;
using QualityControl.Enum;
using System.Data.Entity;

namespace QualityControl.Controllers
{
    public class ManufacturerController : BaseController
    {
        public ManufacturerController()
        {
        }

        public ManufacturerController(ApplicationUserManager userManager)
            : base(userManager)
        {
        }

        [HttpGet]
        public ActionResult AddProduct(long id = -1)
        {
            if (id != -1)
            {
                //var product = 
            }
            return View();
        }

        public ActionResult AddProduct(Product model)
        {
            return View();
        }

        public ActionResult Uploader(long id)
        {
            var trade = Db.Trades.Find(id);
            if(trade==null)
            {
                throw new Exception("交易不存在！");
            }
            var uid=User.Identity.GetUserId();
            var user = UserManager.FindById(uid);
            if(user.Type!=(int)EnumUserType.Producer)
            {
                throw new Exception("您无权查看！");
            }

            return View(trade);
        }

        public bool Finish(long id)
        {
            var userId = User.Identity.GetUserId();
            var user = Db.Users.Find(userId);
            var trade = Db.Trades.Find(id);
            if (trade == null)
            {
                return false;
            }
            if (user.Type == (int)EnumUserType.Controller)
            {
                trade.Status = (int)EnumTradeStatus.SampleStart;
            }
            else
            {
                return false;
            }
            Db.Entry(trade).State = EntityState.Modified;
            Db.SaveChanges();
            return true;
        }
    }
}