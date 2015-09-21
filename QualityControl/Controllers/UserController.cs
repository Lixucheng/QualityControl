using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace QualityControl.Controllers
{
    public class UserController : BaseController
    {
        // GET: User
        public ActionResult ChooseProduct(string key="",long t1=0,long t2=0,long t3=0)
        {
            var x = new List<Db.CompanyProduct>();
            if (t3 != 0)
            {
                var t = Db.ThirdProductTypes.Find(t3);
                if (t == null)
                {
                    throw new Exception("访问错误");
                }
                
            }
           
            //   if (!string.IsNullOrEmpty(key))
            x = Db.CompanyProducts.Where(e => e.Name.Contains(key)).ToList();
            ViewBag.list = x;
            return View();
        }
    }
}