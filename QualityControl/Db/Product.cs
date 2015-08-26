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
        
        public string Type { get; set; }

        public float Price { get; set; }

    }
}