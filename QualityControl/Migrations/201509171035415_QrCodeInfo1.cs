namespace QualityControl.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class QrCodeInfo1 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.GxQrCodeInfo", "QrName", c => c.String());
            AlterColumn("dbo.GxQrCodeInfo", "IdCode", c => c.String());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.GxQrCodeInfo", "IdCode", c => c.Long(nullable: false));
            AlterColumn("dbo.GxQrCodeInfo", "QrName", c => c.Long(nullable: false));
        }
    }
}
