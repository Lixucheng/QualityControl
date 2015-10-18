using System.Web;

namespace QualityControl.Models
{
    public class Singleton
    {
        private ApplicationDbContext _db;

        public static string SingletonName
        {
            get { return "__Singleton__"; }
        }

        public static Singleton Self
        {
            get { return HttpContext.Current.Items[SingletonName] as Singleton; }
        }

        public ApplicationDbContext Db
        {
            get { return _db ?? (_db = new ApplicationDbContext()); }
        }

        public void Dispose()
        {
            if (_db != null)
            {
                _db.Dispose();
            }
        }
    }
}