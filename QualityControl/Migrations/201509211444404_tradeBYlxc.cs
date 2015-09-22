namespace QualityControl.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class tradeBYlxc : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.GxTrade", "Product_Id", c => c.Long());
            CreateIndex("dbo.GxTrade", "Product_Id");
            AddForeignKey("dbo.GxTrade", "Product_Id", "dbo.GxProduct", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.GxTrade", "Product_Id", "dbo.GxProduct");
            DropIndex("dbo.GxTrade", new[] { "Product_Id" });
            DropColumn("dbo.GxTrade", "Product_Id");
        }
    }
}
