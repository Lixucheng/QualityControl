using QualityControl.Db;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace QualityControl.Controllers
{
    public class ManufacturerController : BaseController
    {
        public ManufacturerController() { }

        public ManufacturerController(ApplicationUserManager userManager) 
            :base(userManager)
        {

        }

        [HttpGet]
        public ActionResult AddProduct(long id = -1)
        {
            if (id != -1)
            {
                //var product = 
            }
            return View();
        }

        public ActionResult AddProduct(Product model)
        {
            return View();
        }
    }
}