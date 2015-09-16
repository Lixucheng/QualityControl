using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QualityControl.Db
{
    [Table("GxDict")]
    public class Dict
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }

        public string Key { get; set; }

        public string Value { get; set; }

        public int Type { get; set; }

        public int Status { get; set; }
    }

    public enum EnumDictType
    {

    }
}