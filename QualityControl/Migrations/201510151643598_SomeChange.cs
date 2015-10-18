using System.Data.Entity.Migrations;

namespace QualityControl.Migrations
{
    public partial class SomeChange : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.GxDetectionReport", "DataList", c => c.String(false));
            AlterColumn("dbo.GxDetectionReport", "Conclusion", c => c.String(false));
            AlterColumn("dbo.GxDetectionReport", "Comments", c => c.String(false));
        }

        public override void Down()
        {
            AlterColumn("dbo.GxDetectionReport", "Comments", c => c.String());
            AlterColumn("dbo.GxDetectionReport", "Conclusion", c => c.String());
            AlterColumn("dbo.GxDetectionReport", "DataList", c => c.String());
        }
    }
}