using System.Data.Entity.Migrations;

namespace QualityControl.Migrations
{
    public partial class compactup : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.GxCompact", "Id", "dbo.GxTrade");
            DropIndex("dbo.GxCompact", new[] {"Id"});
            DropPrimaryKey("dbo.GxCompact");
            AddColumn("dbo.GxTrade", "Compact_Id", c => c.Long());
            AlterColumn("dbo.GxCompact", "Id", c => c.Long(false, true));
            AddPrimaryKey("dbo.GxCompact", "Id");
            CreateIndex("dbo.GxTrade", "Compact_Id");
            AddForeignKey("dbo.GxTrade", "Compact_Id", "dbo.GxCompact", "Id");
        }

        public override void Down()
        {
            DropForeignKey("dbo.GxTrade", "Compact_Id", "dbo.GxCompact");
            DropIndex("dbo.GxTrade", new[] {"Compact_Id"});
            DropPrimaryKey("dbo.GxCompact");
            AlterColumn("dbo.GxCompact", "Id", c => c.Long(false));
            DropColumn("dbo.GxTrade", "Compact_Id");
            AddPrimaryKey("dbo.GxCompact", "Id");
            CreateIndex("dbo.GxCompact", "Id");
            AddForeignKey("dbo.GxCompact", "Id", "dbo.GxTrade", "Id");
        }
    }
}