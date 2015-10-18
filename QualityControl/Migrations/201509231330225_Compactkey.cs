using System.Data.Entity.Migrations;

namespace QualityControl.Migrations
{
    public partial class Compactkey : DbMigration
    {
        public override void Up()
        {
            DropIndex("dbo.GxCompact", new[] {"Id"});
            DropPrimaryKey("dbo.GxCompact");
            AlterColumn("dbo.GxCompact", "Id", c => c.Long(false));
            AddPrimaryKey("dbo.GxCompact", "Id");
            CreateIndex("dbo.GxCompact", "Id");
        }

        public override void Down()
        {
            DropIndex("dbo.GxCompact", new[] {"Id"});
            DropPrimaryKey("dbo.GxCompact");
            AlterColumn("dbo.GxCompact", "Id", c => c.Long(false, true));
            AddPrimaryKey("dbo.GxCompact", "Id");
            CreateIndex("dbo.GxCompact", "Id");
        }
    }
}