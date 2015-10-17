using System.Data.Entity.Migrations;

namespace QualityControl.Migrations
{
    public partial class InitialCreate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.GxCompact",
                c => new
                {
                    Id = c.Long(false, true),
                    Content = c.String(),
                    FirstParty = c.String(),
                    SecondParty = c.String(),
                    Time = c.DateTime(false),
                    CheckNum = c.String()
                })
                .PrimaryKey(t => t.Id);

            CreateTable(
                "dbo.GxCompany",
                c => new
                {
                    Id = c.Long(false, true),
                    UserId = c.String(),
                    Name = c.String(false),
                    Address = c.String(false),
                    EstablishedTime = c.DateTime(false),
                    License = c.String(false),
                    CorporationName = c.String(false),
                    CorporationIdentity = c.String(false),
                    OrganizationCode = c.String(false),
                    LicenseScope = c.String(false),
                    Status = c.Int(false),
                    UpdateJson = c.String(),
                    CreateTime = c.DateTime(false),
                    LastChangeTime = c.DateTime(false)
                })
                .PrimaryKey(t => t.Id);

            CreateTable(
                "dbo.GxProduct",
                c => new
                {
                    Id = c.Long(false, true),
                    Name = c.String(),
                    Price = c.Single(false),
                    Status = c.Int(false),
                    UserId = c.String(),
                    Type_Id = c.Long(),
                    Company_Id = c.Long()
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.GxProductType", t => t.Type_Id)
                .ForeignKey("dbo.GxCompany", t => t.Company_Id)
                .Index(t => t.Type_Id)
                .Index(t => t.Company_Id);

            CreateTable(
                "dbo.GxProductType",
                c => new
                {
                    Id = c.Long(false, true),
                    Title = c.String(),
                    Description = c.String()
                })
                .PrimaryKey(t => t.Id);

            CreateTable(
                "dbo.GxCompanyProduct",
                c => new
                {
                    Id = c.Long(false, true),
                    CompanyId = c.Long(false),
                    Name = c.String(),
                    ProductTypeId = c.Long(false),
                    ProductionCertificateNo = c.String(),
                    GetDate = c.String(),
                    Standard = c.String(),
                    CompanyProductStatus = c.Int(false)
                })
                .PrimaryKey(t => t.Id);

            CreateTable(
                "dbo.GxContractModification",
                c => new
                {
                    Id = c.Long(false, true),
                    DetectionSchemeId = c.Long(false),
                    UserId = c.String(),
                    Modify = c.String()
                })
                .PrimaryKey(t => t.Id);

            CreateTable(
                "dbo.GxContract",
                c => new
                {
                    Id = c.Long(false, true),
                    CheckNum = c.String(),
                    ProductId = c.Long(false),
                    Level = c.String(),
                    Quote = c.Double(false),
                    UserId = c.String(),
                    Time = c.Int(false),
                    Status = c.Int(false)
                })
                .PrimaryKey(t => t.Id);

            CreateTable(
                "dbo.GxDetectionScheme",
                c => new
                {
                    Id = c.Long(false, true),
                    CheckNum = c.String(),
                    ProductId = c.Long(false),
                    Level = c.String(),
                    MaxQuote = c.Double(false),
                    MinQuote = c.Double(false),
                    MaxTime = c.Int(false),
                    MinTime = c.Int(false),
                    UserQuote = c.Double(false),
                    OrganQuote = c.Double(false),
                    Time = c.Int(false),
                    Status = c.Int(false)
                })
                .PrimaryKey(t => t.Id);

            CreateTable(
                "dbo.GxMessage",
                c => new
                {
                    Id = c.Long(false, true),
                    UserId = c.String(),
                    Content = c.String(),
                    Status = c.Int(false)
                })
                .PrimaryKey(t => t.Id);

            CreateTable(
                "dbo.GxProductBatch",
                c => new
                {
                    Id = c.Long(false, true),
                    ProductId = c.Long(false),
                    BatchName = c.String(),
                    Count = c.Long(false),
                    CheckNum = c.String()
                })
                .PrimaryKey(t => t.Id);

            CreateTable(
                "dbo.GxIdentityRole",
                c => new
                {
                    Id = c.String(false, 128),
                    Name = c.String(false, 256)
                })
                .PrimaryKey(t => t.Id)
                .Index(t => t.Name, unique: true, name: "RoleNameIndex");

            CreateTable(
                "dbo.GxIdentityUserRole",
                c => new
                {
                    UserId = c.String(false, 128),
                    RoleId = c.String(false, 128)
                })
                .PrimaryKey(t => new {t.UserId, t.RoleId})
                .ForeignKey("dbo.GxIdentityRole", t => t.RoleId, true)
                .ForeignKey("dbo.GxUser", t => t.UserId, true)
                .Index(t => t.UserId)
                .Index(t => t.RoleId);

            CreateTable(
                "dbo.GxUser",
                c => new
                {
                    Id = c.String(false, 128),
                    Type = c.Int(false),
                    Status = c.Int(false),
                    Email = c.String(maxLength: 256),
                    EmailConfirmed = c.Boolean(false),
                    PasswordHash = c.String(),
                    SecurityStamp = c.String(),
                    PhoneNumber = c.String(),
                    PhoneNumberConfirmed = c.Boolean(false),
                    TwoFactorEnabled = c.Boolean(false),
                    LockoutEndDateUtc = c.DateTime(),
                    LockoutEnabled = c.Boolean(false),
                    AccessFailedCount = c.Int(false),
                    UserName = c.String(false, 256)
                })
                .PrimaryKey(t => t.Id)
                .Index(t => t.UserName, unique: true, name: "UserNameIndex");

            CreateTable(
                "dbo.GxIdentityUserClaim",
                c => new
                {
                    Id = c.Int(false, true),
                    UserId = c.String(false, 128),
                    ClaimType = c.String(),
                    ClaimValue = c.String()
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.GxUser", t => t.UserId, true)
                .Index(t => t.UserId);

            CreateTable(
                "dbo.GxIdentityUserLogin",
                c => new
                {
                    LoginProvider = c.String(false, 128),
                    ProviderKey = c.String(false, 128),
                    UserId = c.String(false, 128)
                })
                .PrimaryKey(t => new {t.LoginProvider, t.ProviderKey, t.UserId})
                .ForeignKey("dbo.GxUser", t => t.UserId, true)
                .Index(t => t.UserId);
        }

        public override void Down()
        {
            DropForeignKey("dbo.GxIdentityUserRole", "UserId", "dbo.GxUser");
            DropForeignKey("dbo.GxIdentityUserLogin", "UserId", "dbo.GxUser");
            DropForeignKey("dbo.GxIdentityUserClaim", "UserId", "dbo.GxUser");
            DropForeignKey("dbo.GxIdentityUserRole", "RoleId", "dbo.GxIdentityRole");
            DropForeignKey("dbo.GxProduct", "Company_Id", "dbo.GxCompany");
            DropForeignKey("dbo.GxProduct", "Type_Id", "dbo.GxProductType");
            DropIndex("dbo.GxIdentityUserLogin", new[] {"UserId"});
            DropIndex("dbo.GxIdentityUserClaim", new[] {"UserId"});
            DropIndex("dbo.GxUser", "UserNameIndex");
            DropIndex("dbo.GxIdentityUserRole", new[] {"RoleId"});
            DropIndex("dbo.GxIdentityUserRole", new[] {"UserId"});
            DropIndex("dbo.GxIdentityRole", "RoleNameIndex");
            DropIndex("dbo.GxProduct", new[] {"Company_Id"});
            DropIndex("dbo.GxProduct", new[] {"Type_Id"});
            DropTable("dbo.GxIdentityUserLogin");
            DropTable("dbo.GxIdentityUserClaim");
            DropTable("dbo.GxUser");
            DropTable("dbo.GxIdentityUserRole");
            DropTable("dbo.GxIdentityRole");
            DropTable("dbo.GxProductBatch");
            DropTable("dbo.GxMessage");
            DropTable("dbo.GxDetectionScheme");
            DropTable("dbo.GxContract");
            DropTable("dbo.GxContractModification");
            DropTable("dbo.GxCompanyProduct");
            DropTable("dbo.GxProductType");
            DropTable("dbo.GxProduct");
            DropTable("dbo.GxCompany");
            DropTable("dbo.GxCompact");
        }
    }
}