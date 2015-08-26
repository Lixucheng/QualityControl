using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace QualityControl.Db
{
    public class TheContext : DbContext
    {
        public TheContext()
            :base("name=TheContext"){}


    }
}