namespace QualityControl.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Init : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.GxBaseProductBatch",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        ProductId = c.Long(nullable: false),
                        BatchName = c.String(),
                        Count = c.Int(nullable: false),
                        ProductionDate = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.GxProduct", t => t.ProductId, cascadeDelete: true)
                .Index(t => t.ProductId);
            
            CreateTable(
                "dbo.GxBatchReport",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        BatchId = c.Long(nullable: false),
                        SampleName = c.String(nullable: false),
                        Specification = c.String(nullable: false),
                        Trustor = c.String(nullable: false),
                        Brand = c.String(nullable: false),
                        TrustorAddress = c.String(nullable: false),
                        TestType = c.String(nullable: false),
                        ProducingAddress = c.String(nullable: false),
                        SampleRank = c.String(nullable: false),
                        Deliverer = c.String(nullable: false),
                        DeliveryDate = c.String(nullable: false),
                        ProducingDate = c.String(nullable: false),
                        Manager = c.String(nullable: false),
                        CheckDate = c.String(nullable: false),
                        ProjectName = c.String(nullable: false),
                        Standard = c.String(nullable: false),
                        Conclusion = c.String(nullable: false),
                        Equipment = c.String(),
                        Note = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.GxCompact",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        Content = c.String(),
                        FirstParty = c.String(),
                        SecondParty = c.String(),
                        Time = c.DateTime(nullable: false),
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
                        FactoryAddress = c.String(nullable: false),
                        Postcode = c.String(nullable: false),
                        Tel = c.String(nullable: false),
                        FAX = c.String(nullable: false),
                        Email = c.String(nullable: false),
                        EconomyType = c.String(nullable: false),
                        BRI = c.String(nullable: false),
                        EstablishedTime = c.DateTime(nullable: false),
                        License = c.String(nullable: false),
                        CorporationName = c.String(nullable: false),
                        OperationTerm = c.String(nullable: false),
                        RegisteredCapital = c.String(nullable: false),
                        GDP = c.String(),
                        AnnualSale = c.String(),
                        TaxPerYear = c.String(),
                        AnnualProfit = c.String(),
                        FixedAsset = c.String(nullable: false),
                        OrganizationCode = c.String(nullable: false),
                        Superintendent = c.String(nullable: false),
                        QA = c.String(nullable: false),
                        MemberCount = c.Int(nullable: false),
                        TechnicianCount = c.Int(nullable: false),
                        ContactPerson = c.String(nullable: false),
                        Status = c.Int(nullable: false),
                        UpdateJson = c.String(),
                        CreateTime = c.DateTime(nullable: false),
                        LastChangeTime = c.DateTime(nullable: false),
                        Note = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.GxProduct",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        Name = c.String(nullable: false),
                        ShelfLife = c.String(),
                        Brand = c.String(),
                        Grade = c.String(),
                        Weight = c.String(),
                        Ingredients = c.String(),
                        StorageCondition = c.String(),
                        Packing = c.String(),
                        PackingMaterial = c.String(),
                        PermissionType = c.String(),
                        Material = c.String(),
                        Image = c.String(),
                        File = c.String(),
                        EmpowerEnterprise = c.String(),
                        Price = c.Single(nullable: false),
                        Status = c.Int(nullable: false),
                        UserId = c.String(),
                        ProductionCertificateNo = c.String(nullable: false),
                        GetDate = c.String(nullable: false),
                        Standard = c.String(nullable: false),
                        CreateTime = c.DateTime(nullable: false),
                        LastChangeTime = c.DateTime(nullable: false),
                        UpdateJson = c.String(),
                        Type_Id = c.Long(),
                        Company_Id = c.Long(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.GxThirdProductType", t => t.Type_Id)
                .ForeignKey("dbo.GxCompany", t => t.Company_Id)
                .Index(t => t.Type_Id)
                .Index(t => t.Company_Id);
            
            CreateTable(
                "dbo.GxSgsProduct",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        Price = c.Single(nullable: false),
                        NeedeDay = c.Int(nullable: false),
                        Product_Id = c.Long(),
                        SGS_Id = c.Long(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.GxProduct", t => t.Product_Id)
                .ForeignKey("dbo.GxSGS", t => t.SGS_Id)
                .Index(t => t.Product_Id)
                .Index(t => t.SGS_Id);
            
            CreateTable(
                "dbo.GxSGS",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        UserId = c.String(),
                        CreateTime = c.DateTime(nullable: false),
                        LastChangeTime = c.DateTime(nullable: false),
                        UpdateJson = c.String(),
                        Status = c.Int(nullable: false),
                        Name = c.String(nullable: false),
                        CorporationName = c.String(nullable: false),
                        Tel = c.String(nullable: false),
                        Address = c.String(nullable: false),
                        License = c.String(nullable: false),
                        Type = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.GxThirdProductType",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        Title = c.String(),
                        Description = c.String(),
                        SecondType_Id = c.Long(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.GxSecondProductType", t => t.SecondType_Id)
                .Index(t => t.SecondType_Id);
            
            CreateTable(
                "dbo.GxSecondProductType",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        Title = c.String(),
                        Description = c.String(),
                        FirstType_Id = c.Long(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.GxFirstProductType", t => t.FirstType_Id)
                .Index(t => t.FirstType_Id);
            
            CreateTable(
                "dbo.GxFirstProductType",
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
                        UserId = c.String(),
                        Modify = c.String(),
                        DetectionScheme_Id = c.Long(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.GxDetectionScheme", t => t.DetectionScheme_Id)
                .Index(t => t.DetectionScheme_Id);
            
            CreateTable(
                "dbo.GxDetectionScheme",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        Level = c.String(),
                        MaxQuote = c.Double(nullable: false),
                        MinQuote = c.Double(nullable: false),
                        MaxTime = c.Int(nullable: false),
                        MinTime = c.Int(nullable: false),
                        UserQuote = c.Double(nullable: false),
                        OrganQuote = c.Double(nullable: false),
                        Time = c.Int(nullable: false),
                        Status = c.Int(nullable: false),
                        Trade_Id = c.Long(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.GxTrade", t => t.Trade_Id)
                .Index(t => t.Trade_Id);
            
            CreateTable(
                "dbo.GxContract",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        Level = c.String(),
                        Quote = c.Double(nullable: false),
                        UserId = c.String(),
                        Time = c.Int(nullable: false),
                        Status = c.Int(nullable: false),
                        DetectionScheme_Id = c.Long(),
                        Product_Id = c.Long(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.GxDetectionScheme", t => t.DetectionScheme_Id)
                .ForeignKey("dbo.GxProduct", t => t.Product_Id)
                .Index(t => t.DetectionScheme_Id)
                .Index(t => t.Product_Id);
            
            CreateTable(
                "dbo.GxTrade",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        SampleType = c.Int(nullable: false),
                        CeateTime = c.DateTime(nullable: false),
                        Status = c.Int(nullable: false),
                        FinishTime = c.DateTime(nullable: false),
                        ManufacturerId = c.String(),
                        SgsUserId = c.String(),
                        UserId = c.String(),
                        Product = c.String(),
                        SGSPaied = c.Boolean(nullable: false),
                        SamplingDate = c.DateTime(nullable: false),
                        DetectingDate = c.DateTime(nullable: false),
                        SampleRecevied = c.String(),
                        Files = c.String(),
                        Compact_Id = c.Long(),
                        Result_Id = c.Long(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.GxCompact", t => t.Compact_Id)
                .ForeignKey("dbo.GxDetectionReport", t => t.Result_Id)
                .Index(t => t.Compact_Id)
                .Index(t => t.Result_Id);
            
            CreateTable(
                "dbo.GxProductBatch",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        ProductionDate = c.DateTime(nullable: false),
                        ProductId = c.Long(nullable: false),
                        BatchName = c.String(),
                        Count = c.Int(nullable: false),
                        SampleCount = c.Int(nullable: false),
                        Level = c.String(),
                        SamplaListJson = c.String(),
                        Report_Id = c.Long(),
                        Trade_Id = c.Long(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.GxBatchReport", t => t.Report_Id)
                .ForeignKey("dbo.GxTrade", t => t.Trade_Id)
                .Index(t => t.Report_Id)
                .Index(t => t.Trade_Id);
            
            CreateTable(
                "dbo.GxDetectionReport",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        Status = c.Int(nullable: false),
                        Files = c.String(),
                        CreateTime = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.CheckLevels",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Min = c.Int(nullable: false),
                        Max = c.Int(nullable: false),
                        S1 = c.String(),
                        S2 = c.String(),
                        S3 = c.String(),
                        S4 = c.String(),
                        I = c.String(),
                        II = c.String(),
                        III = c.String(),
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
                        Time = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.GxQrCodeInfo",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        QrName = c.String(),
                        IdCode = c.String(),
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
            DropForeignKey("dbo.GxDetectionScheme", "Trade_Id", "dbo.GxTrade");
            DropForeignKey("dbo.GxTrade", "Result_Id", "dbo.GxDetectionReport");
            DropForeignKey("dbo.GxTrade", "Compact_Id", "dbo.GxCompact");
            DropForeignKey("dbo.GxProductBatch", "Trade_Id", "dbo.GxTrade");
            DropForeignKey("dbo.GxProductBatch", "Report_Id", "dbo.GxBatchReport");
            DropForeignKey("dbo.GxContractModification", "DetectionScheme_Id", "dbo.GxDetectionScheme");
            DropForeignKey("dbo.GxContract", "Product_Id", "dbo.GxProduct");
            DropForeignKey("dbo.GxContract", "DetectionScheme_Id", "dbo.GxDetectionScheme");
            DropForeignKey("dbo.GxProduct", "Company_Id", "dbo.GxCompany");
            DropForeignKey("dbo.GxThirdProductType", "SecondType_Id", "dbo.GxSecondProductType");
            DropForeignKey("dbo.GxSecondProductType", "FirstType_Id", "dbo.GxFirstProductType");
            DropForeignKey("dbo.GxProduct", "Type_Id", "dbo.GxThirdProductType");
            DropForeignKey("dbo.GxSgsProduct", "SGS_Id", "dbo.GxSGS");
            DropForeignKey("dbo.GxSgsProduct", "Product_Id", "dbo.GxProduct");
            DropForeignKey("dbo.GxBaseProductBatch", "ProductId", "dbo.GxProduct");
            DropIndex("dbo.GxIdentityUserLogin", new[] { "UserId" });
            DropIndex("dbo.GxIdentityUserClaim", new[] { "UserId" });
            DropIndex("dbo.GxUser", "UserNameIndex");
            DropIndex("dbo.GxIdentityUserRole", new[] { "RoleId" });
            DropIndex("dbo.GxIdentityUserRole", new[] { "UserId" });
            DropIndex("dbo.GxIdentityRole", "RoleNameIndex");
            DropIndex("dbo.GxProductBatch", new[] { "Trade_Id" });
            DropIndex("dbo.GxProductBatch", new[] { "Report_Id" });
            DropIndex("dbo.GxTrade", new[] { "Result_Id" });
            DropIndex("dbo.GxTrade", new[] { "Compact_Id" });
            DropIndex("dbo.GxContract", new[] { "Product_Id" });
            DropIndex("dbo.GxContract", new[] { "DetectionScheme_Id" });
            DropIndex("dbo.GxDetectionScheme", new[] { "Trade_Id" });
            DropIndex("dbo.GxContractModification", new[] { "DetectionScheme_Id" });
            DropIndex("dbo.GxSecondProductType", new[] { "FirstType_Id" });
            DropIndex("dbo.GxThirdProductType", new[] { "SecondType_Id" });
            DropIndex("dbo.GxSgsProduct", new[] { "SGS_Id" });
            DropIndex("dbo.GxSgsProduct", new[] { "Product_Id" });
            DropIndex("dbo.GxProduct", new[] { "Company_Id" });
            DropIndex("dbo.GxProduct", new[] { "Type_Id" });
            DropIndex("dbo.GxBaseProductBatch", new[] { "ProductId" });
            DropTable("dbo.GxIdentityUserLogin");
            DropTable("dbo.GxIdentityUserClaim");
            DropTable("dbo.GxUser");
            DropTable("dbo.GxIdentityUserRole");
            DropTable("dbo.GxIdentityRole");
            DropTable("dbo.GxQrCodeInfo");
            DropTable("dbo.GxMessage");
            DropTable("dbo.CheckLevels");
            DropTable("dbo.GxDetectionReport");
            DropTable("dbo.GxProductBatch");
            DropTable("dbo.GxTrade");
            DropTable("dbo.GxContract");
            DropTable("dbo.GxDetectionScheme");
            DropTable("dbo.GxContractModification");
            DropTable("dbo.GxCompanyProduct");
            DropTable("dbo.GxFirstProductType");
            DropTable("dbo.GxSecondProductType");
            DropTable("dbo.GxThirdProductType");
            DropTable("dbo.GxSGS");
            DropTable("dbo.GxSgsProduct");
            DropTable("dbo.GxProduct");
            DropTable("dbo.GxCompany");
            DropTable("dbo.GxCompact");
            DropTable("dbo.GxBatchReport");
            DropTable("dbo.GxBaseProductBatch");
        }
    }
}
