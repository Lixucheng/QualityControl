using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Caching;

namespace QualityControl.Controllers
{
    public class BaseController : Controller
    {

        protected Cache CacheManager
        {
            get { return HttpRuntime.Cache; }
        }

        protected void AddCache(string key, object value, DateTime time)
        {
            CacheManager.Add(key, value, null, time, TimeSpan.Zero, CacheItemPriority.Normal, null);
        }

    }
}