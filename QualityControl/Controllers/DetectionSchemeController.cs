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
        public ActionResult BuildDetectionScheme(long tradeid)
        {
            var trade = Db.Trades.Find(tradeid);
            if (trade == null)
            {
                throw new Exception("访问错误！");
            }

            var x = new Db.DetectionScheme();
            var list = trade.Batches;
            x = trade.Schemes.FirstOrDefault(e =>e.Status!=EnumDetectionSchemeStatus.修改完成留档保存);
            var sgsprolist = Db.SgsProducts.Where(e=>e.Product==trade.Product);
            if (x == null)            
            {
                //方案
                x = new Db.DetectionScheme
                {
                    MaxQuote = sgsprolist.Max(e => e.Price),
                    MinQuote = sgsprolist.Min(e => e.Price),
                    MaxTime = sgsprolist.Max(e => e.NeedeDay),
                    MinTime = sgsprolist.Min(e => e.NeedeDay),
                    Product = trade.Product,
                    Status = QualityControl.Db.EnumDetectionSchemeStatus.未发送,
                    Trade = trade
                };
                Db.DetectionSchemes.Add(x);
                Db.SaveChanges();
             

                var pro = x.Product;
                var
                company = Db.Companies.FirstOrDefault(e => e.UserId == pro.UserId);
                ViewBag.productname = pro.Name;
                ViewBag.company = company.Name;


                ViewBag.model = x;
                ViewBag.list = list;
            }
            else if (x.Status == EnumDetectionSchemeStatus.未发送)
            {
                var pro = x.Product;
                var
                company = Db.Companies.FirstOrDefault(e => e.UserId == pro.UserId);
                ViewBag.productname = pro.Name;
                ViewBag.company = company.Name;


                ViewBag.model = x;
                ViewBag.list = trade.Batches;
            }
            else if (x.Status == EnumDetectionSchemeStatus.返回修改)
            {
                return Redirect("../DetectionScheme/CheckModify?checknum=" + trade.Id);
            }
            else
            {
                throw new Exception("方案已发送待确定或者已确定，不支持编辑！");
            }
            var levelconvert = new Models.ConvertLevel();
            ViewBag.Level = levelconvert.GetLevelByCount(trade.Batches.First().Count);
            ViewBag.tradeid = tradeid;
            return View();
        }

        public JsonResult GetLevel(long tradeid, int  s2 )
        {
            var trade = Db.Trades.Find(tradeid);
            var levelconvert = new Models.ConvertLevel();
            var l=levelconvert.GetLevel(trade.Batches.First().Count,s2);
            var she=trade.Schemes.First(e => e.Status == EnumDetectionSchemeStatus.未发送);           
            var s1 = 0;
            s1 = s2 < 10 ? 0 : 1;
            she.Level = Json(new {l1=s1,l2=s2,l3=l}).ToString();
            Db.SaveChanges();
            return Json(l);
        }

  

        public ActionResult SignContract(long tradeid)
        {
            var userid = User.Identity.GetUserId();
            var trade = Db.Trades.Find(tradeid);
            if (trade == null)
            {
                throw new Exception("访问错误！");
            }
            if (userid != trade.User.Id && userid != trade.SgsUser.Id)
            {
                throw new Exception("访问错误！");
            }
            var x = trade.Schemes.FirstOrDefault(e => e.Status == EnumDetectionSchemeStatus.已发送待确定);
            if (x == null)
            {
                ViewBag.ok = 0;
                ViewBag.message = "合同不在编辑状态,拒绝访问！";
            }
            else
            {
                ViewBag.model = x;
                var pro = x.Product;
                var
                company = Db.Companies.FirstOrDefault(e => e.UserId == pro.UserId);
                ViewBag.productname = pro.Name;
                ViewBag.company = company.Name;
                var list = trade.Batches;
                ViewBag.list = list;
   
               
                var detectionscheme = trade.Schemes.FirstOrDefault(e => e.Status == EnumDetectionSchemeStatus.已发送待确定);
                if (detectionscheme == null)
                {
                    throw new Exception("没有待确认合同！");
                }
                var have = detectionscheme.Contracts.FirstOrDefault(e => e.UserId==userid&& e.Status == EnumContractStatus.未签订);
                if (have == null)
                {
                    
                    var user = UserManager.FindById(User.Identity.GetUserId());
                    double quote = 0;
                    quote = user.Type == (int) Enum.EnumUserType.User ? x.UserQuote : x.OrganQuote;
                    var c = new Contract
                    {
                        DetectionScheme=detectionscheme,
                        Level = x.Level,
                        Time = x.Time,
                        Product = detectionscheme.Product,
                        Quote = quote,
                        Status = EnumContractStatus.未签订,
                        UserId = user.Id                        
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
        public JsonResult SendDetectionScheme(long tradeid,double quser,double qother,int time,string level)
        {
            var trade = Db.Trades.Find(tradeid);
            var x = trade.Schemes.FirstOrDefault(e => e.Status == EnumDetectionSchemeStatus.未发送);
            x.Level = level;
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
        /// <param name="tradeid"></param>
        /// <returns></returns>
        public bool Sign(long tradeid)
        {
            var trade = Db.Trades.Find(tradeid);
            //todo： 还有userid未使用
            var scheme = trade.Schemes.FirstOrDefault(e => e.Status == EnumDetectionSchemeStatus.已发送待确定);
            if (scheme == null)
            {
                return false;
            }
            var x = scheme.Contracts.FirstOrDefault(e =>  e.Status == EnumContractStatus.未签订);
            if (x == null)
            {
                throw new Exception("无待签合同");
            }
            x.Status = EnumContractStatus.已签定;
            Db.SaveChanges();

            //双方确定方案才确定
            var c = scheme.Contracts.Count(e => e.Status == EnumContractStatus.已签定);
            if (c == 2)
            {
                scheme.Status = EnumDetectionSchemeStatus.已确定;
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
        public JsonResult Modify(long tradeid, string modify)
        {
            var trade = Db.Trades.Find(tradeid);
            var scheme = trade.Schemes.FirstOrDefault(e => e.Status == EnumDetectionSchemeStatus.已发送待确定);
            if (scheme == null)
            {
               throw new Exception("访问错误，请返回刷新!");
            }
            var userid = User.Identity.GetUserId();
            var x = scheme.Contracts.FirstOrDefault(e =>e.UserId== userid&&e.Status == EnumContractStatus.未签订);
            x.Status = EnumContractStatus.修改后未审核;
            Db.SaveChanges();
            scheme.Status = EnumDetectionSchemeStatus.返回修改;
            Db.SaveChanges();

            var mod = new Db.ContractModification();
            mod.DetectionScheme = scheme;
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
        public ActionResult CheckModify(long tradeid)
        {
            var trade = Db.Trades.Find(tradeid);
            var dec = trade.Schemes.FirstOrDefault(e => e.Status == EnumDetectionSchemeStatus.返回修改);
            if (dec == null)
            {
                throw new Exception("访问错误，请返回刷新!");
            }

            {
                var pro = dec.Product;

                var company = Db.Companies.FirstOrDefault(e => e.UserId == pro.UserId);

                ViewBag.productname = pro.Name;
                ViewBag.company = company.Name;
                var list = trade.Batches;
                ViewBag.model = dec;
                ViewBag.list = list;

                var modify = dec.Modifications;
                ViewBag.mlist = modify;
                ViewBag.ok = 1;
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
                Trade=x.Trade,
                Level = x.Level,
                MaxQuote = x.MaxQuote,
                MaxTime = x.MaxTime,
                MinQuote = x.MinQuote,
                MinTime = x.MinTime,
                OrganQuote = qother,
                Product = x.Product,
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
        public ActionResult CompactSign(long tradeid)
        {

            var trade = Db.Trades.Find(tradeid);
            if (trade == null)
            {
                throw new Exception("访问错误！");
            }
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
            ViewBag.FirstParty = "张三";
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
            ViewBag.checknum = trade.Id;
            return View();
        }
        public bool BoolCompactSign(long tradeid)
        {

            var trade = Db.Trades.Find(tradeid);
            if (trade == null)
            {
                throw new Exception("访问错误！");
            }
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

            Sign(tradeid);//合同详细信息
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
            c.Trade = trade;
            Db.Compacts.Add(c);
            Db.SaveChanges();

            return true;
        }

        public int CheckName(string name,long tradeid)
        {
            var n = User.Identity.GetUserName();
            if (name == n)
            {
                BoolCompactSign(tradeid);
                return 1;
            }
            return 0;
        }


    }
}