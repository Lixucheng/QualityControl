using System.Web.Mvc;

namespace QualityControl.Controllers
{
    public class HomeController : BaseController
    {
        public ActionResult Index()
        {
            var a = 123.ToString("D15");
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