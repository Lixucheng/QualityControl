using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace QualityControl.Controllers
{
    public class MessageController : BaseController
    {
        // GET: Message
        public ActionResult Index(string id)
        {
            var list = Db.Messages.Where(e => e.UserId == id && e.Status == 0).ToList();
            ViewBag.list = list;
            return View();
        }

        
        public ActionResult See(long id)
        {
            var m = Db.Messages.Find(id);
            ViewBag.m = m;
            m.Status = 1;
            Db.SaveChanges();
            return View();
        }
    }
}