using System.Data.Entity.Migrations;

namespace QualityControl.Migrations
{
    public partial class QrCodeInfo1 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.GxQrCodeInfo", "QrName", c => c.String());
            AlterColumn("dbo.GxQrCodeInfo", "IdCode", c => c.String());
        }

        public override void Down()
        {
            AlterColumn("dbo.GxQrCodeInfo", "IdCode", c => c.Long(false));
            AlterColumn("dbo.GxQrCodeInfo", "QrName", c => c.Long(false));
        }
    }
}