using System.Data.Entity.Migrations;

namespace QualityControl.Migrations
{
    public partial class TradeProductChange : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.GxTrade", "Product_Id", "dbo.GxProduct");
            DropIndex("dbo.GxTrade", new[] {"Product_Id"});
            AddColumn("dbo.GxTrade", "Product", c => c.String());
            DropColumn("dbo.GxTrade", "Product_Id");
        }

        public override void Down()
        {
            AddColumn("dbo.GxTrade", "Product_Id", c => c.Long());
            DropColumn("dbo.GxTrade", "Product");
            CreateIndex("dbo.GxTrade", "Product_Id");
            AddForeignKey("dbo.GxTrade", "Product_Id", "dbo.GxProduct", "Id");
        }
    }
}