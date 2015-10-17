using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using QualityControl.Enum;

namespace QualityControl.Db
{
    /// <summary>
    ///     企业信息
    /// </summary>
    [Table("GxCompany")]
    public class Company
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [ForeignKey("Products")]
        public long Id { get; set; }

        public string UserId { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string Address { get; set; }

        [Required]
        public DateTime EstablishedTime { get; set; }

        [Required]
        public string License { get; set; }

        [Required]
        public string CorporationName { get; set; }

        [Required]
        public string CorporationIdentity { get; set; }

        [Required]
        public string OrganizationCode { get; set; }

        [Required]
        public string LicenseScope { get; set; }

        public EnumStatus Status { get; set; }

        public string UpdateJson { get; set; }

        public DateTime CreateTime { get; set; }

        public DateTime LastChangeTime { get; set; }

        public virtual List<Product> Products { get; set; }
    }

    [Table("GxCompanyProduct")]
    public class CompanyProduct
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }

        public long CompanyId { get; set; }

        public string Name { get; set; }

        public long ProductTypeId { get; set; } //所属类别id

        public string ProductionCertificateNo { get; set; } //生产许可证编号

        public string GetDate { get; set; } //颁发日期

        public string Standard { get; set; } //执行标准

        public EnumProductStatus CompanyProductStatus { get; set; }
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