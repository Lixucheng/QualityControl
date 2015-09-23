namespace QualityControl.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class adduserid : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.CheckLevels",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Min = c.Int(nullable: false),
                        Max = c.Int(nullable: false),
                        S1 = c.String(),
                        S2 = c.String(),
                        S3 = c.String(),
                        S4 = c.String(),
                        I = c.String(),
                        II = c.String(),
                        III = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            AddColumn("dbo.GxTrade", "User_Id", c => c.String(maxLength: 128));
            CreateIndex("dbo.GxTrade", "User_Id");
            AddForeignKey("dbo.GxTrade", "User_Id", "dbo.GxUser", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.GxTrade", "User_Id", "dbo.GxUser");
            DropIndex("dbo.GxTrade", new[] { "User_Id" });
            DropColumn("dbo.GxTrade", "User_Id");
            DropTable("dbo.CheckLevels");
        }
    }
}
