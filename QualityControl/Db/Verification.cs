using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QualityControl.Db
{
    [Table("Verification")]
    public class Verification
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }

        public virtual QrCodeInfo QrCodeInfo { get; set; }

        public long TradeId { get; set; }

        public EnumVerificationStatus Status { get; set; }
    }

    public enum EnumVerificationStatus
    {
        通过,
        错误
    }
}