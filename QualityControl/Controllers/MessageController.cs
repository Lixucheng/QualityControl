using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;

namespace QualityControl.Controllers
{
   
    public class MessageController : BaseController
    {
        // GET: Message
        [Authorize]
        public ActionResult Index(int s=0)
        {
            var id = User.Identity.GetUserId();
            var list = Db.Messages.Where(e => e.UserId == id && e.Status == s).ToList();
            ViewBag.list = list;
            ViewBag.s = s;
            return View();
        }

        
        public ActionResult Read(long id)
        {
            var m = Db.Messages.Find(id);
            m.Status = 1;
            Db.SaveChanges();
            return Redirect("/message/index");
        }

        

       
    }
}