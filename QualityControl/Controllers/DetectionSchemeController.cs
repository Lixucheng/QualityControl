using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace QualityControl.Controllers
{
    public class DetectionSchemeController : Controller
    {
        //检测方案合同部分
        // GET: DetectionScheme
        public ActionResult BuildDetectionScheme()
        {
            return View();
        }

        public ActionResult SignContract()
        {
            return View();
        }
    }
}