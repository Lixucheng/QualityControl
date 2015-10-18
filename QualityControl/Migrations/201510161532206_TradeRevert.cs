using System.Data.Entity.Migrations;

namespace QualityControl.Migrations
{
    public partial class TradeRevert : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.GxTrade", "CompanyUserId");
        }

        public override void Down()
        {
            AddColumn("dbo.GxTrade", "CompanyUserId", c => c.String());
        }
    }
}