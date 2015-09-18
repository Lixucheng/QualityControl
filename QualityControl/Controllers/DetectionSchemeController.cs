using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using QualityControl.Db;
using System.IO;
using System.Text;

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
                x.ProductId = 3;
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
                        Count =5,
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
        public bool Sign(string checknum)
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

            return true;
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
            mod.DetectionSchemeId = x1.Id;
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

                var modify = Db.ContractModifications.Where(e => e.DetectionSchemeId == dec.Id).ToList();
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
            //todo 写一个message
            return Json(1);
        }

        /// <summary>
        /// 协议内容，需签订
        /// </summary>
        [HttpGet]
        public ActionResult CompactSign(string checknum)
        {
            //todo: 判断checknum
           
            string s0 = "一、";
            
            string s2 = @"（以下简称检验方）接受委托方书面检验委托。委托检验申请单（以下简称委托单）作为本协议的附件。本协议以委托方代表人在委托单上签名盖章，检验方加盖受理骑缝章后生效。 
                          <br />二、委托方应如实填写委托单，如有必要，还应根据检验方的要求提供必要的单据及相关资料。 
                          <br />三、检验方按委托方在委托单上填明的检验要求进行检验，并出具检验报告。 
                          <br />四、委托方必须注明要求使用的检验方法。 
                          <br />五、检验方的检验时间根据检验内容而定，原则上以检验方公布的时间为准，特殊情况双方协商确定，并在委托单上注明。 
                          <br />六、检验方检验收费按有关规定计价。对于批量样品如需减免检验费用的，委托方应在委托时与检验方协商确定，并在委托单上“备注”栏内注明。要求加急服务的，需支付加急费。 
                          <br />七、检验方接受委托方自送样品的检验，检验报告仅对样品负责。 
                          <br />八、对于某些项目，检验方需要分包检验的，检验方应以书面或电子媒体形式通知委托方。除委托方或上级管理机构指定的分包方外，检验方为分包方的工作对委托方负责。 
                          <br />九、检验方在接受委托时，须详细审核委托单内容。在确认委托方的委托及要求后，应填写委托单同一页上的领证凭条交付委托方，委托人凭此查询及索取检验报告。 
                          <br />十、检验方的检验报告有固定的格式，并仅提供唯一正本。如对检验报告有特殊要求，委托方应在委托单上“备注”栏内注明。 
                          <br />十一、检验报告通常采用中文书写。如需采用其他语种，委托方应在委托单“备注”栏内注明，并用相应语种填写有关内容。 
                          <br />十二、委托方如对检验结果有异议的，须在一个月内凭检验证书原件向检验方要求复检，检验方应于十日内安排复检。复检结果维持原检验结果的，委托方须按规定向检验方支付复检费。复检结果确认原检验结果有误的，检验方不再收取复检费。委托方对复检结果仍有异议，双方协商不成时，应与检验方书面协议，委托仲裁机构仲裁。 
                          <br />十五、委托方对本协议及委托单有不明之处，应在填写委托单时，向检验方工作人员咨询。协议自填单之日起生效。";
            string s1= "管控中心";
            ViewBag.FirstParty = "李续铖";
            ViewBag.SecondParty = "管控中心";
            //var user = UserManager.FindById(User.Identity.GetUserId());
            //if (user.Type == (int)Enum.EnumUserType.User)
            //{
            //    ViewBag.FirstParty = user.UserName;
            //    ViewBag.SecondParty = "管控中心";
            //    s1 = "管控中心";
            //    ViewBag.user=1;
            //}
            //else if (user.Type == (int)Enum.EnumUserType.TestingOrg)
            //{
            //    ViewBag.FirstParty = "管控中心";
            //    ViewBag.SecondParty = user.UserName;
            //    s1 = "检测公司";
            //    ViewBag.user=2;
            //}
            //else
            //{
            //    throw new Exception("无权查看！");
            //}
            ViewBag.user=1;//标记输入框位置
            ViewBag.time = DateTime.Today.ToShortDateString();
            ViewBag.Content = s0 + s1 + s2;
            ViewBag.checknum = checknum;
            return View();
        }
        public bool BoolCompactSign(string checknum)
        {
            string s0 = "一、";

            string s2 = @"（以下简称检验方）接受委托方书面检验委托。委托检验申请单（以下简称委托单）作为本协议的附件。本协议以委托方代表人在委托单上签名盖章，检验方加盖受理骑缝章后生效。 
                          <br />二、委托方应如实填写委托单，如有必要，还应根据检验方的要求提供必要的单据及相关资料。 
                          <br />三、检验方按委托方在委托单上填明的检验要求进行检验，并出具检验报告。 
                          <br />四、委托方必须注明要求使用的检验方法。 
                          <br />五、检验方的检验时间根据检验内容而定，原则上以检验方公布的时间为准，特殊情况双方协商确定，并在委托单上注明。 
                          <br />六、检验方检验收费按有关规定计价。对于批量样品如需减免检验费用的，委托方应在委托时与检验方协商确定，并在委托单上“备注”栏内注明。要求加急服务的，需支付加急费。 
                          <br />七、检验方接受委托方自送样品的检验，检验报告仅对样品负责。 
                          <br />八、对于某些项目，检验方需要分包检验的，检验方应以书面或电子媒体形式通知委托方。除委托方或上级管理机构指定的分包方外，检验方为分包方的工作对委托方负责。 
                          <br />九、检验方在接受委托时，须详细审核委托单内容。在确认委托方的委托及要求后，应填写委托单同一页上的领证凭条交付委托方，委托人凭此查询及索取检验报告。 
                          <br />十、检验方的检验报告有固定的格式，并仅提供唯一正本。如对检验报告有特殊要求，委托方应在委托单上“备注”栏内注明。 
                          <br />十一、检验报告通常采用中文书写。如需采用其他语种，委托方应在委托单“备注”栏内注明，并用相应语种填写有关内容。 
                          <br />十二、委托方如对检验结果有异议的，须在一个月内凭检验证书原件向检验方要求复检，检验方应于十日内安排复检。复检结果维持原检验结果的，委托方须按规定向检验方支付复检费。复检结果确认原检验结果有误的，检验方不再收取复检费。委托方对复检结果仍有异议，双方协商不成时，应与检验方书面协议，委托仲裁机构仲裁。 
                          <br />十五、委托方对本协议及委托单有不明之处，应在填写委托单时，向检验方工作人员咨询。协议自填单之日起生效。";
            string s1 = "管控中心";

            Sign(checknum);//合同详细信息
            var c = new Compact();
            var user = UserManager.FindById(User.Identity.GetUserId());
            if (user.Type == (int)Enum.EnumUserType.User)
            {
                c.FirstParty = user.UserName;
                c.SecondParty = "管控中心";
                s1 = "管控中心";
             
            }
            else if (user.Type == (int)Enum.EnumUserType.TestingOrg)
            {
                c.FirstParty = "管控中心";
                c.SecondParty = user.UserName;
                s1 = "检测公司";
               
            }
            else
            {
                throw new Exception("无权查看！");
            }
            c.Content = s0 + s1 + s2;       
            c.Time = DateTime.Now;
            c.CheckNum = checknum;
            Db.Compacts.Add(c);
            Db.SaveChanges();

            return true;
        }

        public int CheckName(string name,string checknum)
        {
            var n = User.Identity.GetUserName();
            if (name == n)
            {
                BoolCompactSign(checknum);
                return 1;
            }
            return 0;
        }


    }
}