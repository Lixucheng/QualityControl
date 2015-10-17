using System.Data.Entity.Migrations;

namespace QualityControl.Migrations
{
    public partial class types : DbMigration
    {
        public override void Up()
        {
            RenameTable("dbo.GxProductType", "GxThirdProductType");
            CreateTable(
                "dbo.GxFirstProductType",
                c => new
                {
                    Id = c.Long(false, true),
                    Title = c.String(),
                    Description = c.String()
                })
                .PrimaryKey(t => t.Id);

            CreateTable(
                "dbo.GxSecondProductType",
                c => new
                {
                    Id = c.Long(false, true),
                    Title = c.String(),
                    Description = c.String(),
                    FirstProductType_Id = c.Long()
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.GxFirstProductType", t => t.FirstProductType_Id)
                .Index(t => t.FirstProductType_Id);

            AddColumn("dbo.GxThirdProductType", "SecondProductType_Id", c => c.Long());
            CreateIndex("dbo.GxThirdProductType", "SecondProductType_Id");
            AddForeignKey("dbo.GxThirdProductType", "SecondProductType_Id", "dbo.GxSecondProductType", "Id");
        }

        public override void Down()
        {
            DropForeignKey("dbo.GxSecondProductType", "FirstProductType_Id", "dbo.GxFirstProductType");
            DropForeignKey("dbo.GxThirdProductType", "SecondProductType_Id", "dbo.GxSecondProductType");
            DropIndex("dbo.GxSecondProductType", new[] {"FirstProductType_Id"});
            DropIndex("dbo.GxThirdProductType", new[] {"SecondProductType_Id"});
            DropColumn("dbo.GxThirdProductType", "SecondProductType_Id");
            DropTable("dbo.GxSecondProductType");
            DropTable("dbo.GxFirstProductType");
            RenameTable("dbo.GxThirdProductType", "GxProductType");
        }
    }
}