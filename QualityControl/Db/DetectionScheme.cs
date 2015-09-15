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

        public string CheckNum { get; set; }  //检测号

        public long ProductId { get; set; }  //检查产品

        public string Level { get; set; }    //检查级别


        public double MaxQuote { get; set; }      //报价

        public double MinQuote { get; set; }      //报价

        public int MaxTime { get; set; }       //检测范围 几天


        public int MinTime { get; set; }       //检测范围 几天



        public double UserQuote { get; set; }     //给用户的报价

        public double OrganQuote { get; set; }    //给检测机构的报价

        public int Time { get; set; }          //确定时间

        public EnumDetectionSchemeStatus Status { get; set; }
    }

    /// <summary>
    /// 合同
    /// </summary>
    public class Contract
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }

        public string CheckNum { get; set; }  //检测号

        public long ProductId { get; set; }  //检查产品


        public string Level { get; set; }      //级别


        public double Quote { get; set; }      //报价

        public string UserId { get; set; }     //用户

        public int Time { get; set; }          //确定时间
        public EnumContractStatus Status { get; set; } //状态


    }

    public enum EnumContractStatus
    {
        未签订=0,
        已签定=1,
        已修改=2,
        修改后未审核=3
    }

    public enum EnumDetectionSchemeStatus
    {
        未发送=0,
        已发送待确定=1,
        返回修改=2,
        修改完成留档保存=3,
        已确定
    }

    /// <summary>
    /// 合同修改意见
    /// </summary>
    public class ContractModification
    {

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }

        public long  ContractId { get; set; }               //合同id

        public string UserId { get; set; }                  //修改人

        public string Modify { get; set; }
    }
}
