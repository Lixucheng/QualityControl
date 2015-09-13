﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QualityControl.Db
{
    /// <summary>
    /// 检测方案
    /// </summary>
    public class DetectionScheme
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }

        public string Project { get; set; } //检查项目

        public string Level { get; set; }   //级别

        public int MaxTime { get; set; }       //检测时间 几天

        public int MaxQuote { get; set; }      //报价

        public int MinTime { get; set; }       //检测时间 几天

        public int MinQuote { get; set; }      //报价
    }

    /// <summary>
    /// 合同
    /// </summary>
    public class Contract
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }

        public string Project { get; set; }    //检查项目

        public string Level { get; set; }      //级别

        public int MaxTime { get; set; }       //检测时间 几天

        public int MaxQuote { get; set; }      //报价

        public int MinTime { get; set; }       //检测时间 几天

        public int MinQuote { get; set; }      //报价

        public string UserId { get; set; }

        public EnumContractStatus Status { get; set; }


    }

    public enum EnumContractStatus
    {
        未签订=0,
        已签定=1,
        已修改=2,
        修改后未审核=3
    }

    /// <summary>
    /// 合同修改意见
    /// </summary>
    public class ContractModification
    {

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }

        public long ContractId { get; set; }                //合同id

        public string UserId { get; set; }                  //修改人

        public string TimeModification { get; set; }        //时间修改意见

        public string QuoteModification { get; set; }       //报价修改意见
    }
}
