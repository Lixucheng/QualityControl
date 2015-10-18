using System.Data.Entity.Migrations;

namespace QualityControl.Migrations
{
    public partial class QrCodeInfo : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.GxQrCodeInfo",
                c => new
                {
                    Id = c.Long(false, true),
                    QrName = c.Long(false),
                    IdCode = c.Long(false)
                })
                .PrimaryKey(t => t.Id);
        }

        public override void Down()
        {
            DropTable("dbo.GxQrCodeInfo");
        }
    }
}