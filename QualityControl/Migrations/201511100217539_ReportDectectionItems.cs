namespace QualityControl.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ReportDectectionItems : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.GxBatchReport", "DectectionItems", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.GxBatchReport", "DectectionItems");
        }
    }
}
