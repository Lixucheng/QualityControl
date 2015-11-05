using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QualityControl.Db
{
    [Table("Verification")]
    public class Verification
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }

        public virtual List<QrCodeInfo> Num { get; set; }
     

        public EnumVerificationStatus Status { get; set; } 
    }
    public enum EnumVerificationStatus
    {
      通过,
      错误
    }

}
