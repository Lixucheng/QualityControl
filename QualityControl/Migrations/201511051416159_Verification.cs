namespace QualityControl.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Verification : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Verification",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        Status = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            AddColumn("dbo.GxQrCodeInfo", "Verification_Id", c => c.Long());
            CreateIndex("dbo.GxQrCodeInfo", "Verification_Id");
            AddForeignKey("dbo.GxQrCodeInfo", "Verification_Id", "dbo.Verification", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.GxQrCodeInfo", "Verification_Id", "dbo.Verification");
            DropIndex("dbo.GxQrCodeInfo", new[] { "Verification_Id" });
            DropColumn("dbo.GxQrCodeInfo", "Verification_Id");
            DropTable("dbo.Verification");
        }
    }
}
