using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using QualityControl.Db;
using QualityControl.Enum;

namespace QualityControl.Controllers
{
    [Authorize]
    public class UserController : BaseController
    {
        // GET: User
        public ActionResult ChooseProduct(string key="",long ProductTypeId=0)
        {
            var x = new List<Db.Product>();
            if (ProductTypeId != 0)
            {
                var t = Db.ThirdProductTypes.Find(ProductTypeId);
                if (t == null)
                {
                    throw new Exception("访问错误");
                }
                else
                {
                    x = t.Products;
                }
                
            }
           
            if (!string.IsNullOrEmpty(key))
            x = Db.Products.Where(e => e.Name.Contains(key)).ToList();

            x = Db.Products.Take(20).ToList();
            ViewBag.list = x;

            
            return View();
        }

        public ActionResult Choose(long id)
        {
            var userid = User.Identity.GetUserId();
            var user = UserManager.FindById(userid);
            if (user.Type != (int)EnumUserType.User)
            {

                throw new Exception("您不是用户，无权限查看！");
            }
            var p = Db.Products.Find(id);
            if (p == null)
            {
                throw new Exception("访问错误！");
            }
            var trade = new Db.Trade
            {
                Product = p,CeateTime=DateTime.Now,FinishTime=DateTime.Now,  
                UserId=userid,Status=(int)EnumTradeStatus.Create   
            };
            Db.Trades.Add(trade);
            Db.SaveChanges();
            return View();
        }

        public ActionResult GetTrade(long id)
        {
            return View();
        }

        
    }
}