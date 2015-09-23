using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using QualityControl.Models;

namespace QualityControl.Db
{
    [Table("GxTrade")]
    public class Trade
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }

        public virtual List<DetectionScheme> Schemes { get; set; }

        public EnumSample SampleType { get; set; }

        public DateTime CeateTime { get; set; }

        public int Status { get; set; }

        //批次
        public virtual List<ProductBatch> Batches { get; set; }

        public DateTime FinishTime { get; set; }

        public virtual List<DetectionReport> Report { get; set; }

        public virtual ApplicationUser Manufacturer { get; set; }

        public virtual ApplicationUser SgsUser { get; set; }

        public string UserId { get; set; }

        public virtual Product Product { get; set; }

        /// <summary>
        /// 协议内容
        /// </summary>
        public virtual Compact Compact { get; set; }

    }

    public enum EnumSample
    {
        Random,
        Systematic
    }

    public enum EnumTradeStatus
    {
        SampleFinshed,
        Create
    }
}