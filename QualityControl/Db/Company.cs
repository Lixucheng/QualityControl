﻿using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QualityControl.Db
{
    /// <summary>
    /// 企业信息
    /// </summary>
    public class Company
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }

        public string UserId { get; set; }

        public string Name { get; set; }

        public string Address { get; set; }

        public DateTime EstablishedTime { get; set; }

        public string License { get; set; }

        public string CorporationName { get; set; }

        public string CorporationIdentity { get; set; }

        public string Postcode { get; set; }

        public string OrganizationCode { get; set; }

        public string LicenseScope { get; set; }
    }

    public class CompanyProduct
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }

        public long CompanyId { get; set; }

        public string Name { get; set; }

        public long ProductTypeId { get; set; }//所属类别id

        public string ProductionCertificateNo { get; set; }//生产许可证编号

        public string GetDate { get; set; }//颁发日期

        public string Standard { get; set; }//执行标准

        public EnumCompanyProductStatus CompanyProductStatus { get; set; }


    }


    public enum EnumCompanyProductStatus
    {
        正在生产=1,
        停产=0
    }
    public class CompanyViewModel
    {
        [Required]
        [Display(Name = "企业名称")]
        public string Name { get; set; }

        [Required]
        [Display(Name = "注册地址")]
        public string Address { get; set; }

        [Required]
        [Display(Name = "成立时间")]
        public DateTime EstablishedTime { get; set; }

        [Required]
        [Display(Name = "营业执照号")]
        public string License { get; set; }

        [Required]
        [Display(Name = "法人")]
        public string CorporationName { get; set; }


        [Required]
        [Display(Name = "法人身份证")]
        public string CorporationIdentity { get; set; }

        [Required]
        [Display(Name = "邮编")]
        public string Postcode { get; set; }

        [Required]
        [Display(Name = "组织机构代码")]
        public string OrganizationCode { get; set; }

        [Required]
        [Display(Name = "营业执照经营范围")]
        public string LicenseScope { get; set; }

    }
}