using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QualityControl.Db
{
    [Table("GxProduct")]
    public class Product
    {
        [Key]
        // 自增
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }

        public string Name { get; set; }

        public float Price { get; set; }

        public virtual ThirdProductType Type { get; set; }

        public int Status { get; set; }

        public string UserId { get; set; }

        public virtual Company Company { get; set; }

        public virtual List<SgsProduct> SgsProducts { get; set; }

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

    public enum EnumProductStatus
    {
        Created,
        Published
    }

    public class ProductViewModel
    {

    }

    [Table("GxProductBatch")]
    public class ProductBatch
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long   Id { get; set; }

        public long   ProductId { get; set; }

        public string BatchName { get; set; }

        public int   Count { get; set; }

        public int SampleCount { get; set; }

        public string CheckNum { get; set; }

        public string SamplaListJson { get; set; }
    }

}