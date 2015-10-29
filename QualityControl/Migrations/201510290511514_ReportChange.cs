namespace QualityControl.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ReportChange : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.GxBatchReport",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        BatchId = c.Long(nullable: false),
                        SampleName = c.String(nullable: false),
                        Specification = c.String(nullable: false),
                        Trustor = c.String(nullable: false),
                        Brand = c.String(nullable: false),
                        TrustorAddress = c.String(nullable: false),
                        TestType = c.String(nullable: false),
                        ProducingAddress = c.String(nullable: false),
                        SampleRank = c.String(nullable: false),
                        Deliverer = c.String(nullable: false),
                        DeliveryDate = c.String(nullable: false),
                        ProducingDate = c.String(nullable: false),
                        Manager = c.String(nullable: false),
                        CheckDate = c.String(nullable: false),
                        ProjectName = c.String(nullable: false),
                        Standard = c.String(nullable: false),
                        Conclusion = c.String(nullable: false),
                        Equipment = c.String(),
                        Note = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            AddColumn("dbo.GxProductBatch", "Report_Id", c => c.Long());
            AddColumn("dbo.GxDetectionReport", "Files", c => c.String());
            AddColumn("dbo.GxDetectionReport", "CreateTime", c => c.DateTime(nullable: false));
            CreateIndex("dbo.GxProductBatch", "Report_Id");
            AddForeignKey("dbo.GxProductBatch", "Report_Id", "dbo.GxBatchReport", "Id");
            DropColumn("dbo.GxDetectionReport", "CompanyName");
            DropColumn("dbo.GxDetectionReport", "Title");
            DropColumn("dbo.GxDetectionReport", "DataList");
            DropColumn("dbo.GxDetectionReport", "Conclusion");
            DropColumn("dbo.GxDetectionReport", "Comments");
        }
        
        public override void Down()
        {
            AddColumn("dbo.GxDetectionReport", "Comments", c => c.String(nullable: false));
            AddColumn("dbo.GxDetectionReport", "Conclusion", c => c.String(nullable: false));
            AddColumn("dbo.GxDetectionReport", "DataList", c => c.String(nullable: false));
            AddColumn("dbo.GxDetectionReport", "Title", c => c.String());
            AddColumn("dbo.GxDetectionReport", "CompanyName", c => c.String());
            DropForeignKey("dbo.GxProductBatch", "Report_Id", "dbo.GxBatchReport");
            DropIndex("dbo.GxProductBatch", new[] { "Report_Id" });
            DropColumn("dbo.GxDetectionReport", "CreateTime");
            DropColumn("dbo.GxDetectionReport", "Files");
            DropColumn("dbo.GxProductBatch", "Report_Id");
            DropTable("dbo.GxBatchReport");
        }
    }
}
