using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace QualityControl.Db
{
    [Table("GxDetectionReport")]
    public class DetectionReport
    {
        [Key]
        // 自增
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }

        public string CompanyName { get; set; }

        public string Title { get; set; }

        [Required]
        public string DataList { get; set; }

        [Required]
        public string Conclusion { get; set; }

        [Required]
        public string Comments { get; set; }

        public int Status { get; set; }
    }

    public class DetectionReportDataItem
    {
        public string Item { get; set; }

        public string Requirement { get; set; }

        public string SampleNo { get; set; }

        public string Result { get; set; }
    }
}