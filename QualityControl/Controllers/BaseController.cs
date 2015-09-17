using System;
using System.Web;
using System.Web.Mvc;
using System.Web.Caching;
using QualityControl.Models;
using Microsoft.AspNet.Identity.Owin;

namespace QualityControl.Controllers
{
    public class BaseController : Controller
    {

        private ApplicationUserManager _userManager;

        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }

        public BaseController() { }

        public BaseController(ApplicationUserManager userManager)
        {
            UserManager = userManager;
        }

        protected Cache CacheManager
        {
            get { return HttpRuntime.Cache; }
        }

        protected void AddCache(string key, object value, DateTime time)
        {
            CacheManager.Add(key, value, null, time, TimeSpan.Zero, CacheItemPriority.Normal, null);
        }

        protected Singleton Sgt
        {
            get { return Singleton.Self; }
        }

        protected ApplicationDbContext Db
        {
            get { return Sgt.Db; }
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (_userManager != null)
                {
                    _userManager.Dispose();
                    _userManager = null;
                }
            }

            base.Dispose(disposing);
        }

    }
}