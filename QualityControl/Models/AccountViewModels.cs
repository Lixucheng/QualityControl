using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using QualityControl.Enum;
using System;

namespace QualityControl.Models
{
    public class ExternalLoginConfirmationViewModel
    {
        [Required]
        [Display(Name = "电子邮件")]
        public string Email { get; set; }
    }

    public class ExternalLoginListViewModel
    {
        public string ReturnUrl { get; set; }
    }

    public class SendCodeViewModel
    {
        public string SelectedProvider { get; set; }
        public ICollection<System.Web.Mvc.SelectListItem> Providers { get; set; }
        public string ReturnUrl { get; set; }
        public bool RememberMe { get; set; }
    }

    public class VerifyCodeViewModel
    {
        [Required]
        public string Provider { get; set; }

        [Required]
        [Display(Name = "代码")]
        public string Code { get; set; }
        public string ReturnUrl { get; set; }

        [Display(Name = "记住此浏览器?")]
        public bool RememberBrowser { get; set; }

        public bool RememberMe { get; set; }
    }

    public class ForgotViewModel
    {
        [Required]
        [Display(Name = "电子邮件")]
        public string Email { get; set; }
    }

    public class LoginViewModel
    {
        [Required]
        [Display(Name = "电子邮件")]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "密码")]
        public string Password { get; set; }

        [Display(Name = "记住我?")]
        public bool RememberMe { get; set; }

        [Required]
        [Display(Name = "类型")]
        public EnumUserType Type { get; set; }
    }

    public class RegisterViewModel
    {
        [Required]
        [EmailAddress]
        [Display(Name = "电子邮件")]
        public string Email { get; set; }

        [Required]
        [Display(Name = "姓名")]
        public string Name { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "{0} 必须至少包含 {2} 个字符。", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "密码")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "确认密码")]
        [Compare("Password", ErrorMessage = "密码和确认密码不匹配。")]
        public string ConfirmPassword { get; set; }

        [Required]
        [Display(Name = "类型")]
        public EnumUserType Type { get; set; }
    }

    public class ResetPasswordViewModel
    {
        [Required]
        [EmailAddress]
        [Display(Name = "电子邮件")]
        public string Email { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "{0} 必须至少包含 {2} 个字符。", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "密码")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "确认密码")]
        [Compare("Password", ErrorMessage = "密码和确认密码不匹配。")]
        public string ConfirmPassword { get; set; }

        public string Code { get; set; }
    }

    public class ForgotPasswordViewModel
    {
        [Required]
        [EmailAddress]
        [Display(Name = "电子邮件")]
        public string Email { get; set; }
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
        public string Corporation { get; set; }

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
