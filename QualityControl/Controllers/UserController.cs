using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using QualityControl.Db;
using QualityControl.Enum;
using QualityControl.Util;
using QualityControl.Models;
using Newtonsoft.Json;

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
                    ViewBag.list = x;
                    return View();
                }
                
            }
           
            if (!string.IsNullOrEmpty(key))
            x = Db.Products.Where(e => e.Name.Contains(key)).ToList();

            x = Db.Products.Take(20).ToList();
            ViewBag.list = x;

            
            return View();
        }

        public ActionResult ChooseProductBatchs(long pid)
        {
            var p = Db.Products.Find(pid);
            var list = p.BaseProductBatchs;
            ViewBag.list = list;
            ViewBag.p = p;
            return View();
        }

        public ActionResult Choose(long productId, List<long> batchIds)
        {
            var userid = User.Identity.GetUserId();
            var user = UserManager.FindById(userid);
            var p = Db.Products.Find(productId);  
            if (p == null)
            {
                throw new Exception("访问错误！");
            }
            var batches = new List<ProductBatch>();
            foreach(var item in batchIds)
            {
                var batch = p.BaseProductBatchs.FirstOrDefault(a => a.Id == item);
                if (batch == null)
                {
                    throw new Exception("访问错误！");
                }
                var pb = new ProductBatch();
                Dumper.Dump(batch, pb, false);
                batches.Add(pb);
            }
            var pStr = new ProductCopy(p);
            var trade = new Db.Trade
            {
                Product = JsonConvert.SerializeObject(pStr),
                CeateTime =DateTime.Now,
                FinishTime =DateTime.Now,  
                UserId=userid,
                Status =(int)EnumTradeStatus.Create,
                SGSPaied = false
            };
            Db.Trades.Add(trade);
            Db.SaveChanges();
            return View();
        }

        /// <summary>
        /// 选择productbatch
        /// </summary>
        /// <returns></returns>
        public ActionResult ChoosePb(List<long> list)
        {

            return View();
        }

        public ActionResult GetTrade(long id)
        {
            return View();
        }

        public ActionResult Apply()
        {
            var userid = User.Identity.GetUserId();
            var user = UserManager.FindById(userid);
            if (user.Type != (int)EnumUserType.User)
            {

                throw new Exception("您不是用户，无权限查看！");
            }
            var trade = Db.Trades.FirstOrDefault(e => e.UserId == userid&&e.Status==(int)(EnumTradeStatus.Create));
            ViewBag.t = trade;
            return View();
        }

        
    }
}