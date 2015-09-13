using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QualityControl.Db
{
    public class Product
    {
        [Key]
        // 自增
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }

        public string Name { get; set; }

        public float Price { get; set; }

        public virtual ProductType Type { get; set; }

        public int Status { get; set; }

        public string UserId { get; set; }

        //public Company Company { get; set; }

    }

    public class ProductType
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public virtual List<Product> Products { get; set; }
    }

    public enum EnumProductStatus
    {
        Created,
        Published
    }

    public class ProductViewModel
    {

    }

}