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
            return View();
        }

        /// <summary>
        ///     选择产品
        /// </summary>
        /// <param name="key"></param>
        /// <param name="ProductTypeId"></param>
        /// <returns></returns>
        public ActionResult ChooseProduct(string key = "", long ProductTypeId = 0)
        {
            var x = new List<Product>();
            if (ProductTypeId != 0)
            {
                var t = Db.ThirdProductTypes.Find(ProductTypeId);
                if (t == null)
                {
                    throw new Exception("访问错误");
                }
                x = t.Products;
                ViewBag.list = x;
                return View();
            }

            if (!string.IsNullOrEmpty(key))
                x = Db.Products.Where(e => e.Name.Contains(key)).ToList();

            x = Db.Products.Take(20).ToList();
            ViewBag.list = x;


            return View();
        }

        /// <summary>
        ///     选择批次
        /// </summary>
        /// <param name="pid"></param>
        /// <returns></returns>
        public ActionResult ChooseProductBatchs(long pid)
        {
            var p = Db.Products.Find(pid);
            var list = p.BaseProductBatchs;
            ViewBag.list = list;
            ViewBag.p = p;
            return View();
        }

        /// <summary>
        ///     提交订单
        /// </summary>
        /// <param name="productId"></param>
        /// <param name="batchIds"></param>
        /// <returns></returns>
        public ActionResult Choose(long productId, List<long> batchIds)
        {
            var userid = User.Identity.GetUserId();
            UserManager.FindById(userid);
            var p = Db.Products.Find(productId);
            if (p == null)
            {
                throw new Exception("访问错误！");
            }
            var batches = new List<ProductBatch>();
            foreach (var item in batchIds)
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
            var trade = new Trade
            {
                Product = JsonConvert.SerializeObject(pStr),
                CeateTime = DateTime.Now,
                FinishTime = DateTime.Now,
                SamplingDate = DateTime.Now,
                DetectingDate = DateTime.Now,
                UserId = userid,
                Status = (int) EnumTradeStatus.Create,
                SGSPaied = false,
                Batches = batches,
                ManufacturerId = p.UserId
            };
            Db.Trades.Add(trade);
            Db.SaveChanges();
            return View();
        }

        /// <summary>
        ///     订单列表
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
            }
            else
            {
                return Content("错误操作");
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
            if (user.Type != (int) EnumUserType.User)
            {
                throw new Exception("您不是用户，无权限查看！");
            }
            var trade = Db.Trades.FirstOrDefault(e => e.UserId == userid && e.Status == (int) (EnumTradeStatus.Create));
            ViewBag.t = trade;
            return View();
        }
    }
}