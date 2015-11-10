namespace QualityControl.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ReportDectectionResultsChange : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.GxBatchReport", "DectectionItemResults", c => c.Boolean(nullable: false, defaultValue: true));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.GxBatchReport", "DectectionItemResults", c => c.String());
        }
    }
}
