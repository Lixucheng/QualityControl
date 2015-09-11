using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace QualityControl.Controllers
{
    public class ProductController : BaseController
    {
        #region  生产商产品部分
        // GET: Product
        /// <summary>
        /// 产品类别界面
        /// </summary>
        /// <returns></returns>
        public ActionResult TypeIndex()
        {
            var list = Db.ProductTypes.ToList();
            list.ForEach(e => {
                if(e.Description.Length>40)
                e.Description = e.Description.Substring(0, 40) + "...";
            });
            ViewBag.list = list;
            ViewBag.count = list.Count;
            return View();
        }

        /// <summary>
        /// 根据id返回整体
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public JsonResult GetTypeInfo(long id)
        {
            var r = Db.ProductTypes.Find(id);
            var ret = new { Title = r.Title, Id = r.Id, Description = r.Description };
            return Json(ret, JsonRequestBehavior.AllowGet);
        }


        public ActionResult Edit(Db.ProductType newone)
        {
            if (!CheckNewProduct(newone))
            {
               throw new Exception("存在重复或有字段为空，请检查后再输入");
            }
            var x = Db.ProductTypes.Find(newone.Id);
            if(x!=null)
            {
                x.Title = newone.Title;
                x.Description = newone.Description;
                Db.SaveChanges();
            }
            else { throw new Exception("不存在此产品");}
            return Redirect("./TypeIndex");
        }

        public ActionResult Add(Db.ProductType newone)
        {
            if (!CheckNewProduct(newone))
            {
                throw new Exception("存在重复或有字段为空，请检查后再输入");
            }
            Db.ProductTypes.Add(newone);
            Db.SaveChanges();
            return Redirect("./TypeIndex");
        }

        public ActionResult Del(long id)
        {
            var x = Db.ProductTypes.Find(id);
            if (x != null)
            {
                Db.ProductTypes.Remove(x);
                Db.SaveChanges();
            }
            return Redirect("../TypeIndex");
        }

        public ActionResult _Options()
        {
            var list=Db.ProductTypes.ToList();
            ViewBag.list = list;
            ViewBag.count = list.Count;
            return View();
        }
        #endregion


        #region product 逻辑函数

        public bool CheckNewProduct(Db.ProductType newone)
        {
            if (string.IsNullOrEmpty(newone.Title) || string.IsNullOrEmpty(newone.Description))
            {
                return false;
            }
            var x = Db.ProductTypes.Count(e => e.Title == newone.Title);
            return x <= 0;
        }

       

        #endregion
    }
}