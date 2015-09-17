using QualityControl.Db;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace QualityControl.Models.Adapters
{
    public class CompanyInfo
    {
        public Company Company { get; set; }

        public ApplicationUser User { get; set; }
    }
}