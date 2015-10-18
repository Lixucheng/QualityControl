using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Newtonsoft.Json;
using QualityControl.Db;
using QualityControl.Enum;
using QualityControl.Models;

namespace QualityControl.Controllers
{
    [Authorize]
    public class DetectionSchemeController : BaseController
    {
        //检测方案合同部分 1121112231
        // GET: DetectionScheme
        public ActionResult BuildDetectionScheme(long tradeid)
        {
            var trade = Db.Trades.Find(tradeid);
            if (trade == null)
            {
                ViewBag.ok = 1;
                ViewBag.message = "访问错误，订单不存在！";
            }

            var x = new DetectionScheme();
            var list = trade.Batches;
            if (trade.Schemes == null)
            {
                trade.Schemes = new List<DetectionScheme>();
                x = null;
            }
            else
            {
                x =
                    Db.DetectionSchemes.FirstOrDefault(
                        e => e.Status != EnumDetectionSchemeStatus.修改完成留档保存 && e.Trade.Id == tradeid);
            }
            var tradeProduct = JsonConvert.DeserializeObject<ProductCopy>(trade.Product);
            var sgsprolist = Db.SgsProducts.Where(e => e.Product.Id == tradeProduct.Id);
            if (!sgsprolist.Any())
            {
                ViewBag.ok = 1;
                ViewBag.message = "没有检测机构可以检测此产品！";
                return View();
            }
            ViewBag.sgslist = sgsprolist.Select(e => e.SGS).Distinct();

            if (x == null)
            {
                //方案
                x = new DetectionScheme
                {
                    MaxQuote = sgsprolist.Max(e => e.Price),
                    MinQuote = sgsprolist.Min(e => e.Price),
                    MaxTime = sgsprolist.Max(e => e.NeedeDay),
                    MinTime = sgsprolist.Min(e => e.NeedeDay),
                    Status = EnumDetectionSchemeStatus.未发送,
                    Trade = trade
                };
                Db.DetectionSchemes.Add(x);
                trade.Status = (int) EnumTradeStatus.AlreadyApply;
                Db.SaveChanges();


                var pro = JsonConvert.DeserializeObject<ProductCopy>(x.Trade.Product);
                var company = Db.Companies.FirstOrDefault(e => e.UserId == pro.UserId);
                ViewBag.productname = pro.Name;
                ViewBag.company = company.Name;


                ViewBag.model = x;
                ViewBag.list = list;
            }
            else if (x.Status == EnumDetectionSchemeStatus.未发送)
            {
                var pro = JsonConvert.DeserializeObject<ProductCopy>(x.Trade.Product);
                var company = Db.Companies.FirstOrDefault(e => e.UserId == pro.UserId);
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
                ViewBag.ok = 1;
                ViewBag.message = "方案已发送待确定或者已确定，不支持编辑！";
                return View();
            }
            var levelconvert = new ConvertLevel();

            ViewBag.Level = levelconvert.GetLevelByCount(list.First().Count);
            ViewBag.tradeid = tradeid;
            return View();
        }

        public JsonResult GetLevel(long tradeid, int s2)
        {
            var trade = Db.Trades.Find(tradeid);
            var levelconvert = new ConvertLevel();
            var l = levelconvert.GetLevel(trade.Batches.First().Count, s2);
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
            if (userid != trade.UserId && userid != trade.SgsUserId)
            {
                throw new Exception("访问错误！");
            }
            var see = trade.Schemes.FirstOrDefault(e => e.Status == EnumDetectionSchemeStatus.已确定);
            if (see != null)
            {
                return Redirect("SeeContract?tradeid=" + tradeid);
            }

            var x = trade.Schemes.FirstOrDefault(e => e.Status == EnumDetectionSchemeStatus.已发送待确定);
            if (x == null)
            {
                ViewBag.ok = 0;
                ViewBag.message = "合同不在编辑状态,拒绝访问！";
            }
            else
            {
                var usernow = UserManager.FindById(userid);
                ViewBag.u = usernow.Type == (int) EnumUserType.User ? 0 : 1;

                ViewBag.model = x;
                var pro = JsonConvert.DeserializeObject<ProductCopy>(x.Trade.Product);
                var company = Db.Companies.FirstOrDefault(e => e.UserId == pro.UserId);
                ViewBag.productname = pro.Name;
                ViewBag.company = company.Name;
                var list = trade.Batches;
                ViewBag.list = list;


                var detectionscheme = trade.Schemes.FirstOrDefault(e => e.Status == EnumDetectionSchemeStatus.已发送待确定);
                if (detectionscheme == null)
                {
                    throw new Exception("没有待确认合同！");
                }
                var haveAllreadysend =
                    detectionscheme.Contracts.FirstOrDefault(
                        e => e.UserId == userid && e.Status == EnumContractStatus.修改后未审核);
                if (haveAllreadysend != null)
                {
                    ViewBag.ok = 0;
                    ViewBag.message = "合同已经在修改中,等待管控中心再次制定方案！";
                }
                var have =
                    detectionscheme.Contracts.FirstOrDefault(
                        e => e.UserId == userid && e.Status == EnumContractStatus.未签订);
                if (have == null)
                {
                    var user = UserManager.FindById(User.Identity.GetUserId());
                    double quote = 0;
                    quote = user.Type == (int) EnumUserType.User ? x.UserQuote : x.OrganQuote;
                    var c = new Contract
                    {
                        DetectionScheme = detectionscheme,
                        Level = x.Level,
                        Time = x.Time,
                        Product =
                            JsonConvert.DeserializeObject<ProductCopy>(detectionscheme.Trade.Product).ToProductDbOject(),
                        Quote = quote,
                        Status = EnumContractStatus.未签订,
                        UserId = user.Id
                    };
                    Db.Contracts.Add(c);
                    Db.SaveChanges();
                }

                var s = x.Level;
                var l = JsonConvert.DeserializeObject<Level>(s);
                ViewBag.l = l;
            }
            ViewBag.disable = 0; //可编辑
            return View();
        }

        /// <summary>
        ///     查看已经确定的
        /// </summary>
        /// <param name="tradeid"></param>
        /// <returns></returns>
        public ActionResult SeeContract(long tradeid)
        {
            var userid = User.Identity.GetUserId();
            var trade = Db.Trades.Find(tradeid);
            var x = trade.Schemes.FirstOrDefault(e => e.Status == EnumDetectionSchemeStatus.已确定);

            var usernow = UserManager.FindById(userid);
            ViewBag.u = usernow.Type == (int) EnumUserType.User ? 0 : 1;
            ViewBag.model = x;
            var pro = JsonConvert.DeserializeObject<ProductCopy>(x.Trade.Product);
            var company = Db.Companies.FirstOrDefault(e => e.UserId == pro.UserId);
            ViewBag.productname = pro.Name;
            ViewBag.company = company.Name;
            var list = trade.Batches;
            ViewBag.list = list;
            var s = x.Level;
            var l = JsonConvert.DeserializeObject<Level>(s);
            ViewBag.l = l;


            ViewBag.disable = 1; //不可编辑
            return View("SignContract");
        }

        /// <summary>
        ///     将合同发送给两方
        ///     将合同发送给两方
        /// </summary>
        /// <param name="checknum"></param>
        /// <param name="quser"></param>
        /// <param name="qother"></param>
        /// <param name="time"></param>
        /// <returns></returns>
        public JsonResult SendDetectionScheme(long tradeid, double quser, double qother, int time, int l1, int l2,
            string l3, long sgsid)
        {
            var trade = Db.Trades.Find(tradeid);
            var sgsuserid = Db.SGSs.Find(sgsid).UserId;

            trade.SgsUserId = sgsuserid;
            Db.SaveChanges();

            var x = trade.Schemes.FirstOrDefault(e => e.Status == EnumDetectionSchemeStatus.未发送);
            x.Level = JsonConvert.SerializeObject(new {l1, l2, l3});
            x.UserQuote = quser;
            x.OrganQuote = qother;
            x.Time = time;
            x.Status = EnumDetectionSchemeStatus.已发送待确定;
            Db.SaveChanges();
            //发送站内信息
            SendMessage(trade.UserId, "合同已发送，请查看！");
            SendMessage(trade.SgsUserId, "合同已发送，请查看！");

            return Json(1);
        }


        /// <summary>
        ///     签合同
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
            var userid = User.Identity.GetUserId();
            var x = scheme.Contracts.FirstOrDefault(e => e.UserId == userid && e.Status == EnumContractStatus.未签订);
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
                scheme.Trade.Status = (int) EnumTradeStatus.Signed;
                Db.SaveChanges();
                //发送站内信息
                SendMessage(trade.UserId, "合同双方已签订，随后进入付款流程");
                SendMessage(trade.SgsUserId, "合同双方已签订，随后进入付款流程");
            }

            return true;
        }

        /// <summary>
        ///     提出修改
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
            var x = scheme.Contracts.FirstOrDefault(e => e.UserId == userid && e.Status == EnumContractStatus.未签订);
            x.Status = EnumContractStatus.修改后未审核;
            Db.SaveChanges();
            scheme.Status = EnumDetectionSchemeStatus.返回修改;
            Db.SaveChanges();

            var mod = new ContractModification();
            mod.DetectionScheme = scheme;
            mod.Modify = modify;
            mod.UserId = User.Identity.GetUserId();
            Db.ContractModifications.Add(mod);
            Db.SaveChanges();
            //发送站内信息
            SendMessage(trade.UserId, "用户提出合同修改意见，之前的合同已失效，等待管控中心审核并下发新合同！");
            SendMessage(trade.SgsUserId, "用户提出合同修改意见，之前的合同已失效，等待管控中心审核并下发新合同！");

            return Json(1);
        }

        /// <summary>
        ///     审查修改
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
                var pro = JsonConvert.DeserializeObject<ProductCopy>(dec.Trade.Product);

                var company = Db.Companies.FirstOrDefault(e => e.UserId == pro.UserId);

                ViewBag.productname = pro.Name;
                ViewBag.company = company.Name;
                var list = trade.Batches;
                ViewBag.model = dec;
                ViewBag.list = list;

                var modify = dec.Modifications.Select(e =>
                {
                    var u = UserManager.FindById(e.UserId);
                    if (u.Type == (int) EnumUserType.User)
                        return new ModifyWithName {Modify = e.Modify, User = "用户（" + u.UserName + ")"};
                    return new ModifyWithName {Modify = e.Modify, User = "检测中心（" + u.UserName + ")"};
                }).ToList();

                ViewBag.mlist = modify;
                ViewBag.ok = 1;
            }

            return View();
        }

        /// <summary>
        ///     修改待再次发送给双方
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
                Trade = x.Trade,
                Level = x.Level,
                MaxQuote = x.MaxQuote,
                MaxTime = x.MaxTime,
                MinQuote = x.MinQuote,
                MinTime = x.MinTime,
                OrganQuote = qother,
                Time = time,
                UserQuote = quser,
                Status = EnumDetectionSchemeStatus.已发送待确定
            };
            Db.DetectionSchemes.Add(n);
            Db.SaveChanges();
            //发送站内信息
            SendMessage(x.Trade.UserId, "修改后的合同已下发，请注意查看！");
            SendMessage(x.Trade.SgsUserId, "修改后的合同已下发，请注意查看！");
            return Json(1);
        }

        /// <summary>
        ///     协议内容，需签订
        /// </summary>
        [HttpGet]
        public ActionResult CompactSign(long tradeid)
        {
            var trade = Db.Trades.Find(tradeid);
            if (trade == null)
            {
                throw new Exception("访问错误！");
            }
            var userid = User.Identity.GetUserId();
            if (userid != trade.UserId && userid != trade.SgsUserId)
            {
                throw new Exception("无权限查看！");
            }
            var s0 = "一、";

            var s2 = @"（以下简称检验方）接受委托方书面检验委托。委托检验申请单（以下简称委托单）作为本协议的附件。本协议以委托方代表人在委托单上签名盖章，检验方加盖受理骑缝章后生效。 
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
            var s1 = "";
            var sysid = trade.SgsUserId;

            var user = UserManager.FindById(User.Identity.GetUserId());
            if (user.Type == (int) EnumUserType.User)
            {
                ViewBag.FirstParty = user.UserName;
                s1 = "管控中心";
                ViewBag.SecondParty = Db.SGSs.FirstOrDefault(e => e.UserId == sysid).Name;
                ;
                ViewBag.user = 1;
            }
            else if (user.Type == (int) EnumUserType.TestingOrg)
            {
                ViewBag.FirstParty = "管控中心";
                ViewBag.SecondParty = Db.SGSs.FirstOrDefault(e => e.UserId == sysid).Name;
                s1 = Db.SGSs.FirstOrDefault(e => e.UserId == sysid).Name;
                ViewBag.user = 2;
            }
            else
            {
                throw new Exception("无权查看！");
            }

            ViewBag.time = DateTime.Today.ToShortDateString();
            ViewBag.Content = s0 + s1 + s2;
            ViewBag.checknum = trade.Id;
            ViewBag.TradeId = trade.Id;
            return View();
        }

        public bool BoolCompactSign(long tradeid)
        {
            var trade = Db.Trades.Find(tradeid);
            if (trade == null)
            {
                throw new Exception("访问错误！");
            }
            var s0 = "一、";

            var s2 = @"（以下简称检验方）接受委托方书面检验委托。委托检验申请单（以下简称委托单）作为本协议的附件。本协议以委托方代表人在委托单上签名盖章，检验方加盖受理骑缝章后生效。 
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
            var s1 = "管控中心";

            Sign(tradeid); //合同详细信息
            var c = new Compact();
            var sysid = trade.SgsUserId;

            var user = UserManager.FindById(User.Identity.GetUserId());
            if (user.Type == (int) EnumUserType.User)
            {
                c.FirstParty = user.UserName;
                c.SecondParty = Db.SGSs.FirstOrDefault(e => e.UserId == sysid).Name;
                ;
            }
            else if (user.Type == (int) EnumUserType.TestingOrg)
            {
                c.FirstParty = "管控中心";
                c.SecondParty = Db.SGSs.FirstOrDefault(e => e.UserId == sysid).Name;
            }
            else
            {
                throw new Exception("无权查看！");
            }
            c.Content = s0 + s1 + s2;
            c.Time = DateTime.Now;
            Db.Compacts.Add(c);
            Db.SaveChanges();
            trade.Compact = c;
            Db.SaveChanges();
            return true;
        }

        public int CheckName(string name, long tradeid)
        {
            var n = User.Identity.GetUserName();
            if (name == n)
            {
                BoolCompactSign(tradeid);
                return 1;
            }
            return 0;
        }


        public bool SendMessage(string userid, string content)
        {
            var m = new Message
            {
                Status = (int) EnumMeaasgeStatus.Unread,
                Time = DateTime.Now,
                Content = content,
                UserId = userid
            };
            Db.Messages.Add(m);
            Db.SaveChanges();
            return true;
        }


        public void test(long id)
        {
            var x = Db.Trades.Find(id);
            for (var i = 0; i < 3; i++)
            {
                var b = new ProductBatch();
                b.BatchName = "2015-9-" + i + "批次";
                b.Count = 5;
                b.ProductId = JsonConvert.DeserializeObject<ProductCopy>(x.Product).Id;
                b.Trade = x;
                Db.ProductBatchs.Add(b);
                Db.SaveChanges();
            }
        }

        public int ConfirmPay(long tradeId, int type)
        {
            var trade = Db.Trades.Find(tradeId);
            if (trade == null)
            {
                return 0;
            }
            if (trade.Status != (int) EnumTradeStatus.Signed)
            {
                return 0;
            }
            if (type == 0)
            {
                trade.Status = (int) EnumTradeStatus.MakeQrCode;
            }
            else
            {
                trade.SGSPaied = true;
            }
            Db.Entry(trade).State = EntityState.Modified;
            Db.SaveChanges();
            return 1;
        }


        public class Level
        {
            public int l1;
            public int l2;
            public string l3;

            public string L1 => l1 == 0 ? "特殊检测等级" : "一般检查等级";

            public string L2
            {
                get
                {
                    switch (l2)
                    {
                        case 1:
                            return "S-1";
                        case 2:
                            return "S-2";
                        case 3:
                            return "S-3";
                        case 4:
                            return "S-4";
                        case 11:
                            return "I";
                        case 12:
                            return "Ⅱ";
                        case 13:
                            return "Ⅲ";
                        default:
                            return null;
                    }
                }
            }
        }

        public class ModifyWithName
        {
            public string Modify;
            public string User;
        }
    }
}