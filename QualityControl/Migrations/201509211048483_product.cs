using System.Data.Entity.Migrations;

namespace QualityControl.Migrations
{
    public partial class product : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.GxProduct", "ProductionCertificateNo", c => c.String());
            AddColumn("dbo.GxProduct", "GetDate", c => c.String());
            AddColumn("dbo.GxProduct", "Standard", c => c.String());
        }

        public override void Down()
        {
            DropColumn("dbo.GxProduct", "Standard");
            DropColumn("dbo.GxProduct", "GetDate");
            DropColumn("dbo.GxProduct", "ProductionCertificateNo");
        }
    }
}