namespace QualityControl.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class SomeChange : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.GxDetectionReport", "DataList", c => c.String(nullable: false));
            AlterColumn("dbo.GxDetectionReport", "Conclusion", c => c.String(nullable: false));
            AlterColumn("dbo.GxDetectionReport", "Comments", c => c.String(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.GxDetectionReport", "Comments", c => c.String());
            AlterColumn("dbo.GxDetectionReport", "Conclusion", c => c.String());
            AlterColumn("dbo.GxDetectionReport", "DataList", c => c.String());
        }
    }
}
