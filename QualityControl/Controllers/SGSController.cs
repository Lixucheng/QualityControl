using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Newtonsoft.Json;
using QualityControl.Db;
using QualityControl.Enum;
using QualityControl.Models;

namespace QualityControl.Controllers
{
    [Authorize]
    public class SGSController : BaseController
    {
        // GET: SGS
        public ActionResult Index()
        {
            var userId = User.Identity.GetUserId();
            var sgs = Db.SGSs.FirstOrDefault(a => a.UserId == userId);

            var userid = User.Identity.GetUserId();
            var already = Db.Trades.Count(e => (e.UserId == userid || e.SgsUserId == userid) && e.Status == (int)EnumTradeStatus.Finish);
            var ing = Db.Trades.Count(e => (e.UserId == userid || e.SgsUserId == userid) && e.Status != (int)EnumTradeStatus.Finish);

            ViewBag.a = already;
            ViewBag.i = ing;
            var list = Db.Products
                .OrderBy(a => Guid.NewGuid())
                .Take(3)
                .ToList();
            ViewBag.list = list;
            return View(sgs);
        }

        [HttpGet]
        public ActionResult Save()
        {
            var userId = User.Identity.GetUserId();
            var sgs = Db.SGSs.FirstOrDefault(a => a.UserId == userId);
            return View(sgs);
        }

        [HttpPost]
        public ActionResult Save(SGS model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            model.UserId = User.Identity.GetUserId();
            var sgs = Db.SGSs.FirstOrDefault(a => a.UserId == model.UserId);
            if (sgs == null)
            {
                model.Status = EnumStatus.FirstUncheked;
                model.CreateTime = DateTime.Now;
                model.LastChangeTime = model.CreateTime;
                Db.SGSs.Add(model);
            }
            else
            {
                if (Util.Util.Equal(model, sgs,
                    excepts: new List<string> {"UserId", "CreateTime", "LastChangeTime", "Status"}))
                {
                    return RedirectToAction("Index");
                }
                if (sgs.Status == EnumStatus.FirstUncheked)
                {
                    Util.Util.Dump(model, sgs,
                        excepts: new List<string> {"UserId", "CreateTime", "LastChangeTime", "Status"});
                }
                else
                {
                    sgs.UpdateJson = JsonConvert.SerializeObject(model);
                    sgs.LastChangeTime = DateTime.Now;
                    sgs.Status = EnumStatus.Unchecked;
                }
                Db.Entry(sgs).State = EntityState.Modified;
            }
            Db.SaveChanges();
            return RedirectToAction("Index");
        }

        public ActionResult ChooseProduct(string key = "", long ProductTypeId = 0)
        {
            var userid = User.Identity.GetUserId();
            var user = UserManager.FindById(userid);
            if (user.Type != (int) EnumUserType.TestingOrg)
            {
                throw new Exception("您不是检测中心管理员，无权限查看！");
            }

            var x = new List<Product>();
            if (ProductTypeId != 0)
            {
                var t = Db.ThirdProductTypes.Find(ProductTypeId);
                if (t == null)
                {
                    throw new Exception("访问错误");
                }
                x = t.Products;
            }

            if (!string.IsNullOrEmpty(key))
                x = Db.Products.Where(e => e.Name.Contains(key)).ToList();

            x = Db.Products.Take(20).ToList();
            ViewBag.list = x;

            var sgs = Db.SGSs.First(e => e.UserId == userid);
            ViewBag.hadlist = sgs.Products.Select(e => e.Product.Id).ToList();

            return View();
        }

        public ActionResult AddSgsProduct(long pid, float price, int days)
        {
            var userid = User.Identity.GetUserId();
            var sgs = Db.SGSs.First(e => e.UserId == userid);
            var sgsp = new SgsProduct
            {
                NeedeDay = days,
                Price = price,
                Product = Db.Products.Find(pid)
            };
            sgs.Products.Add(sgsp);
            Db.SaveChanges();
            return Redirect("./ChooseProduct");
        }

        public ActionResult ManageProducts()
        {
            var userid = User.Identity.GetUserId();
            var user = UserManager.FindById(userid);
            if (user.Type != (int) EnumUserType.TestingOrg)
            {
                throw new Exception("您不是检测中心管理员，无权限查看！");
            }


            var sgs = Db.SGSs.First(e => e.UserId == userid);
            ViewBag.list = sgs.Products;

            return View();
        }

        public ActionResult UpdateSGSProduct(long pid, int price, int days)
        {
            var sgsp = Db.SgsProducts.Find(pid);
            sgsp.Price = price;
            sgsp.NeedeDay = days;
            Db.SaveChanges();
            return Redirect("./ManageProducts");
        }

        /// <summary>
        /// 验证样品
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult Verification(long id)
        {        
            var list = Db.Verifications.Where(e => e.TradeId == id).ToList();
            ViewBag.tid = id;
            return View(list);
        }

        public List<string> GetNum(long id)
        {
            var l = Db.QrCodeInfos.Where(e => e.TradeId == id).Select(a=>a.IdCode).ToList();
            return l;
        }

        public JsonResult Check(long id,string idcode)
        {

         
            var l = GetNum(id);
            var r = l.Any(e => e == idcode);
          
            if (r==true)
            {
                var info= Db.QrCodeInfos.FirstOrDefault(e => e.IdCode == idcode && e.TradeId == id);
                var a = Db.Verifications.FirstOrDefault(e => e.QrCodeInfo.Id == info.Id);
                if(a!=null)
                {
                    return Json(3);
                }
                var x = new Verification();
                x.TradeId = id;
                x.Status = EnumVerificationStatus.通过;
                x.QrCodeInfo = info;
                Db.Verifications.Add(x);
                Db.SaveChanges();
                //var ret = new { b = 1, num =x.QrCodeInfo.QrName };
                return Json(1);
            }


            //var ret1 = new { b = 0};
            return Json(0);
        }

        #region 检测项目

        public ActionResult Items()
        {
            var userId = User.Identity.GetUserId();
            var sgs = Db.SGSs.FirstOrDefault(a => a.UserId == userId);
            if (sgs == null)
            {
                return Content("错误操作");
            }
            return View();
        }

        public string AllItems()
        {
            var userId = User.Identity.GetUserId();
            var sgs = Db.SGSs.FirstOrDefault(a => a.UserId == userId);
            if (sgs == null)
            {
                return null;
            }
            return JsonConvert.SerializeObject(sgs.DectectionItems);
        }

        public long SaveItem(DectectionItem item)
        {
            if (!ModelState.IsValid)
            {
                return 0;
            }
            var userId = User.Identity.GetUserId();
            var sgs = Db.SGSs.FirstOrDefault(a => a.UserId == userId);
            if (sgs == null)
            {
                return 0;
            }
            if (item.Id == 0)
            {
                sgs.DectectionItems.Add(item);
            }
            else
            {
                var it = sgs.DectectionItems.FirstOrDefault(a => a.Id == item.Id);
                if (it == null)
                {
                    return 0;
                }
                Util.Dumper.Dump(item, it);
                Db.Entry(it).State = EntityState.Modified;
            }
            sgs.DectectionItemString = "";
            var items = sgs.DectectionItems.Select(a => a.Name).OrderBy(a => a).ToList();
            foreach (var i in items)
            {
                sgs.DectectionItemString += "," +  i;
            }
            Db.Entry(sgs).State = EntityState.Modified;
            Db.SaveChanges();
            return item.Id;
        }

        public int RemoveItem(DectectionItem item)
        {
            var userId = User.Identity.GetUserId();
            var sgs = Db.SGSs.FirstOrDefault(a => a.UserId == userId);
            var it = sgs?.DectectionItems.FirstOrDefault(a => a.Id == item.Id);
            if (it == null)
            {
                return 0;
            }
            sgs.DectectionItems.Remove(it);
            var items = sgs.DectectionItems.Select(a => a.Name).OrderBy(a => a).ToList();
            foreach (var i in items)
            {
                sgs.DectectionItemString += "," +  i;
            }
            Db.Entry(sgs).State = EntityState.Modified;
            Db.SaveChanges();
            Db.SaveChanges();
            return 1;
        }
        #endregion
    }
}