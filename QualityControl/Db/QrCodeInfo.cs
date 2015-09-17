using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QualityControl.Db
{
    [Table("GxQrCodeInfo")]
    public class QrCodeInfo
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }

        public string QrName { get; set; }//产品信息

        public string IdCode { get; set; }//验证码信息
    }
}
