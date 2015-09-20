﻿using QualityControl.Enum;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace QualityControl.Db
{
    [Table("GxSGS")]
    public class SGS
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        
        public long Id { get; set; }

        public string UserId { get; set; }

        public DateTime CreateTime { get; set; }

        public DateTime LastChangeTime { get; set; }

        public string UpdateJson { get; set; }

        public EnumStatus Status { get; set; }
        
        [Required]
        public string Name { set; get; }

        [Required]
        public string CorporationName { get; set; }

        [Required]
        public string Tel { get; set; }

        [Required]
        public string Address { get; set; }

        [Required]
        public string License { get; set; }

        public string Type { get; set; }

        public List<SgsProduct> Products { get; set; }
    }

    [Table("GxSgsProduct")]
    public class SgsProduct
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }

        public float Price { get; set; }

        public int NeedeDay { get; set; }

        public virtual SGS SGS { get; set; }

        public virtual Product Product { get; set; }
    }
}