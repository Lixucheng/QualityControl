namespace QualityControl.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class SGS : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.GxSGS",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        UserId = c.String(),
                        CreateTime = c.DateTime(nullable: false),
                        LastChangeTime = c.DateTime(nullable: false),
                        UpdateJson = c.String(),
                        Status = c.Int(nullable: false),
                        Name = c.String(nullable: false),
                        CorporationName = c.String(nullable: false),
                        Tel = c.String(nullable: false),
                        Address = c.String(nullable: false),
                        License = c.String(nullable: false),
                        Type = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.GxSGS");
        }
    }
}
