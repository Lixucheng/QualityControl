﻿using System.Data.Entity;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using QualityControl.Db;

namespace QualityControl.Models
{
    // 可以通过向 ApplicationUser 类添加更多属性来为用户添加配置文件数据。若要了解详细信息，请访问 http://go.microsoft.com/fwlink/?LinkID=317594。
    public class ApplicationUser : IdentityUser
    {
        public int Type { get; set; }
        public int Status { get; set; }

        public int Del { get; set; }

        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager)
        {
            // 请注意，authenticationType 必须与 CookieAuthenticationOptions.AuthenticationType 中定义的相应项匹配
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);

            // 在此处添加自定义用户声明
            userIdentity.AddClaim(new Claim("Type", Type.ToString()));
            userIdentity.AddClaim(new Claim("Status", Status.ToString()));
            userIdentity.AddClaim(new Claim("Email", Email));
            userIdentity.AddClaim(new Claim("Del", Del.ToString()));
            return userIdentity;
        }
    }

    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext()
            : base("TheContext", false)
        {
        }

        public DbSet<ThirdProductType> ThirdProductTypes { get; set; }
        public DbSet<FirstProductType> FirstProductTypes { get; set; }
        public DbSet<SecondProductType> SecondProductTypes { get; set; }

        public DbSet<Product> Products { get; set; }

        public DbSet<Company> Companies { get; set; }

        public DbSet<CompanyProduct> CompanyProducts { get; set; }

        public DbSet<Message> Messages { get; set; }

        public DbSet<DetectionScheme> DetectionSchemes { get; set; }

        public DbSet<Contract> Contracts { get; set; }

        public DbSet<ContractModification> ContractModifications { get; set; }

        public DbSet<ProductBatch> ProductBatchs { get; set; }

        public DbSet<ProductDectectionItem> ProductDectectionItems { get; set; }


        /// <summary>
        ///     协议内容
        /// </summary>
        public DbSet<Compact> Compacts { get; set; }

        public DbSet<QrCodeInfo> QrCodeInfos { get; set; }

        public DbSet<SGS> SGSs { get; set; }

        public DbSet<Trade> Trades { get; set; }
        public DbSet<SgsProduct> SgsProducts { get; set; }

        public DbSet<CheckLevel> Levels { get; set; }

        public DbSet<BaseProductBatch> BaseProductBatchs { get; set; }

        public DbSet<BatchReport> BatchReports { get; set; }

        public DbSet<Verification> Verifications { get; set; }

        public DbSet<DectectionItem> DectectionItems { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<ApplicationUser>().ToTable("GxUser");
            modelBuilder.Entity<IdentityRole>().ToTable("GxIdentityRole");
            modelBuilder.Entity<IdentityUserLogin>().ToTable("GxIdentityUserLogin");
            modelBuilder.Entity<IdentityUserRole>().ToTable("GxIdentityUserRole");
            modelBuilder.Entity<IdentityUserClaim>().ToTable("GxIdentityUserClaim");
        }

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }
    }
}