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

        public JsonResult GetDescription(long id)
        {                
            return Json(Db.ProductTypes.Find(id).Description,JsonRequestBehavior.AllowGet); 
        }
    }
}