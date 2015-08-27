using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Caching;
using QualityControl.Models;
using QualityControl.Db;

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

        protected Singleton Sgt
        {
            get { return Singleton.Self; }
        }

        protected TheContext Db
        {
            get { return Sgt.Db; }
        }

    }
}