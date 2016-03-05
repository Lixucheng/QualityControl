using System.Data.Entity.Migrations;

namespace QualityControl.Migrations
{
    public partial class Verification1 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Verification", "TradeId", c => c.Long(false));
        }

        public override void Down()
        {
            DropColumn("dbo.Verification", "TradeId");
        }
    }
}