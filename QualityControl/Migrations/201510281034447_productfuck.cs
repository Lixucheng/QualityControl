namespace QualityControl.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class productfuck : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.GxBaseProductBatch", "ProductionDate", c => c.String());
            AddColumn("dbo.GxProduct", "ShelfLife", c => c.String());
            AddColumn("dbo.GxProduct", "Brand", c => c.String());
            AddColumn("dbo.GxProduct", "Grade", c => c.String());
            AddColumn("dbo.GxProduct", "Weight", c => c.String());
            AddColumn("dbo.GxProduct", "Ingredients", c => c.String());
            AddColumn("dbo.GxProduct", "StorageCondition", c => c.String());
            AddColumn("dbo.GxProduct", "Packing", c => c.String());
            AddColumn("dbo.GxProduct", "PackingMaterial", c => c.String());
            AddColumn("dbo.GxProduct", "PermissionType", c => c.String());
            AddColumn("dbo.GxProduct", "Material", c => c.String());
            AddColumn("dbo.GxProduct", "Image", c => c.String());
            AddColumn("dbo.GxProduct", "File", c => c.String());
            AddColumn("dbo.GxProduct", "EmpowerEnterprise", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.GxProduct", "EmpowerEnterprise");
            DropColumn("dbo.GxProduct", "File");
            DropColumn("dbo.GxProduct", "Image");
            DropColumn("dbo.GxProduct", "Material");
            DropColumn("dbo.GxProduct", "PermissionType");
            DropColumn("dbo.GxProduct", "PackingMaterial");
            DropColumn("dbo.GxProduct", "Packing");
            DropColumn("dbo.GxProduct", "StorageCondition");
            DropColumn("dbo.GxProduct", "Ingredients");
            DropColumn("dbo.GxProduct", "Weight");
            DropColumn("dbo.GxProduct", "Grade");
            DropColumn("dbo.GxProduct", "Brand");
            DropColumn("dbo.GxProduct", "ShelfLife");
            DropColumn("dbo.GxBaseProductBatch", "ProductionDate");
        }
    }
}
