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
            x.MinTime = 19;
            x.ProductId = 2;
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

            var pro = Db.Products.Find(x.ProductId);
            var
            company = Db.Companies.FirstOrDefault(e => e.UserId == pro.UserId);
            ViewBag.productname = pro.Name;
            ViewBag.company = company.Name;

         
            ViewBag.model = x;
            ViewBag.list = list;
            return View();
        }

        public ActionResult SignContract(string checknum)
        {

            var x = Db.DetectionSchemes.FirstOrDefault(e => e.CheckNum == checknum && e.Status == EnumDetectionSchemeStatus.已发送待确定);
            ViewBag.model = x;
            var pro = Db.Products.Find(x.ProductId);
            var
            company = Db.Companies.FirstOrDefault(e => e.UserId == pro.UserId);
            ViewBag.productname = pro.Name;
            ViewBag.company = company.Name;
            var list = Db.ProductBatchs.Where(e => e.CheckNum == checknum).ToList();
            ViewBag.list = list;
            //todo: 判断用户
            var have = Db.Contracts.FirstOrDefault(e => e.CheckNum == checknum && e.Status == EnumContractStatus.未签订);
            if (have == null)
            {

                var user = UserManager.FindById(User.Identity.GetUserId());
                double quote = 0;
                quote = x.UserQuote;
                //quote = user.Type == (int) Enum.EnumUserType.User ? x.UserQuote : x.OrganQuote;
                var c = new Contract
                {
                    CheckNum = checknum,
                    Level = x.Level,
                    Time=x.Time,
                    ProductId = x.ProductId,
                    Quote = quote,
                    Status = EnumContractStatus.未签订,
                    UserId="userid"
                   //todo: UserId = user.Id
                };
                Db.Contracts.Add(c);
                Db.SaveChanges();
            }
           

            return View();
        }

        /// <summary>
        /// 将合同发送给两方
        /// </summary>
        /// <param name="checknum"></param>
        /// <param name="quser"></param>
        /// <param name="qother"></param>
        /// <param name="time"></param>
        /// <returns></returns>
        public JsonResult SendDetectionScheme(string checknum,double quser,double qother,int time)
        {
            var x = Db.DetectionSchemes.FirstOrDefault(e => e.CheckNum == checknum && e.Status == EnumDetectionSchemeStatus.未发送);
            x.UserQuote = quser;
            x.OrganQuote = qother;
            x.Time = time;
            x.Status = EnumDetectionSchemeStatus.已发送待确定;
            Db.SaveChanges();
            return Json(1);
        }

        public ActionResult Sign(string checknum)
        {
            var x = Db.Contracts.FirstOrDefault(e => e.CheckNum == checknum && e.Status == EnumContractStatus.未签订);
            x.Status = EnumContractStatus.已签定;
            Db.SaveChanges();
            var x1= Db.DetectionSchemes.FirstOrDefault(e => e.CheckNum == checknum && e.Status==EnumDetectionSchemeStatus.已发送待确定);
            x1.Status = EnumDetectionSchemeStatus.已确定;
            Db.SaveChanges();
            return Redirect("..");
        }

        public JsonResult Modify(string checknum, string modify)
        {
            var x = Db.Contracts.FirstOrDefault(e => e.CheckNum == checknum && e.Status == EnumContractStatus.未签订);
            x.Status = EnumContractStatus.修改后未审核;
            Db.SaveChanges();
            var x1 = Db.DetectionSchemes.FirstOrDefault(e => e.CheckNum == checknum && e.Status == EnumDetectionSchemeStatus.已发送待确定);
            x1.Status = EnumDetectionSchemeStatus.返回修改;
            Db.SaveChanges();

            var mod = new Db.ContractModification();
            mod.ContractId = x.Id;
            mod.Modify = modify;
            mod.UserId = User.Identity.GetUserId();
            Db.ContractModifications.Add(mod);
            Db.SaveChanges();
            return Json(1);
        }

        public ActionResult CheckModify(string checknum)
        {
            var dec =
                Db.DetectionSchemes.FirstOrDefault(
                    e => e.CheckNum == checknum && e.Status == EnumDetectionSchemeStatus.返回修改);
            var pro = Db.Products.FirstOrDefault(e => e.Id == dec.ProductId);

            var company = Db.Companies.FirstOrDefault(e => e.UserId == pro.UserId);

            ViewBag.productname = pro.Name;
            ViewBag.company = company.Name;
            var list = Db.ProductBatchs.Where(e => e.CheckNum == checknum);
            ViewBag.model = dec;
            ViewBag.list = list;

            var modify = Db.ContractModifications.Where(e => e.ContractId == dec.Id).ToList();
            ViewBag.mlist = modify;
            return View();
        }

        public JsonResult ModifyDetectionScheme(long detectionid, double quser, double qother, int time)
        {
            var x = Db.DetectionSchemes.Find(detectionid);
            x.Status = EnumDetectionSchemeStatus.修改完成留档保存;
            Db.SaveChanges();
            var n = new DetectionScheme
            {
                CheckNum = x.CheckNum,
                Level = x.Level,
                MaxQuote = x.MaxQuote,
                MaxTime = x.MaxTime,
                MinQuote = x.MinQuote,
                MinTime = x.MinTime,
                OrganQuote = qother,
                ProductId = x.ProductId,
                Time = time,
                UserQuote = quser,
                Status = EnumDetectionSchemeStatus.已发送待确定
            };
            Db.DetectionSchemes.Add(n);
            Db.SaveChanges();

            return Json(1);
        }
    }
}