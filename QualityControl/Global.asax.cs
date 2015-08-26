using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using System.Data.Entity;
using QualityControl.Db;

namespace QualityControl
{
    public class MvcApplication : HttpApplication
    {
        protected void Application_Start()
        {
            // Code First Db init
            Database.SetInitializer(new DropCreateDatabaseIfModelChanges<TheContext>());

            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
        }
    }
}
