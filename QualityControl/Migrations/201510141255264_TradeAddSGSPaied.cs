using System.Data.Entity.Migrations;

namespace QualityControl.Migrations
{
    public partial class TradeAddSGSPaied : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.GxTrade", "SGSPaied", c => c.Boolean(false));
        }

        public override void Down()
        {
            DropColumn("dbo.GxTrade", "SGSPaied");
        }
    }
}