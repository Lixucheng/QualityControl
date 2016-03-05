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
        public string FactoryAddress { get; set; }

        [Required]
        public string Postcode { get; set; }

        [Required]
        public string Tel { get; set; }

        [Required]
        public string FAX { get; set; }

        [Required]
        public string Email { get; set; }

        [Required]
        public string EconomyType { get; set; }

        /// <summary>
        ///     工商登记机构
        /// </summary>
        [Required]
        public string BRI { get; set; }

        [Required]
        public DateTime EstablishedTime { get; set; }

        [Required]
        public string License { get; set; }

        [Required]
        public string CorporationName { get; set; }

        /// <summary>
        ///     经营期限
        /// </summary>
        [Required]
        public string OperationTerm { get; set; }

        /// <summary>
        ///     注册资金
        /// </summary>
        [Required]
        public string RegisteredCapital { get; set; }

        /// <summary>
        ///     年总产值
        /// </summary>
        public string GDP { get; set; }

        /// <summary>
        ///     年销售额
        /// </summary>
        public string AnnualSale { get; set; }

        /// <summary>
        ///     年缴税金额
        /// </summary>
        public string TaxPerYear { get; set; }

        /// <summary>
        ///     年利润
        /// </summary>
        public string AnnualProfit { get; set; }

        /// <summary>
        ///     固定资产
        /// </summary>
        [Required]
        public string FixedAsset { get; set; }

        [Required]
        public string OrganizationCode { get; set; }

        /// <summary>
        ///     企业负责人
        /// </summary>
        [Required]
        public string Superintendent { get; set; }

        /// <summary>
        ///     质量保证负责人
        /// </summary>
        [Required]
        public string QA { get; set; }

        /// <summary>
        ///     从业人员总数
        /// </summary>
        [Required]
        public int MemberCount { get; set; }

        /// <summary>
        ///     专业技术人员数
        /// </summary>
        [Required]
        public int TechnicianCount { get; set; }

        /// <summary>
        ///     联系人
        /// </summary>
        [Required]
        public string ContactPerson { get; set; }

        public EnumStatus Status { get; set; }

        public string UpdateJson { get; set; }

        public DateTime CreateTime { get; set; }

        public DateTime LastChangeTime { get; set; }

        public virtual List<Product> Products { get; set; }

        public string Note { get; set; }
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