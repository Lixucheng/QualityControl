using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Newtonsoft.Json;
using QualityControl.Db;
using QualityControl.Enum;
using QualityControl.Migrations;
using QualityControl.Models;
using QualityControl.Models.Adapters;
using Trade = QualityControl.Db.Trade;

namespace QualityControl.Controllers
{
    [Authorize]
    public class TradeController : BaseController
    {
        // GET: Trade

        public ActionResult Index()
        {
            return View();
        }

        /// <summary>
        ///     选择产品
        /// </summary>
        /// <param name="key"></param>
        /// <param name="ProductTypeId"></param>
        /// <returns></returns>
        public ActionResult ChooseProduct(string key = "", long ProductTypeId = 0)
        {
            var x = new List<Product>();
            if (ProductTypeId != 0)
            {
                var t = Db.ThirdProductTypes.Find(ProductTypeId);
                if (t == null)
                {
                    throw new Exception("访问错误");
                }
                x = t.Products;
                ViewBag.list = x;
                return View();
            }
            if (!string.IsNullOrEmpty(key))
                x = Db.Products.Where(e => e.Name.Contains(key)).ToList();
            x = Db.Products.Take(20).ToList();
            ViewBag.list = x;
            return View();
        }

        /// <summary>
        /// 选择完产品
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult ProductChosen(long id)
        {
            var userid = User.Identity.GetUserId();
            var p = Db.Products.Find(id);
            var pStr = new ProductCopy(p);
            var trade = new Trade
            {
                Product = JsonConvert.SerializeObject(pStr),
                CeateTime = DateTime.Now,
                FinishTime = DateTime.Now,
                SamplingDate = DateTime.Now,
                DetectingDate = DateTime.Now,
                UserId = userid,
                Status = (int)EnumTradeStatus.Create,
                SGSPaied = false,
                ManufacturerId = p.UserId
            };
            Db.Trades.Add(trade);
            Db.SaveChanges();
            return RedirectToAction("TradeDetail", new {id = trade.Id});
        }


        /// <summary>
        /// 确认产品信息完善
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult ProductInfoChecked(long id)
        {
            var trade = Db.Trades.Find(id);
            if (trade == null)
            {
                return Content("错误操作");
            }
            var userid = User.Identity.GetUserId();
            if (trade.ManufacturerId != userid)
            {
                return Content("错误操作");
            }
            trade.Status = (int)EnumTradeStatus.ProductInfoChecked;
            Db.Entry(trade).State = EntityState.Modified;
            Db.SaveChanges();
            return RedirectToAction("TradeDetail", new { id = trade.Id });
        }

        /// <summary>
        /// 审核产品信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult ProductInfoConfirmed(long id)
        {
            var trade = Db.Trades.Find(id);
            if (trade == null)
            {
                return Content("错误操作");
            }
            var userid = User.Identity.GetUserId();
            var user = UserManager.FindById(userid);
            if (user.Type != (int)EnumUserType.Controller)
            {
                return Content("错误操作");
            }
            trade.Status = (int)EnumTradeStatus.ProductInfoConfirmed;
            Db.Entry(trade).State = EntityState.Modified;
            Db.SaveChanges();
            return RedirectToAction("TradeDetail", new { id = trade.Id });
        }

        /// <summary>
        /// 选择批次
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult BatchSelect(long id)
        {
            var trade = Db.Trades.Find(id);
            if (trade == null)
            {
                return Content("错误操作");
            }
            if (User.Identity.GetUserId() != trade.UserId)
            {
                return Content("错误操作");
            }
            var p = Db.Products.Find(JsonConvert.DeserializeObject<ProductCopy>(trade.Product).Id);
            var list = p.BaseProductBatchs;
            ViewBag.list = list;
            ViewBag.p = p;
            return View();
        }


        /// <summary>
        /// 选择批次
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult BatchSelect(long id, List<long> batchIds)
        {
            var trade = Db.Trades.Find(id);
            var userid = User.Identity.GetUserId(); 
            if (trade == null || trade.Status != (int)EnumTradeStatus.ProductInfoConfirmed || trade.UserId != userid)
            {
                return Content("错误操作");
            }
            var p = Db.Products.Find(JsonConvert.DeserializeObject<ProductCopy>(trade.Product).Id);
            if (p == null)
            {
                throw new Exception("访问错误！");
            }
            var batches = new List<ProductBatch>();
            foreach (var item in batchIds)
            {
                var batch = p.BaseProductBatchs.FirstOrDefault(a => a.Id == item);
                if (batch == null)
                {
                    throw new Exception("访问错误！");
                }
                var pb = new ProductBatch
                {
                    BatchName = batch.BatchName,
                    Count = batch.Count,
                    ProductId = batch.ProductId
                };

                batches.Add(pb);
            }
            var pStr = new ProductCopy(p);
            trade.Product = JsonConvert.SerializeObject(pStr);
            trade.Status = (int) EnumTradeStatus.BatchSelected;
            trade.Batches = batches;
            Db.Entry(trade).State = EntityState.Modified;
            Db.SaveChanges();
            return RedirectToAction("TradeDetail", new { id = trade.Id });
        }

        /// <summary>
        /// 交易列表
        /// </summary>
        /// <returns></returns>
        public ActionResult Trades()
        {
            return View(Db.Trades.Join(Db.Users, a => a.UserId, a => a.Id, (trade, user) => new TradeInfo
            {
                Trade = trade,
                User = user
            }).ToList());
        }


        /// <summary>
        ///     抽样
        /// </summary>
        /// <param name="tradeId"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public ActionResult Sample(long tradeId, EnumSample type)
        {
            var trade = Db.Trades.Find(tradeId);
            if (trade == null)
            {
                return Content("警告：错误操作!");
            }
            if (trade.Status != (int) EnumTradeStatus.SampleStart)
            {
                return RedirectToAction("TradeDetail", new {id = tradeId});
            }

            trade.SampleType = type;
            Db.Entry(trade).Property(a => a.SampleType).IsModified = true;

            //抽样
            var batches = trade.Batches;
            var random = new Random();
            if (type == EnumSample.Random)
            {
                // 随机
                foreach (var b in batches)
                {
                    var qrCodes = new List<string>();
                    b.SampleCount = LevelCount(b.Level);
                    for (var i = 0; i < b.SampleCount; i ++)
                    {
                        string code;
                        do
                        {
                            code = trade.Id.ToString("D15") + "_" +
                                   b.ProductId.ToString("D10") + "_" +
                                   b.BatchName + "_" +
                                   random.Next(0, b.Count);
                        } while (qrCodes.Contains(code));
                        qrCodes.Add(code);
                    }
                    b.SamplaListJson = JsonConvert.SerializeObject(qrCodes);
                    Db.Entry(b).State = EntityState.Modified;
                }
            }
            else
            {
                foreach (var b in batches)
                {
                    b.SampleCount = LevelCount(b.Level);
                    var qrCodes = new List<string>();
                    var divider = b.Count/b.SampleCount;
                    var num = random.Next(0, b.SampleCount);
                    for (var i = 0; i < b.SampleCount; i++)
                    {
                        num += i* divider;
                        string code;
                        code = trade.Id.ToString("D15") + "_" +
                               b.ProductId.ToString("D10") + "_" +
                               b.BatchName + "_" +
                               num;
                        qrCodes.Add(code);
                    }
                    b.SamplaListJson = JsonConvert.SerializeObject(qrCodes);
                    Db.Entry(b).State = EntityState.Modified;
                }
            }
            trade.Status = (int) EnumTradeStatus.SampleReceived;
            Db.Entry(trade).State = EntityState.Modified;
            Db.Messages.Add(new Message
            {
                UserId = trade.ManufacturerId,
                Content = "你的管控合同抽样信息已经完成，请注意查看",
                Time = DateTime.Now
            });
            Db.Messages.Add(new Message
            {
                UserId = trade.SgsUserId,
                Content = "你的管控合同抽样信息已经完成，请注意查看",
                Time = DateTime.Now
            });
            Db.SaveChanges();
            return RedirectToAction("TradeDetail", new {id = tradeId});
        }

        /// <summary>
        ///     处理合格与不合格
        /// </summary>
        /// <param name="tradeId"></param>
        /// <param name="status"></param>
        /// <returns></returns>
        public string ChangeValidStatus(long tradeId, int status)
        {
            var trade = Db.Trades.Find(tradeId);
            if (trade.Status == status)
            {
                return null;
            }
            if (status == 0 || status == 1)
            {
                trade.Status = status;
            }
            Db.Entry(trade).State = EntityState.Modified;
            Db.SaveChanges();
            return null;
        }

        public ActionResult ComfirmPay(long id)
        {
            var userId = User.Identity.GetUserId();
            var user = Db.Users.Find(userId);
            var trade = Db.Trades.Find(id);
            if (trade == null)
            {
                return Content("错误操作");
            }
            if (user.Type == (int) EnumUserType.Controller)
            {
                trade.Status = (int) EnumTradeStatus.MakeQrCode;
            }
            else if (trade.SgsUserId == userId)
            {
                trade.SGSPaied = true;
            }
            else
            {
                return Content("错误操作");
            }
            Db.Entry(trade).State = EntityState.Modified;
            Db.SaveChanges();
            return RedirectToAction("TradeDetail", new {id});
        }

        public ActionResult TradeDetail(long id)
        {
            var userId = User.Identity.GetUserId();
            var user = Db.Users.Find(userId);
            ViewBag.User = user;
            var trade = Db.Trades.Find(id);
            ViewBag.SGS = Db.SGSs.FirstOrDefault(a => a.UserId == trade.SgsUserId);
            if (trade == null)
            {
                return Content("错误操作");
            }
            if (trade.UserId != userId && trade.SgsUserId != userId && trade.ManufacturerId != userId &&
                user.Type != (int) EnumUserType.Controller)
            {
                return Content("错误操作");
            }
            return View(trade);
        }

        public ActionResult Finish(long id)
        {
            var userId = User.Identity.GetUserId();
            var user = Db.Users.Find(userId);
            var trade = Db.Trades.Find(id);
            if (trade == null || user.Type != (int) EnumUserType.Controller ||
                trade.Status != (int) EnumTradeStatus.Tested)
            {
                return Content("错误操作");
            }
            trade.Status = (int) EnumTradeStatus.Finish;
            Db.Entry(trade).State = EntityState.Modified;
            Db.SaveChanges();
            return RedirectToAction("TradeDetail", new {id});
        }

        public ActionResult Report(long id)
        {
            var userId = User.Identity.GetUserId();
            var user = Db.Users.Find(userId);
            ViewBag.User = user;
            var trade = Db.Trades.Find(id);
            ViewBag.SGS = Db.SGSs.FirstOrDefault(a => a.UserId == trade.SgsUserId);
            ViewBag.Company = Db.Companies.FirstOrDefault(a => a.UserId == trade.ManufacturerId);
            if (trade == null)
            {
                return Content("错误操作");
            }
            if (trade.UserId != userId && trade.SgsUserId != userId && user.Type != (int) EnumUserType.Controller)
            {
                return Content("错误操作");
            }
            return View(trade);
        }

        [HttpGet]
        public ActionResult MakeReport(long id)
        {
            var userId = User.Identity.GetUserId();
            var user = Db.Users.Find(userId);
            ViewBag.User = user;
            var trade = Db.Trades.Find(id);
            ViewBag.SGS = Db.SGSs.FirstOrDefault(a => a.UserId == trade.SgsUserId);
            ViewBag.Company = Db.Companies.FirstOrDefault(a => a.UserId == trade.ManufacturerId);
            if (trade == null || trade.SgsUserId != userId)
            {
                return Content("错误操作");
            }
            return View(trade);
        }

        [HttpPost]
        public ActionResult MakeReport(long tradeId, DetectionReport report, DateTime SamplingDate,
            DateTime DetectingDate)
        {
            if (!ModelState.IsValid)
            {
                return RedirectToAction("MakeReport", new {id = tradeId});
            }
            var userId = User.Identity.GetUserId();
            var user = Db.Users.Find(userId);
            var trade = Db.Trades.Find(tradeId);
            if (trade == null || trade.SgsUserId != userId)
            {
                return Content("错误操作");
            }
            trade.Result = report;
            trade.Result.CompanyName = Db.Companies.FirstOrDefault(a => a.UserId == trade.ManufacturerId).Name;
            trade.Status = (int) EnumTradeStatus.Tested;
            Db.Entry(trade).State = EntityState.Modified;
            Db.SaveChanges();
            return RedirectToAction("TradeDetail", new {id = tradeId});
        }

        public ActionResult SampleReceive(long id)
        {
            var trade = Db.Trades.Find(id);
            if (trade == null)
            {
                return Content("错误操作");
            }
            var userId = User.Identity.GetUserId();
            if (trade.ManufacturerId != userId && trade.SgsUserId != userId)
            {
                return Content("错误操作");
            }
            if (trade.SampleRecevied == null)
            {
                trade.SampleRecevied = "00";
            }
            if (trade.ManufacturerId == userId)
            {
                trade.SampleRecevied = trade.SampleRecevied[0] + "1";
            }
            if (trade.SgsUserId == userId)
            {
                trade.SampleRecevied = "1" + trade.SampleRecevied[1];
            }
            if (trade.SampleRecevied == "11")
            {
                trade.Status = (int) EnumTradeStatus.Testing;
            }
            Db.Entry(trade).State = EntityState.Modified;
            Db.SaveChanges();
            return RedirectToAction("TradeDetail", new { id = id });
        }

        public ActionResult GetQrCode(long id)
        {
            var trade = Db.Trades.Find(id);
            if (trade == null)
            {
                return Content("错误操作");
            }
            var userId = User.Identity.GetUserId();
            var user = UserManager.FindById(userId);
            if (user.Type != (int)EnumUserType.Producer)
            {

                return Content("错误操作");
            }
            if(trade.Status!=(int)EnumTradeStatus.FinishMakeQrCode)
            {
                return Content("错误操作");
            }
            trade.Status = (int)EnumTradeStatus.SampleStart;
            Db.Entry(trade).State = EntityState.Modified;
            Db.SaveChanges();
            return RedirectToAction("TradeDetail", new { id = id });
        }

        public int LevelCount(string level)
        {
            switch (level)
            {
                case "A":
                    return 2;
                case "B":
                    return 3;
                case "C":
                    return 5;
                case "D":
                    return 8;
                case "E":
                    return 13;
                case "F":
                    return 20;
                case "G":
                    return 32;
                case "H":
                    return 50;
                case "J":
                    return 80;
                case "K":
                    return 125;
                case "L":
                    return 200;
                case "M":
                    return 315;
                case "N":
                    return 500;
                case "P":
                    return 800;
                case "Q":
                    return 1250;
                case "R":
                    return 2000;
            }
            return 0;
        }
    }
}