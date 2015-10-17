using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using QualityControl.Models;

namespace QualityControl
{
    public class MvcApplication : HttpApplication
    {
        protected void Application_Start()
        {
            // Code First Db init
            //Database.SetInitializer(new DropCreateDatabaseIfModelChanges<TheContext>());
            //Database.SetInitializer(new DropCreateDatabaseIfModelChanges<ApplicationDbContext>());
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
        }

        protected void Application_BeginRequest()
        {
            HttpContext.Current.Items[Singleton.SingletonName] = new Singleton();
        }

        protected void Application_EndReuqest()
        {
            var sgt = HttpContext.Current.Items[Singleton.SingletonName] as Singleton;
            if (sgt != null)
            {
                sgt.Dispose();
            }
        }
    }
}