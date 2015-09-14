using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using QualityControl.Db;

namespace QualityControl.Controllers
{
    public class DetectionSchemeController : BaseController
    {
        //检测方案合同部分
        // GET: DetectionScheme
        public ActionResult BuildDetectionScheme()
        {
            //var x = new Db.DetectionScheme();
            //x.Level = "特殊检测水平 S-1 A";
            //x.MaxQuote = 5000;
            //x.MinQuote = 1000;
            //x.MaxTime = 20;
            //x.MinTime = 2;
            //x.ProductId = 1;
            //x.Status = QualityControl.Db.EnumDetectionSchemeStatus.未发送;
            //x.CheckNum = Guid.NewGuid().ToString();
            //Db.DetectionSchemes.Add(x);
            //Db.SaveChanges();
            //var list = new List<ProductBatch>();
            //for (int i = 1; i < 5; i++)
            //{
            //    list.Add(new ProductBatch
            //    {
            //        BatchName="2015-8-"+i.ToString(),
            //        Count=i*1000,
            //        ProductId=1,
            //CheckNum=x.CheckNum
            //    });
            //}
            //Db.ProductBatchs.AddRange(list);
            //Db.SaveChanges();

            //ViewBag.productname = Db.Products.Find(x.ProductId).Name;
            //ViewBag.company = "百事公司";
            //ViewBag.model = x;
            //ViewBag.list = list;
            return View();
        }

        public ActionResult SignContract(string checknum)
        {
            var x=Db.DetectionSchemes.FirstOrDefault(e => e.CheckNum == checknum && e.Status == EnumDetectionSchemeStatus.已发送待确定);
            ViewBag.model = x;
            var pro = Db.Products.Find(x.ProductId);
            var
            company = Db.Companies.FirstOrDefault(e => e.UserId == pro.UserId);
            ViewBag.productname = pro.Name;
            ViewBag.company = company.Name;
            var list = Db.ProductBatchs.Where(e => e.CheckNum == checknum).ToList();
            ViewBag.list = list;
            return View();
        }
    }
}