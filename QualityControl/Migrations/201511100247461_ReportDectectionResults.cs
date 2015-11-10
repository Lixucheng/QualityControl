namespace QualityControl.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ReportDectectionResults : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.GxBatchReport", "DectectionItemResults", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.GxBatchReport", "DectectionItemResults");
        }
    }
}
