using System.Data.Entity.Migrations;

namespace QualityControl.Migrations
{
    public partial class del : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.GxUser", "Del", c => c.Int(false));
        }

        public override void Down()
        {
            DropColumn("dbo.GxUser", "Del");
        }
    }
}