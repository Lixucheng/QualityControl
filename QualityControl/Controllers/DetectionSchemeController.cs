using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using QualityControl.Db;

namespace QualityControl.Controllers
{
    public class DetectionSchemeController : BaseController
    {
        //检测方案合同部分
        // GET: DetectionScheme
        public ActionResult BuildDetectionScheme()
        {
            var x = new Db.DetectionScheme();
            x.Level = "特殊检测水平 S-1 A";
            x.MaxQuote = 5000;
            x.MinQuote = 1000;
            x.MaxTime = 20;
            x.MinTime = 2;
            x.ProductId = 1;
            x.Status = QualityControl.Db.EnumDetectionSchemeStatus.未发送;
            x.CheckNum = Guid.NewGuid().ToString();
            Db.DetectionSchemes.Add(x);
            Db.SaveChanges();
            var list = new List<ProductBatch>();
            for (int i = 1; i < 5; i++)
            {
                list.Add(new ProductBatch
                {
                    BatchName = "2015-8-" + i.ToString(),
                    Count = i * 1000,
                    ProductId = 1,
                    CheckNum = x.CheckNum
                });
            }
            Db.ProductBatchs.AddRange(list);
            Db.SaveChanges();

//            var pro = Db.Products.Find(x.ProductId);
//            var
//            company = Db.Companies.FirstOrDefault(e => e.UserId == pro.UserId);
            ViewBag.productname = "pro.Name";
            ViewBag.company = "company.Name";

         
            ViewBag.model = x;
            ViewBag.list = list;
            return View();
        }

        public ActionResult SignContract(string checknum)
        {
            //todo: 判断用户

            var x=Db.DetectionSchemes.FirstOrDefault(e => e.CheckNum == checknum && e.Status == EnumDetectionSchemeStatus.已发送待确定);
            ViewBag.model = x;
            var pro = Db.Products.Find(x.ProductId);
            var
            company = Db.Companies.FirstOrDefault(e => e.UserId == pro.UserId);
            ViewBag.productname = pro.Name;
            ViewBag.company = company.Name;
            var list = Db.ProductBatchs.Where(e => e.CheckNum == checknum).ToList();
            ViewBag.list = list;

            var user = UserManager.FindById(User.Identity.GetUserId());

            var c = new Contract
            {
                CheckNum = checknum,
                Level = x.Level,
                MaxTime = x.MaxTime,
                MinTime = x.MinTime,
                ProductId = x.ProductId,
                Quote = x.UserQuote,
                Status = EnumContractStatus.已签定,
                UserId = "123123"
            };
            Db.Contracts.Add(c);
            Db.SaveChanges();

            return View();
        }

        /// <summary>
        /// 将合同发送给两方
        /// </summary>
        /// <param name="checknum"></param>
        /// <param name="quser"></param>
        /// <param name="qother"></param>
        /// <returns></returns>
        public ActionResult SendDetectionScheme(string checknum,double quser,double qother)
        {
            var x = Db.DetectionSchemes.FirstOrDefault(e => e.CheckNum == checknum && e.Status == EnumDetectionSchemeStatus.未发送);
            x.UserQuote = quser;
            x.OrganQuote = qother;
            x.Status = EnumDetectionSchemeStatus.已发送待确定;
            Db.SaveChanges();
            return View();
        }

        public ActionResult Sign(string checknum)
        {
            var x = Db.Contracts.FirstOrDefault(e => e.CheckNum == checknum && e.Status == EnumContractStatus.未签订);
            x.Status = EnumContractStatus.已签定;
            Db.SaveChanges();
            var x1= Db.DetectionSchemes.FirstOrDefault(e => e.CheckNum == checknum && e.Status==EnumDetectionSchemeStatus.已发送待确定);
            x1.Status = EnumDetectionSchemeStatus.已确定;
            Db.SaveChanges();
            return View();
        }

        public ActionResult Modify(string checknum, string modify)
        {
            var x = Db.Contracts.FirstOrDefault(e => e.CheckNum == checknum && e.Status == EnumContractStatus.未签订);
            x.Status = EnumContractStatus.修改后未审核;
            Db.SaveChanges();
            var x1 = Db.DetectionSchemes.FirstOrDefault(e => e.CheckNum == checknum && e.Status == EnumDetectionSchemeStatus.已发送待确定);
            x1.Status = EnumDetectionSchemeStatus.已确定;
            Db.SaveChanges();
            return View();
        }
    }
}