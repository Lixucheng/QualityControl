using System.Data.Entity.Migrations;

namespace QualityControl.Migrations
{
    public partial class Verification11 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.GxQrCodeInfo", "TradeId", c => c.Long(false));
        }

        public override void Down()
        {
            DropColumn("dbo.GxQrCodeInfo", "TradeId");
        }
    }
}