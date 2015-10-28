namespace QualityControl.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class SCSInfoChange : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.GxCompany", "FactoryAddress", c => c.String(nullable: false));
            AddColumn("dbo.GxCompany", "Postcode", c => c.String(nullable: false));
            AddColumn("dbo.GxCompany", "Tel", c => c.String(nullable: false));
            AddColumn("dbo.GxCompany", "FAX", c => c.String(nullable: false));
            AddColumn("dbo.GxCompany", "Email", c => c.String(nullable: false));
            AddColumn("dbo.GxCompany", "EconomyType", c => c.String(nullable: false));
            AddColumn("dbo.GxCompany", "BRI", c => c.String(nullable: false));
            AddColumn("dbo.GxCompany", "OperationTerm", c => c.String(nullable: false));
            AddColumn("dbo.GxCompany", "RegisteredCapital", c => c.String(nullable: false));
            AddColumn("dbo.GxCompany", "GDP", c => c.String());
            AddColumn("dbo.GxCompany", "AnnualSale", c => c.String());
            AddColumn("dbo.GxCompany", "TaxPerYear", c => c.String());
            AddColumn("dbo.GxCompany", "AnnualProfit", c => c.String());
            AddColumn("dbo.GxCompany", "FixedAsset", c => c.String(nullable: false));
            AddColumn("dbo.GxCompany", "Superintendent", c => c.String(nullable: false));
            AddColumn("dbo.GxCompany", "QA", c => c.String(nullable: false));
            AddColumn("dbo.GxCompany", "MemberCount", c => c.Int(nullable: false));
            AddColumn("dbo.GxCompany", "TechnicianCount", c => c.Int(nullable: false));
            AddColumn("dbo.GxCompany", "ContactPerson", c => c.String(nullable: false));
            AddColumn("dbo.GxCompany", "Note", c => c.String());
            DropColumn("dbo.GxCompany", "CorporationIdentity");
            DropColumn("dbo.GxCompany", "LicenseScope");
        }
        
        public override void Down()
        {
            AddColumn("dbo.GxCompany", "LicenseScope", c => c.String(nullable: false));
            AddColumn("dbo.GxCompany", "CorporationIdentity", c => c.String(nullable: false));
            DropColumn("dbo.GxCompany", "Note");
            DropColumn("dbo.GxCompany", "ContactPerson");
            DropColumn("dbo.GxCompany", "TechnicianCount");
            DropColumn("dbo.GxCompany", "MemberCount");
            DropColumn("dbo.GxCompany", "QA");
            DropColumn("dbo.GxCompany", "Superintendent");
            DropColumn("dbo.GxCompany", "FixedAsset");
            DropColumn("dbo.GxCompany", "AnnualProfit");
            DropColumn("dbo.GxCompany", "TaxPerYear");
            DropColumn("dbo.GxCompany", "AnnualSale");
            DropColumn("dbo.GxCompany", "GDP");
            DropColumn("dbo.GxCompany", "RegisteredCapital");
            DropColumn("dbo.GxCompany", "OperationTerm");
            DropColumn("dbo.GxCompany", "BRI");
            DropColumn("dbo.GxCompany", "EconomyType");
            DropColumn("dbo.GxCompany", "Email");
            DropColumn("dbo.GxCompany", "FAX");
            DropColumn("dbo.GxCompany", "Tel");
            DropColumn("dbo.GxCompany", "Postcode");
            DropColumn("dbo.GxCompany", "FactoryAddress");
        }
    }
}
