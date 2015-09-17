using QualityControl.Db;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace QualityControl.Models
{
    public class Singleton
    {

        public static string SingletonName
        {
            get { return "__Singleton__";  }
        }

        public static Singleton Self
        {
            get { return HttpContext.Current.Items[SingletonName] as Singleton;  }
        }

        private ApplicationDbContext _db;
        public ApplicationDbContext Db
        {
            get
            {
                return _db ?? (_db = new ApplicationDbContext());
            }
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