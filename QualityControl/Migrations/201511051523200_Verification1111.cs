namespace QualityControl.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Verification1111 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.GxQrCodeInfo", "Verification_Id", "dbo.Verification");
            DropIndex("dbo.GxQrCodeInfo", new[] { "Verification_Id" });
            AddColumn("dbo.Verification", "QrCodeInfo_Id", c => c.Long());
            CreateIndex("dbo.Verification", "QrCodeInfo_Id");
            AddForeignKey("dbo.Verification", "QrCodeInfo_Id", "dbo.GxQrCodeInfo", "Id");
            DropColumn("dbo.GxQrCodeInfo", "Verification_Id");
        }
        
        public override void Down()
        {
            AddColumn("dbo.GxQrCodeInfo", "Verification_Id", c => c.Long());
            DropForeignKey("dbo.Verification", "QrCodeInfo_Id", "dbo.GxQrCodeInfo");
            DropIndex("dbo.Verification", new[] { "QrCodeInfo_Id" });
            DropColumn("dbo.Verification", "QrCodeInfo_Id");
            CreateIndex("dbo.GxQrCodeInfo", "Verification_Id");
            AddForeignKey("dbo.GxQrCodeInfo", "Verification_Id", "dbo.Verification", "Id");
        }
    }
}
