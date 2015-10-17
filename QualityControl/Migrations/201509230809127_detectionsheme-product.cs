using System.Data.Entity.Migrations;

namespace QualityControl.Migrations
{
    public partial class detectionshemeproduct : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.GxDetectionScheme", "Product_Id", "dbo.GxProduct");
            DropIndex("dbo.GxDetectionScheme", new[] {"Product_Id"});
            DropColumn("dbo.GxDetectionScheme", "Product_Id");
        }

        public override void Down()
        {
            AddColumn("dbo.GxDetectionScheme", "Product_Id", c => c.Long());
            CreateIndex("dbo.GxDetectionScheme", "Product_Id");
            AddForeignKey("dbo.GxDetectionScheme", "Product_Id", "dbo.GxProduct", "Id");
        }
    }
}