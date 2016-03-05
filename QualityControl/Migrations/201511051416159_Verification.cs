using System.Data.Entity.Migrations;

namespace QualityControl.Migrations
{
    public partial class Verification : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Verification",
                c => new
                {
                    Id = c.Long(false, true),
                    Status = c.Int(false)
                })
                .PrimaryKey(t => t.Id);

            AddColumn("dbo.GxQrCodeInfo", "Verification_Id", c => c.Long());
            CreateIndex("dbo.GxQrCodeInfo", "Verification_Id");
            AddForeignKey("dbo.GxQrCodeInfo", "Verification_Id", "dbo.Verification", "Id");
        }

        public override void Down()
        {
            DropForeignKey("dbo.GxQrCodeInfo", "Verification_Id", "dbo.Verification");
            DropIndex("dbo.GxQrCodeInfo", new[] {"Verification_Id"});
            DropColumn("dbo.GxQrCodeInfo", "Verification_Id");
            DropTable("dbo.Verification");
        }
    }
}