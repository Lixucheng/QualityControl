using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using QualityControl.Enum;

namespace QualityControl.Db
{
    [Table("GxProduct")]
    public class Product
    {
        [Key]
        // 自增
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }

        [Required]
        public string Name { get; set; }

        /// <summary>
        /// 保质期
        /// </summary>
        public string ShelfLife { get; set; }

        public string Brand { get; set; }

        /// <summary>
        /// 等级
        /// </summary>
        public string Grade { get; set; }

        /// <summary>
        /// 净含量
        /// </summary>
        public string Weight { get; set; }

        /// <summary>
        /// 配料表
        /// </summary>
        public string Ingredients { get; set; }



        /// <summary>
        /// 储存条件
        /// </summary>
        public string StorageCondition { get; set; }

        /// <summary>
        /// 包装说明
        /// </summary>
        public string Packing { get; set; }

        /// <summary>
        /// 包装材料
        /// </summary>
        public string PackingMaterial { get; set; }

        /// <summary>
        /// 许可类别
        /// </summary>
        public string PermissionType { get; set; }

        /// <summary>
        /// 材料
        /// </summary>
        public string Material { get; set; }

        /// <summary>
        /// 产品图片
        /// </summary>
        public string Image { get; set; }

        /// <summary>
        /// 上传材料
        /// </summary>
        public string File { get; set; }

        /// <summary>
        /// 授权企业
        /// </summary>
        public string EmpowerEnterprise { get; set; }

        public float Price { get; set; }

        public virtual ThirdProductType Type { get; set; }

        public EnumStatus Status { get; set; }

        public string UserId { get; set; }

        public virtual Company Company { get; set; }

        [Required]
        public string ProductionCertificateNo { get; set; } //生产许可证编号

        [Required]
        public string GetDate { get; set; } //到期日期

        [Required]
        public string Standard { get; set; } //执行标准

        public virtual List<SgsProduct> SgsProducts { get; set; }

        public virtual List<BaseProductBatch> BaseProductBatchs { get; set; } //产品批次

        public DateTime CreateTime { get; set; }

        public DateTime LastChangeTime { get; set; }

        public string UpdateJson { get; set; }
    }

    public enum EnumProductStatus
    {
        正在生产 = 1,
        停产 = 0
    }

    [Table("GxFirstProductType")]
    public class FirstProductType
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public virtual List<SecondProductType> SecondProductTypes { get; set; }
    }

    [Table("GxSecondProductType")]
    public class SecondProductType
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public virtual List<ThirdProductType> Productypes { get; set; }

        public virtual FirstProductType FirstType { get; set; }
    }


    [Table("GxThirdProductType")]
    public class ThirdProductType
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public virtual List<Product> Products { get; set; }

        public virtual SecondProductType SecondType { get; set; }
    }


    public class ProductViewModel
    {
    }

    [Table("GxProductBatch")]
    public class ProductBatch
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }

        public long ProductId { get; set; }

        public string BatchName { get; set; }

        public int Count { get; set; }

        public int SampleCount { get; set; }

        public virtual Trade Trade { get; set; }
        public string Level { get; set; } //检查级别for example abcd


        public string SamplaListJson { get; set; }
    }

    [Table("GxBaseProductBatch")]
    public class BaseProductBatch
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }

        public long ProductId { get; set; }

        public string BatchName { get; set; }

        public int Count { get; set; }

        public string ProductionDate { get; set; }
    }
}