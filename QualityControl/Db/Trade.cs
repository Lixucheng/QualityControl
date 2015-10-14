﻿using System;
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

        public DateTime FinishTime { get; set; }

        public virtual List<DetectionReport> Report { get; set; }

        public string  ManufacturerId { get; set; }

        public string  SgsUserId { get; set; }

        public string UserId { get; set; }

        public string Product { get; set; }

        public bool SGSPaied { get; set; }

        /// <summary>
        /// 协议内容
        /// </summary>
        public virtual Compact Compact { get; set; }

        public virtual List<ProductBatch> Batches { get; set; }

    }

    public enum EnumSample
    {
        Random,
        Systematic
    }

    public enum EnumTradeStatus
    {
        
        /// <summary>
        /// 创建
        /// </summary>
        Create,
        /// <summary>
        /// 申请完成
        /// </summary>
        AlreadyApply,
        /// <summary>
        /// 合同确认流程
        /// </summary>
        EnsureContract,

        /// <summary>
        /// 合同已签
        /// </summary>
        Signed,
        /// <summary>
        /// 制码流程
        /// </summary>
        MakeQrCode,
        /// <summary>
        /// 抽样流程结束
        /// </summary>
        SampleFinshed,
        /// <summary>
        /// 检查中
        /// </summary>
        Testing,

        /// <summary>
        /// 检测接受，等待审核
        /// </summary>
        Tested,

        /// <summary>
        /// 结束
        /// </summary>
        Finish


    }
}