using System.Data.Entity.Migrations;

namespace QualityControl.Migrations
{
    public partial class userid : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.GxTrade", "Manufacturer_Id", "dbo.GxUser");
            DropForeignKey("dbo.GxTrade", "SgsUser_Id", "dbo.GxUser");
            DropIndex("dbo.GxTrade", new[] {"Manufacturer_Id"});
            DropIndex("dbo.GxTrade", new[] {"SgsUser_Id"});
            AddColumn("dbo.GxTrade", "ManufacturerId", c => c.String());
            AddColumn("dbo.GxTrade", "SgsUserId", c => c.String());
            DropColumn("dbo.GxTrade", "Manufacturer_Id");
            DropColumn("dbo.GxTrade", "SgsUser_Id");
        }

        public override void Down()
        {
            AddColumn("dbo.GxTrade", "SgsUser_Id", c => c.String(maxLength: 128));
            AddColumn("dbo.GxTrade", "Manufacturer_Id", c => c.String(maxLength: 128));
            DropColumn("dbo.GxTrade", "SgsUserId");
            DropColumn("dbo.GxTrade", "ManufacturerId");
            CreateIndex("dbo.GxTrade", "SgsUser_Id");
            CreateIndex("dbo.GxTrade", "Manufacturer_Id");
            AddForeignKey("dbo.GxTrade", "SgsUser_Id", "dbo.GxUser", "Id");
            AddForeignKey("dbo.GxTrade", "Manufacturer_Id", "dbo.GxUser", "Id");
        }
    }
}