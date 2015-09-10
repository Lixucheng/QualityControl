using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace QualityControl.Controllers
{
    public class ProductController : BaseController
    {
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
            var x = Db.ProductTypes.Find(newone.Id);
            if(x!=null)
            {
                x.Title = newone.Title;
                x.Description = newone.Description;
                Db.SaveChanges();
            }
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
    }
}