using System.Data.Entity.Migrations;

namespace QualityControl.Migrations
{
    public partial class SGS : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.GxSGS",
                c => new
                {
                    Id = c.Long(false, true),
                    UserId = c.String(),
                    CreateTime = c.DateTime(false),
                    LastChangeTime = c.DateTime(false),
                    UpdateJson = c.String(),
                    Status = c.Int(false),
                    Name = c.String(false),
                    CorporationName = c.String(false),
                    Tel = c.String(false),
                    Address = c.String(false),
                    License = c.String(false),
                    Type = c.String()
                })
                .PrimaryKey(t => t.Id);
        }

        public override void Down()
        {
            DropTable("dbo.GxSGS");
        }
    }
}