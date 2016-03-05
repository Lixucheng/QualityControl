using System.Data.Entity.Migrations;

namespace QualityControl.Migrations
{
    public partial class Init : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.GxBaseProductBatch",
                c => new
                {
                    Id = c.Long(false, true),
                    ProductId = c.Long(false),
                    BatchName = c.String(),
                    Count = c.Int(false),
                    ProductionDate = c.DateTime(false)
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.GxProduct", t => t.ProductId, true)
                .Index(t => t.ProductId);

            CreateTable(
                "dbo.GxBatchReport",
                c => new
                {
                    Id = c.Long(false, true),
                    BatchId = c.Long(false),
                    SampleName = c.String(false),
                    Specification = c.String(false),
                    Trustor = c.String(false),
                    Brand = c.String(false),
                    TrustorAddress = c.String(false),
                    TestType = c.String(false),
                    ProducingAddress = c.String(false),
                    SampleRank = c.String(false),
                    Deliverer = c.String(false),
                    DeliveryDate = c.String(false),
                    ProducingDate = c.String(false),
                    Manager = c.String(false),
                    CheckDate = c.String(false),
                    ProjectName = c.String(false),
                    Standard = c.String(false),
                    Conclusion = c.String(false),
                    Equipment = c.String(),
                    Note = c.String(false)
                })
                .PrimaryKey(t => t.Id);

            CreateTable(
                "dbo.GxCompact",
                c => new
                {
                    Id = c.Long(false, true),
                    Content = c.String(),
                    FirstParty = c.String(),
                    SecondParty = c.String(),
                    Time = c.DateTime(false)
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
                    FactoryAddress = c.String(false),
                    Postcode = c.String(false),
                    Tel = c.String(false),
                    FAX = c.String(false),
                    Email = c.String(false),
                    EconomyType = c.String(false),
                    BRI = c.String(false),
                    EstablishedTime = c.DateTime(false),
                    License = c.String(false),
                    CorporationName = c.String(false),
                    OperationTerm = c.String(false),
                    RegisteredCapital = c.String(false),
                    GDP = c.String(),
                    AnnualSale = c.String(),
                    TaxPerYear = c.String(),
                    AnnualProfit = c.String(),
                    FixedAsset = c.String(false),
                    OrganizationCode = c.String(false),
                    Superintendent = c.String(false),
                    QA = c.String(false),
                    MemberCount = c.Int(false),
                    TechnicianCount = c.Int(false),
                    ContactPerson = c.String(false),
                    Status = c.Int(false),
                    UpdateJson = c.String(),
                    CreateTime = c.DateTime(false),
                    LastChangeTime = c.DateTime(false),
                    Note = c.String()
                })
                .PrimaryKey(t => t.Id);

            CreateTable(
                "dbo.GxProduct",
                c => new
                {
                    Id = c.Long(false, true),
                    Name = c.String(false),
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
                    Price = c.Single(false),
                    Status = c.Int(false),
                    UserId = c.String(),
                    ProductionCertificateNo = c.String(false),
                    GetDate = c.String(false),
                    Standard = c.String(false),
                    CreateTime = c.DateTime(false),
                    LastChangeTime = c.DateTime(false),
                    UpdateJson = c.String(),
                    Type_Id = c.Long(),
                    Company_Id = c.Long()
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
                    Id = c.Long(false, true),
                    Price = c.Single(false),
                    NeedeDay = c.Int(false),
                    Product_Id = c.Long(),
                    SGS_Id = c.Long()
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
                    Id = c.Long(false, true),
                    UserId = c.String(),
                    CreateTime = c.DateTime(false),
                    LastChangeTime = c.DateTime(false),
                    UpdateJson = c.String(),
                    Status = c.Int(false),
                    Name = c.String(false),
                    CorporationName = c.String(false),
                    Tel = c.String(false),
                    Address = c.String(false),
                    License = c.String(false),
                    Type = c.String()
                })
                .PrimaryKey(t => t.Id);

            CreateTable(
                "dbo.GxThirdProductType",
                c => new
                {
                    Id = c.Long(false, true),
                    Title = c.String(),
                    Description = c.String(),
                    SecondType_Id = c.Long()
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.GxSecondProductType", t => t.SecondType_Id)
                .Index(t => t.SecondType_Id);

            CreateTable(
                "dbo.GxSecondProductType",
                c => new
                {
                    Id = c.Long(false, true),
                    Title = c.String(),
                    Description = c.String(),
                    FirstType_Id = c.Long()
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.GxFirstProductType", t => t.FirstType_Id)
                .Index(t => t.FirstType_Id);

            CreateTable(
                "dbo.GxFirstProductType",
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
                    UserId = c.String(),
                    Modify = c.String(),
                    DetectionScheme_Id = c.Long()
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.GxDetectionScheme", t => t.DetectionScheme_Id)
                .Index(t => t.DetectionScheme_Id);

            CreateTable(
                "dbo.GxDetectionScheme",
                c => new
                {
                    Id = c.Long(false, true),
                    Level = c.String(),
                    MaxQuote = c.Double(false),
                    MinQuote = c.Double(false),
                    MaxTime = c.Int(false),
                    MinTime = c.Int(false),
                    UserQuote = c.Double(false),
                    OrganQuote = c.Double(false),
                    Time = c.Int(false),
                    Status = c.Int(false),
                    Trade_Id = c.Long()
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.GxTrade", t => t.Trade_Id)
                .Index(t => t.Trade_Id);

            CreateTable(
                "dbo.GxContract",
                c => new
                {
                    Id = c.Long(false, true),
                    Level = c.String(),
                    Quote = c.Double(false),
                    UserId = c.String(),
                    Time = c.Int(false),
                    Status = c.Int(false),
                    DetectionScheme_Id = c.Long(),
                    Product_Id = c.Long()
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
                    Id = c.Long(false, true),
                    SampleType = c.Int(false),
                    CeateTime = c.DateTime(false),
                    Status = c.Int(false),
                    FinishTime = c.DateTime(false),
                    ManufacturerId = c.String(),
                    SgsUserId = c.String(),
                    UserId = c.String(),
                    Product = c.String(),
                    SGSPaied = c.Boolean(false),
                    SamplingDate = c.DateTime(false),
                    DetectingDate = c.DateTime(false),
                    SampleRecevied = c.String(),
                    Files = c.String(),
                    Compact_Id = c.Long(),
                    Result_Id = c.Long()
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
                    Id = c.Long(false, true),
                    ProductionDate = c.DateTime(false),
                    ProductId = c.Long(false),
                    BatchName = c.String(),
                    Count = c.Int(false),
                    SampleCount = c.Int(false),
                    Level = c.String(),
                    SamplaListJson = c.String(),
                    Report_Id = c.Long(),
                    Trade_Id = c.Long()
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
                    Id = c.Long(false, true),
                    Status = c.Int(false),
                    Files = c.String(),
                    CreateTime = c.DateTime(false)
                })
                .PrimaryKey(t => t.Id);

            CreateTable(
                "dbo.CheckLevels",
                c => new
                {
                    Id = c.Int(false, true),
                    Min = c.Int(false),
                    Max = c.Int(false),
                    S1 = c.String(),
                    S2 = c.String(),
                    S3 = c.String(),
                    S4 = c.String(),
                    I = c.String(),
                    II = c.String(),
                    III = c.String()
                })
                .PrimaryKey(t => t.Id);

            CreateTable(
                "dbo.GxMessage",
                c => new
                {
                    Id = c.Long(false, true),
                    UserId = c.String(),
                    Content = c.String(),
                    Status = c.Int(false),
                    Time = c.DateTime(false)
                })
                .PrimaryKey(t => t.Id);

            CreateTable(
                "dbo.GxQrCodeInfo",
                c => new
                {
                    Id = c.Long(false, true),
                    QrName = c.String(),
                    IdCode = c.String()
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
            DropIndex("dbo.GxIdentityUserLogin", new[] {"UserId"});
            DropIndex("dbo.GxIdentityUserClaim", new[] {"UserId"});
            DropIndex("dbo.GxUser", "UserNameIndex");
            DropIndex("dbo.GxIdentityUserRole", new[] {"RoleId"});
            DropIndex("dbo.GxIdentityUserRole", new[] {"UserId"});
            DropIndex("dbo.GxIdentityRole", "RoleNameIndex");
            DropIndex("dbo.GxProductBatch", new[] {"Trade_Id"});
            DropIndex("dbo.GxProductBatch", new[] {"Report_Id"});
            DropIndex("dbo.GxTrade", new[] {"Result_Id"});
            DropIndex("dbo.GxTrade", new[] {"Compact_Id"});
            DropIndex("dbo.GxContract", new[] {"Product_Id"});
            DropIndex("dbo.GxContract", new[] {"DetectionScheme_Id"});
            DropIndex("dbo.GxDetectionScheme", new[] {"Trade_Id"});
            DropIndex("dbo.GxContractModification", new[] {"DetectionScheme_Id"});
            DropIndex("dbo.GxSecondProductType", new[] {"FirstType_Id"});
            DropIndex("dbo.GxThirdProductType", new[] {"SecondType_Id"});
            DropIndex("dbo.GxSgsProduct", new[] {"SGS_Id"});
            DropIndex("dbo.GxSgsProduct", new[] {"Product_Id"});
            DropIndex("dbo.GxProduct", new[] {"Company_Id"});
            DropIndex("dbo.GxProduct", new[] {"Type_Id"});
            DropIndex("dbo.GxBaseProductBatch", new[] {"ProductId"});
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