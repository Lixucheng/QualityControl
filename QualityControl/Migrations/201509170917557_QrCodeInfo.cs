namespace QualityControl.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class QrCodeInfo : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.GxQrCodeInfo",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        QrName = c.Long(nullable: false),
                        IdCode = c.Long(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.GxQrCodeInfo");
        }
    }
}
