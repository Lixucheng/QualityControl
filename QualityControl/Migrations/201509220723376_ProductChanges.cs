namespace QualityControl.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ProductChanges : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.GxProduct", "CreateTime", c => c.DateTime(nullable: false));
            AddColumn("dbo.GxProduct", "LastChangeTime", c => c.DateTime(nullable: false));
            AlterColumn("dbo.GxProduct", "Name", c => c.String(nullable: false));
            AlterColumn("dbo.GxProduct", "ProductionCertificateNo", c => c.String(nullable: false));
            AlterColumn("dbo.GxProduct", "GetDate", c => c.String(nullable: false));
            AlterColumn("dbo.GxProduct", "Standard", c => c.String(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.GxProduct", "Standard", c => c.String());
            AlterColumn("dbo.GxProduct", "GetDate", c => c.String());
            AlterColumn("dbo.GxProduct", "ProductionCertificateNo", c => c.String());
            AlterColumn("dbo.GxProduct", "Name", c => c.String());
            DropColumn("dbo.GxProduct", "LastChangeTime");
            DropColumn("dbo.GxProduct", "CreateTime");
        }
    }
}
