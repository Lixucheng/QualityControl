using System.Data.Entity.Migrations;

namespace QualityControl.Migrations
{
    public partial class AddMessageTime : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.GxMessage", "Time", c => c.DateTime(false));
        }

        public override void Down()
        {
            DropColumn("dbo.GxMessage", "Time");
        }
    }
}