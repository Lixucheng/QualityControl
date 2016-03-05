using System.Data.Entity.Migrations;

namespace QualityControl.Migrations
{
    public partial class sgsitems : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.GxTrade", "RealDetectionTtems", c => c.String());
        }

        public override void Down()
        {
            DropColumn("dbo.GxTrade", "RealDetectionTtems");
        }
    }
}