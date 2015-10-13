namespace QualityControl.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Compactkey : DbMigration
    {
        public override void Up()
        {
            DropIndex("dbo.GxCompact", new[] { "Id" });
            DropPrimaryKey("dbo.GxCompact");
            AlterColumn("dbo.GxCompact", "Id", c => c.Long(nullable: false));
            AddPrimaryKey("dbo.GxCompact", "Id");
            CreateIndex("dbo.GxCompact", "Id");
        }
        
        public override void Down()
        {
            DropIndex("dbo.GxCompact", new[] { "Id" });
            DropPrimaryKey("dbo.GxCompact");
            AlterColumn("dbo.GxCompact", "Id", c => c.Long(nullable: false, identity: true));
            AddPrimaryKey("dbo.GxCompact", "Id");
            CreateIndex("dbo.GxCompact", "Id");
        }
    }
}
