using System.Data.Entity.Migrations;

namespace QualityControl.Migrations
{
    public partial class count : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.GxTrade", "Count", c => c.Long(false));
        }

        public override void Down()
        {
            DropColumn("dbo.GxTrade", "Count");
        }
    }
}