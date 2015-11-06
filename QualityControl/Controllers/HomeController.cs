using System.Web.Mvc;
using System.Linq;
using System;

namespace QualityControl.Controllers
{
    public class HomeController : BaseController
    {
        public ActionResult Index()
        {
            ViewBag.Products = Db.Products
                .OrderBy(a => Guid.NewGuid())
                .Take(3)
                .ToList();
            ViewBag.Types = Db.ThirdProductTypes.ToList();
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";
            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}