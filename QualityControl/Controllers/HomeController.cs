using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;

namespace QualityControl.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            System.Web.Caching.Cache cache = HttpRuntime.Cache;
            cache.Insert("test", "hello", null, DateTime.Now.AddSeconds(60), TimeSpan.Zero, System.Web.Caching.CacheItemPriority.Normal, null);
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