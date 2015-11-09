namespace QualityControl.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class DectectionItem : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.GxDectectionItem",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        Name = c.String(nullable: false),
                        Days = c.Int(nullable: false),
                        Price = c.Single(nullable: false),
                        SGS_Id = c.Long(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.GxSGS", t => t.SGS_Id)
                .Index(t => t.SGS_Id);
            
            AddColumn("dbo.GxSGS", "DectectionItemString", c => c.String());
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.GxDectectionItem", "SGS_Id", "dbo.GxSGS");
            DropIndex("dbo.GxDectectionItem", new[] { "SGS_Id" });
            DropColumn("dbo.GxSGS", "DectectionItemString");
            DropTable("dbo.GxDectectionItem");
        }
    }
}
