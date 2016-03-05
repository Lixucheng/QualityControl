using System.Data.Entity.Migrations;

namespace QualityControl.Migrations
{
    public partial class ReportDectectionResultsChange : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.GxBatchReport", "DectectionItemResults", c => c.Boolean(false, true));
        }

        public override void Down()
        {
            AlterColumn("dbo.GxBatchReport", "DectectionItemResults", c => c.String());
        }
    }
}