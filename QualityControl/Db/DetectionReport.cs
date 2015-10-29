using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QualityControl.Db
{
    [Table("GxDetectionReport")]
    public class DetectionReport
    {
        [Key]
        // 自增
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }

        public int Status { get; set; }

        public string Files { get; set; }

        public DateTime CreateTime { get; set; }
    }

    public class DetectionReportDataItem
    {
        public string Item { get; set; }

        public string Requirement { get; set; }

        public string SampleNo { get; set; }

        public string Result { get; set; }
    }
}