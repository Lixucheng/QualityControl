namespace QualityControl.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ProductAddUpdateJson : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.GxProduct", "UpdateJson", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.GxProduct", "UpdateJson");
        }
    }
}
