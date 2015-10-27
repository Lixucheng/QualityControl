namespace QualityControl.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class level : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.GxProductBatch", "Level", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.GxProductBatch", "Level");
        }
    }
}
