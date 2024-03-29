﻿using System;
using System.Collections.Generic;
using QualityControl.Db;
using QualityControl.Enum;
using QualityControl.Util;

namespace QualityControl.Models
{
    public class ProductCopy
    {
        public ProductCopy()
        {
        }

        public ProductCopy(Product src)
        {
            Dumper.Dump(src, this, false);
            TypeId = src.Type.Id;
        }

        public long Id { get; set; }

        public string Name { get; set; }

        public float Price { get; set; }

        public long TypeId { get; set; }

        public EnumStatus Status { get; set; }

        public string UserId { get; set; }

        public string ProductionCertificateNo { get; set; } //生产许可证编号

        public string GetDate { get; set; } //颁发日期

        public string Standard { get; set; } //执行标准

        public virtual List<BaseProductBatch> BaseProductBatchs { get; set; } //产品批次

        public DateTime CreateTime { get; set; }

        public DateTime LastChangeTime { get; set; }

        public List<BaseProductBatch> Batches { get; set; }

        public Product ToProductDbOject()
        {
            return Singleton.Self.Db.Products.Find(Id);
        }
    }
}