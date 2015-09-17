namespace QualityControl.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.GxCompact",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        Content = c.String(),
                        FirstParty = c.String(),
                        SecondParty = c.String(),
                        Time = c.DateTime(nullable: false),
                        CheckNum = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.GxCompany",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        UserId = c.String(),
                        Name = c.String(nullable: false),
                        Address = c.String(nullable: false),
                        EstablishedTime = c.DateTime(nullable: false),
                        License = c.String(nullable: false),
                        CorporationName = c.String(nullable: false),
                        CorporationIdentity = c.String(nullable: false),
                        OrganizationCode = c.String(nullable: false),
                        LicenseScope = c.String(nullable: false),
                        Status = c.Int(nullable: false),
                        UpdateJson = c.String(),
                        CreateTime = c.DateTime(nullable: false),
                        LastChangeTime = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.GxProduct",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        Name = c.String(),
                        Price = c.Single(nullable: false),
                        Status = c.Int(nullable: false),
                        UserId = c.String(),
                        Type_Id = c.Long(),
                        Company_Id = c.Long(),
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
                        Id = c.Long(nullable: false, identity: true),
                        Title = c.String(),
                        Description = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.GxCompanyProduct",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        CompanyId = c.Long(nullable: false),
                        Name = c.String(),
                        ProductTypeId = c.Long(nullable: false),
                        ProductionCertificateNo = c.String(),
                        GetDate = c.String(),
                        Standard = c.String(),
                        CompanyProductStatus = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.GxContractModification",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        DetectionSchemeId = c.Long(nullable: false),
                        UserId = c.String(),
                        Modify = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.GxContract",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        CheckNum = c.String(),
                        ProductId = c.Long(nullable: false),
                        Level = c.String(),
                        Quote = c.Double(nullable: false),
                        UserId = c.String(),
                        Time = c.Int(nullable: false),
                        Status = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.GxDetectionScheme",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        CheckNum = c.String(),
                        ProductId = c.Long(nullable: false),
                        Level = c.String(),
                        MaxQuote = c.Double(nullable: false),
                        MinQuote = c.Double(nullable: false),
                        MaxTime = c.Int(nullable: false),
                        MinTime = c.Int(nullable: false),
                        UserQuote = c.Double(nullable: false),
                        OrganQuote = c.Double(nullable: false),
                        Time = c.Int(nullable: false),
                        Status = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.GxMessage",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        UserId = c.String(),
                        Content = c.String(),
                        Status = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.GxProductBatch",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        ProductId = c.Long(nullable: false),
                        BatchName = c.String(),
                        Count = c.Long(nullable: false),
                        CheckNum = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.GxIdentityRole",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Name = c.String(nullable: false, maxLength: 256),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.Name, unique: true, name: "RoleNameIndex");
            
            CreateTable(
                "dbo.GxIdentityUserRole",
                c => new
                    {
                        UserId = c.String(nullable: false, maxLength: 128),
                        RoleId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.UserId, t.RoleId })
                .ForeignKey("dbo.GxIdentityRole", t => t.RoleId, cascadeDelete: true)
                .ForeignKey("dbo.GxUser", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId)
                .Index(t => t.RoleId);
            
            CreateTable(
                "dbo.GxUser",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Type = c.Int(nullable: false),
                        Status = c.Int(nullable: false),
                        Email = c.String(maxLength: 256),
                        EmailConfirmed = c.Boolean(nullable: false),
                        PasswordHash = c.String(),
                        SecurityStamp = c.String(),
                        PhoneNumber = c.String(),
                        PhoneNumberConfirmed = c.Boolean(nullable: false),
                        TwoFactorEnabled = c.Boolean(nullable: false),
                        LockoutEndDateUtc = c.DateTime(),
                        LockoutEnabled = c.Boolean(nullable: false),
                        AccessFailedCount = c.Int(nullable: false),
                        UserName = c.String(nullable: false, maxLength: 256),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.UserName, unique: true, name: "UserNameIndex");
            
            CreateTable(
                "dbo.GxIdentityUserClaim",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserId = c.String(nullable: false, maxLength: 128),
                        ClaimType = c.String(),
                        ClaimValue = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.GxUser", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.GxIdentityUserLogin",
                c => new
                    {
                        LoginProvider = c.String(nullable: false, maxLength: 128),
                        ProviderKey = c.String(nullable: false, maxLength: 128),
                        UserId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.LoginProvider, t.ProviderKey, t.UserId })
                .ForeignKey("dbo.GxUser", t => t.UserId, cascadeDelete: true)
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
            DropIndex("dbo.GxIdentityUserLogin", new[] { "UserId" });
            DropIndex("dbo.GxIdentityUserClaim", new[] { "UserId" });
            DropIndex("dbo.GxUser", "UserNameIndex");
            DropIndex("dbo.GxIdentityUserRole", new[] { "RoleId" });
            DropIndex("dbo.GxIdentityUserRole", new[] { "UserId" });
            DropIndex("dbo.GxIdentityRole", "RoleNameIndex");
            DropIndex("dbo.GxProduct", new[] { "Company_Id" });
            DropIndex("dbo.GxProduct", new[] { "Type_Id" });
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
