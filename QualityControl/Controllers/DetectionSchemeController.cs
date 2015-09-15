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
        //检测方案合同部分 1121112231
        // GET: DetectionScheme
        public ActionResult BuildDetectionScheme(string checknum)
        {
            if (string.IsNullOrEmpty(checknum))
            {
                throw new Exception("访问错误！");
            }
            var x = new Db.DetectionScheme();
            var list = new List<ProductBatch>();
            x = Db.DetectionSchemes.FirstOrDefault(e => e.CheckNum == checknum&&e.Status!=EnumDetectionSchemeStatus.修改完成留档保存);
            if (x == null)            
            {
                //方案
                x = new Db.DetectionScheme();
                x.Level = "特殊检测水平 S-1 A";
                x.MaxQuote = 5000;
                x.MinQuote = 1000;
                x.MaxTime = 20;
                x.MinTime = 19;
                x.ProductId = 2;
                x.Status = QualityControl.Db.EnumDetectionSchemeStatus.未发送;
                x.CheckNum = checknum;
                Db.DetectionSchemes.Add(x);
                Db.SaveChanges();
               //批次
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
            }
            else if (x.Status == EnumDetectionSchemeStatus.未发送)
            {
                var pro = Db.Products.Find(x.ProductId);
                var
                company = Db.Companies.FirstOrDefault(e => e.UserId == pro.UserId);
                ViewBag.productname = pro.Name;
                ViewBag.company = company.Name;


                ViewBag.model = x;
                ViewBag.list = Db.ProductBatchs.Where(e=>e.CheckNum==checknum).ToList();
            }
            else if (x.Status == EnumDetectionSchemeStatus.返回修改)
            {
                return Redirect("../DetectionScheme/CheckModify?checknum=" + checknum);
            }
            else
            {
                throw new Exception("方案已发送待确定或者已确定，不支持编辑！");
            }
         
          
            return View();
        }

        public ActionResult SignContract(string checknum)
        {
            //todo 根据用户判断是否有权查看 双方之一
            if (string.IsNullOrEmpty(checknum))
            {
                throw new Exception("访问错误！");
            }
            var x = Db.DetectionSchemes.FirstOrDefault(e => e.CheckNum == checknum && e.Status == EnumDetectionSchemeStatus.已发送待确定);
            if (x == null)
            {
                ViewBag.ok = 0;
                ViewBag.message = "合同不在编辑状态,拒绝访问！";
            }
            else
            {
                ViewBag.model = x;
                var pro = Db.Products.Find(x.ProductId);
                var
                company = Db.Companies.FirstOrDefault(e => e.UserId == pro.UserId);
                ViewBag.productname = pro.Name;
                ViewBag.company = company.Name;
                var list = Db.ProductBatchs.Where(e => e.CheckNum == checknum).ToList();
                ViewBag.list = list;
                //todo 这块查找带上用户
                var have = Db.Contracts.FirstOrDefault(e => e.CheckNum == checknum && e.Status == EnumContractStatus.未签订);
                if (have == null)
                {
                    //todo: 判断用户 生成双方的合同
                    var user = UserManager.FindById(User.Identity.GetUserId());
                    double quote = 0;
                    quote = x.UserQuote;
                    //quote = user.Type == (int) Enum.EnumUserType.User ? x.UserQuote : x.OrganQuote;
                    var c = new Contract
                    {
                        CheckNum = checknum,
                        Level = x.Level,
                        Time = x.Time,
                        ProductId = x.ProductId,
                        Quote = quote,
                        Status = EnumContractStatus.未签订,
                        UserId = "userid"
                        //todo: UserId = user.Id
                    };
                    Db.Contracts.Add(c);
                    Db.SaveChanges();
                }

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


        /// <summary>
        /// 签合同
        /// </summary>
        /// <param name="checknum"></param>
        /// <returns></returns>
        public ActionResult Sign(string checknum)
        {
            //todo： 还有userid未使用
            var x = Db.Contracts.FirstOrDefault(e => e.CheckNum == checknum && e.Status == EnumContractStatus.未签订);
            if (x == null)
            {
                throw new Exception("无待签合同");
            }
            x.Status = EnumContractStatus.已签定;
            Db.SaveChanges();

            //双方确定方案才确定
            var c = Db.Contracts.Count(e => e.CheckNum == checknum && e.Status == EnumContractStatus.已签定);
            if (c == 2)
            {
                var x1 = Db.DetectionSchemes.FirstOrDefault(e => e.CheckNum == checknum && e.Status == EnumDetectionSchemeStatus.已发送待确定);
                x1.Status = EnumDetectionSchemeStatus.已确定;
                Db.SaveChanges();
            }

           

            return Redirect("..");
        }

        /// <summary>
        /// 提出修改
        /// </summary>
        /// <param name="checknum"></param>
        /// <param name="modify"></param>
        /// <returns></returns>
        public JsonResult Modify(string checknum, string modify)
        {
            var x = Db.Contracts.FirstOrDefault(e => e.CheckNum == checknum && e.Status == EnumContractStatus.未签订);
            x.Status = EnumContractStatus.修改后未审核;
            Db.SaveChanges();
            var x1 = Db.DetectionSchemes.FirstOrDefault(e => e.CheckNum == checknum && e.Status == EnumDetectionSchemeStatus.已发送待确定);
            x1.Status = EnumDetectionSchemeStatus.返回修改;
            Db.SaveChanges();

            var mod = new Db.ContractModification();
            mod.ContractId = x1.Id;
            mod.Modify = modify;
            mod.UserId = User.Identity.GetUserId();
            Db.ContractModifications.Add(mod);
            Db.SaveChanges();
            return Json(1);
        }

        /// <summary>
        /// 审查修改
        /// </summary>
        /// <param name="checknum"></param>
        /// <returns></returns>
        public ActionResult CheckModify(string checknum)
        {
            var dec =
                Db.DetectionSchemes.FirstOrDefault(
                    e => e.CheckNum == checknum && e.Status == EnumDetectionSchemeStatus.返回修改);
            if (dec != null)
            {
                var pro = Db.Products.FirstOrDefault(e => e.Id == dec.ProductId);

                var company = Db.Companies.FirstOrDefault(e => e.UserId == pro.UserId);

                ViewBag.productname = pro.Name;
                ViewBag.company = company.Name;
                var list = Db.ProductBatchs.Where(e => e.CheckNum == checknum);
                ViewBag.model = dec;
                ViewBag.list = list;

                var modify = Db.ContractModifications.Where(e => e.ContractId == dec.Id).ToList();
                ViewBag.mlist = modify;
                ViewBag.ok = 1;
            }
            else
            {
                ViewBag.ok = 0;
            }

            return View();
        }

        /// <summary>
        /// 修改待再次发送给双方
        /// </summary>
        /// <param name="detectionid"></param>
        /// <param name="quser"></param>
        /// <param name="qother"></param>
        /// <param name="time"></param>
        /// <returns></returns>
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