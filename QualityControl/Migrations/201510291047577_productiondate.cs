namespace QualityControl.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class productiondate : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.GxProductBatch", "ProductionDate", c => c.DateTime(nullable: false));
            AlterColumn("dbo.GxBaseProductBatch", "ProductionDate", c => c.DateTime(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.GxBaseProductBatch", "ProductionDate", c => c.String());
            DropColumn("dbo.GxProductBatch", "ProductionDate");
        }
    }
}
