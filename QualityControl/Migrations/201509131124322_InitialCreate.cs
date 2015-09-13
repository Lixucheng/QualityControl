namespace QualityControl.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Companies",
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
                        Postcode = c.String(),
                        OrganizationCode = c.String(nullable: false),
                        LicenseScope = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Products",
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
                .ForeignKey("dbo.ProductTypes", t => t.Type_Id)
                .ForeignKey("dbo.Companies", t => t.Company_Id)
                .Index(t => t.Type_Id)
                .Index(t => t.Company_Id);
            
            CreateTable(
                "dbo.ProductTypes",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        Title = c.String(),
                        Description = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.CompanyProducts",
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
                "dbo.Messages",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        UserId = c.String(),
                        Content = c.String(),
                        Status = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Products", "Company_Id", "dbo.Companies");
            DropForeignKey("dbo.Products", "Type_Id", "dbo.ProductTypes");
            DropIndex("dbo.Products", new[] { "Company_Id" });
            DropIndex("dbo.Products", new[] { "Type_Id" });
            DropTable("dbo.Messages");
            DropTable("dbo.CompanyProducts");
            DropTable("dbo.ProductTypes");
            DropTable("dbo.Products");
            DropTable("dbo.Companies");
        }
    }
}
