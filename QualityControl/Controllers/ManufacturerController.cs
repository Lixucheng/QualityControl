using System.Web.Mvc;
using QualityControl.Db;
using System;
using Microsoft.AspNet.Identity;
using QualityControl.Enum;
using System.Data.Entity;
using System.Collections.Generic;
using Newtonsoft.Json;

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

        [HttpGet]
        public ActionResult Uploader(long id)
        {
            var trade = Db.Trades.Find(id);
            if(trade==null)
            {
                throw new Exception("管控合同不存在！");
            }
            if(trade.Status!=(int)EnumTradeStatus.FinishMakeQrCode)
            {
                throw new Exception("访问错误！");
            }
            var uid=User.Identity.GetUserId();
            var user = UserManager.FindById(uid);
            if(user.Type!=(int)EnumUserType.Producer)
            {
                throw new Exception("您无权查看！");
            }

            return View(trade);
        }

        [HttpPost]
        public ActionResult Uploader(long id, List<string> list)
        {
            var trade = Db.Trades.Find(id);
            if (trade == null)
            {
                throw new Exception("管控合同不存在！");
            }

            var files = JsonConvert.SerializeObject(list);
            trade.Files = files;
            trade.Status = (int)EnumTradeStatus.SampleStart;
            Db.SaveChanges();
            return Redirect("/Trade/TradeDetail/"+id);
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