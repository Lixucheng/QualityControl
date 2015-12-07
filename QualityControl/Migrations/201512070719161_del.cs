namespace QualityControl.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class del : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.GxUser", "Del", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.GxUser", "Del");
        }
    }
}
