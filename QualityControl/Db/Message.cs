using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QualityControl.Db
{
    [Table("GxMessage")]
    public class Message
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }

        public string UserId { get; set; }

        public string Content { get; set; }

        public int Status { get; set; }
    }
}