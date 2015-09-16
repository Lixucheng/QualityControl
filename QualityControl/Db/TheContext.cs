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

        //public DbSet<RegisterApply> RegisterApplies { get; set; }
        
        public DbSet<ProductType> ProductTypes { get; set; }

        public DbSet<Product> Products { get; set; }

        public DbSet<Company> Companies { get; set; }

        public DbSet<CompanyProduct> CompanyProducts { get; set; }

        public DbSet<Message> Messages { get; set; }

        public DbSet<DetectionScheme> DetectionSchemes { get; set; }

        public DbSet<Contract> Contracts { get; set; }

        public DbSet<ContractModification> ContractModifications { get; set; }

        public DbSet<ProductBatch> ProductBatchs { get; set; }


        /// <summary>
        /// 协议内容
        /// </summary>
        public DbSet<Compact> Compacts { get; set; }
    }
}