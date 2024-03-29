﻿using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using QualityControl.Db;
using QualityControl.Enum;
using QualityControl.Models;
using QualityControl.Models.Adapters;

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
            List<Product> x = null;
            if (ProductTypeId != 0)
            {
                var t = Db.ThirdProductTypes.Find(ProductTypeId);
                if (t == null)
                {
                    throw new Exception("访问错误");
                }
                x = t.Products.Where(e => e.Status != EnumStatus.Del).ToList();
                ViewBag.list = x;
            }
            else if (!string.IsNullOrEmpty(key))
            {
                x = Db.Products.Where(e => e.Name.Contains(key) && e.Status != EnumStatus.Del).ToList();
            }
            else
            {
                x = Db.Products.Where(e => e.Status != EnumStatus.Del).ToList();
            }
            ViewBag.list = x;
            return View();
        }

        /// <summary>
        ///     选择完产品
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult ProductChosen(long id, long count)
        {
            var userid = User.Identity.GetUserId();
            var p = Db.Products.Find(id);
            var pStr = new ProductCopy(p);
            var trade = new Trade
            {
                Product = JsonConvert.SerializeObject(pStr),
                Count = count,
                CeateTime = DateTime.Now,
                FinishTime = DateTime.Now,
                SamplingDate = DateTime.Now,
                DetectingDate = DateTime.Now,
                UserId = userid,
                Status = (int) EnumTradeStatus.Create,
                SGSPaied = false,
                ManufacturerId = p.UserId
            };
            Db.Trades.Add(trade);
            Db.SaveChanges();
            return RedirectToAction("TradeDetail", new {id = trade.Id});
        }


        /// <summary>
        ///     确认产品信息完善
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

            var p = JsonConvert.DeserializeObject<ProductCopy>(trade.Product);
            //添加检测项目
            var dlist = Db.ProductDectectionItems.Where(e => e.ProductId == p.Id).Select(a => new DectectionItemModel
            {
                Name = a.Name,
                Range = a.Range,
                Denney = a.Denney
            }
                ).ToList();

            WriteDetectionItem(id, dlist);


            trade.Status = (int) EnumTradeStatus.ProductInfoChecked;
            Db.Entry(trade).State = EntityState.Modified;
            Db.SaveChanges();
            return RedirectToAction("TradeDetail", new {id = trade.Id});
        }

        /// <summary>
        ///     审核产品信息
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
            if (user.Type != (int) EnumUserType.Controller)
            {
                return Content("错误操作");
            }
            trade.Status = (int) EnumTradeStatus.ProductInfoConfirmed;
            Db.Entry(trade).State = EntityState.Modified;
            Db.SaveChanges();
            return RedirectToAction("TradeDetail", new {id = trade.Id});
        }

        /// <summary>
        ///     选择批次
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
        ///     选择批次
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult BatchSelect(long id, List<long> batchIds)
        {
            var trade = Db.Trades.Find(id);
            var userid = User.Identity.GetUserId();
            if (trade == null || trade.Status != (int) EnumTradeStatus.ProductInfoConfirmed || trade.UserId != userid)
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
                    ProductId = batch.ProductId,
                    ProductionDate = batch.ProductionDate
                };

                batches.Add(pb);
            }
            var pStr = new ProductCopy(p);
            trade.Product = JsonConvert.SerializeObject(pStr);
            trade.Status = (int) EnumTradeStatus.BatchSelected;
            trade.Batches = batches;
            Db.Entry(trade).State = EntityState.Modified;
            Db.SaveChanges();
            return RedirectToAction("TradeDetail", new {id = trade.Id});
        }

        /// <summary>
        ///     生产商确定批次
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult BatchSelected(long id)
        {
            var trade = Db.Trades.Find(id);
            var userid = User.Identity.GetUserId();
            if (trade == null || trade.Status != (int) EnumTradeStatus.ProductInfoConfirmed ||
                trade.ManufacturerId != userid)
            {
                return Content("错误操作");
            }

            trade.Status = (int) EnumTradeStatus.BatchSelected;
            Db.Entry(trade).State = EntityState.Modified;
            Db.SaveChanges();
            return RedirectToAction("TradeDetail", new {id = trade.Id});
        }


        /// <summary>
        ///     管控合同列表
        /// </summary>
        /// <returns></returns>
        public ActionResult Trades()
        {
            return View(Db.Trades
                .Where(a => a.Status == (int) EnumTradeStatus.BatchSelected)
                .Join(Db.Users, a => a.UserId, a => a.Id, (trade, user) => new TradeInfo
                {
                    Trade = trade,
                    User = user
                }).ToList());
        }


        public ActionResult TradesOK()
        {
            return View("Trades", Db.Trades
                .Where(a => a.Status > (int) EnumTradeStatus.EnsureContract)
                .Join(Db.Users, a => a.UserId, a => a.Id, (trade, user) => new TradeInfo
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
        [HttpGet]
        public ActionResult Sample(long tradeId)
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
            var batches = trade.Batches;
            foreach (var b in batches)
            {
                b.SampleCount = LevelCount(b.Level);
            }
            return View(trade);
        }

        [HttpPost]
        public ActionResult Sample(long tradeId, EnumSample type, string data)
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

            var countData = JsonConvert.DeserializeObject<List<ProductBatch>>(data);

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
                    b.SampleCount = countData.FirstOrDefault(a => a.Id == b.Id).SampleCount;
                    for (var i = 0; i < b.SampleCount; i ++)
                    {
                        string code;
                        do
                        {
                            code = trade.Id + "_" +
                                   b.ProductId + "_" +
                                   b.BatchName + "_" +
                                   random.Next(1, b.Count);
                            // code=code+":" + Db.QrCodeInfos.FirstOrDefault(e => e.QrName==code&&e.TradeId==trade.Id).IdCode; 
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
                    b.SampleCount = countData.FirstOrDefault(a => a.Id == b.Id).SampleCount;
                    var qrCodes = new List<string>();
                    var divider = b.Count/b.SampleCount;
                    var num = random.Next(1, divider);
                    for (var i = 0; i < b.SampleCount; i++)
                    {
                        string code;
                        code = trade.Id + "_" +
                               b.ProductId + "_" +
                               b.BatchName + "_" +
                               num;
                        // code = code + ":" + Db.QrCodeInfos.FirstOrDefault(e => e.QrName == code && e.TradeId == trade.Id).IdCode;
                        qrCodes.Add(code);
                        num += divider;
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


        [AllowAnonymous]
        public ActionResult GetBatchStatus(long id)
        {
            var b = Db.ProductBatchs.Find(id);
            if (b == null)
            {
                ViewBag.Text = "该编号无效";
                ViewBag.s = 0;
            }
            else if (b.Trade.Status == (int) EnumTradeStatus.Finish)
            {
                ViewBag.Text = "已经通过检测";
                ViewBag.s = 1;
            }
            else
            {
                ViewBag.Text = "正在检测该产品";
                ViewBag.s = 2;
            }
            ViewBag.p = JsonConvert.DeserializeObject<ProductCopy>(b.Trade.Product);
            ViewBag.m = Db.Companies.First(e => e.UserId == b.Trade.ManufacturerId);
            ViewBag.bdate = b.ProductionDate;
            ViewBag.bname = b.BatchName;
            return View();
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

        [HttpGet]
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

        [HttpPost]
        public string Report(long id, string data)
        {
            var userId = User.Identity.GetUserId();
            var user = Db.Users.Find(userId);
            ViewBag.User = user;
            var trade = Db.Trades.Find(id);
            ViewBag.SGS = Db.SGSs.FirstOrDefault(a => a.UserId == trade.SgsUserId);
            ViewBag.Company = Db.Companies.FirstOrDefault(a => a.UserId == trade.ManufacturerId);
            if (trade == null)
            {
                return null;
            }
            if (user.Type != (int) EnumUserType.Controller)
            {
                return null;
            }
            var items = JsonConvert.DeserializeObject<JObject>(data);
            foreach (var i in items)
            {
                var b = trade.Batches.FirstOrDefault(a => a.Id.ToString() == i.Key);
                if (b == null)
                {
                    return null;
                }
                b.Report.DectectionItemResults = Convert.ToBoolean(i.Value);
            }
            trade.Status = (int) EnumTradeStatus.Finish;
            Db.Entry(trade).State = EntityState.Modified;
            Db.SaveChanges();
            return "ok";
        }

        [HttpGet]
        public ActionResult MakeReport(long id)
        {
            var userId = User.Identity.GetUserId();
            var user = Db.Users.Find(userId);
            ViewBag.User = user;
            var trade = Db.Trades.Find(id);
            if (trade == null || trade.SgsUserId != userId)
            {
                return Content("错误操作");
            }
            ViewBag.SGS = Db.SGSs.FirstOrDefault(a => a.UserId == trade.SgsUserId);
            ViewBag.Items = JsonConvert.DeserializeObject<List<DectectionItemModel>>(trade.DetectionItems);
            ViewBag.Company = Db.Companies.FirstOrDefault(a => a.UserId == trade.ManufacturerId);
            return View(trade);
        }

        [HttpPost]
        public string MakeReport(long tradeId, string files, List<BatchReport> batchReports)
        {
            if (!ModelState.IsValid)
            {
                return "error";
            }
            var userId = User.Identity.GetUserId();
            var trade = Db.Trades.Find(tradeId);
            if (trade == null || trade.SgsUserId != userId)
            {
                return "error";
            }
            //trade.Result = report;
            foreach (var b in trade.Batches)
            {
                b.Report = batchReports.FirstOrDefault(a => a.BatchId == b.Id);
            }
            trade.Result = new DetectionReport
            {
                Files = files,
                CreateTime = DateTime.Now
            };
            trade.Status = (int) EnumTradeStatus.Tested;
            Db.Entry(trade).State = EntityState.Modified;
            Db.SaveChanges();
            return "ok";
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
                trade.Status = (int) EnumTradeStatus.SgsReceivedSample;
            }
            Db.Entry(trade).State = EntityState.Modified;
            Db.SaveChanges();
            return RedirectToAction("TradeDetail", new {id});
        }

        public ActionResult SgsSampleReceive(long id)
        {
            var trade = Db.Trades.Find(id);
            if (trade == null)
            {
                return Content("错误操作");
            }
            var userId = User.Identity.GetUserId();
            if (trade.SgsUserId != userId)
            {
                return Content("错误操作");
            }
            trade.Status = (int) EnumTradeStatus.Testing;
            Db.SaveChanges();
            return RedirectToAction("TradeDetail", new {id});
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
            if (user.Type != (int) EnumUserType.Producer)
            {
                return Content("错误操作");
            }
            if (trade.Status != (int) EnumTradeStatus.FinishMakeQrCode)
            {
                return Content("错误操作");
            }
            trade.Status = (int) EnumTradeStatus.SampleStart;
            Db.Entry(trade).State = EntityState.Modified;
            Db.SaveChanges();
            return RedirectToAction("TradeDetail", new {id});
        }

        [HttpGet]
        public ActionResult DectectionItem(long id)
        {
            var trade = Db.Trades.Find(id);
            if (trade == null)
            {
                return Content("错误操作");
            }
            var userId = User.Identity.GetUserId();
            var user = UserManager.FindById(userId);
            if (!(trade.Status == (int) EnumTradeStatus.Create && trade.ManufacturerId == userId)
                &&
                !(trade.Status == (int) EnumTradeStatus.ProductInfoChecked && user.Type == (int) EnumUserType.Controller))
            {
                return Content("错误操作");
            }
            ViewBag.Id = id;
            var items = trade.DetectionItems != null
                ? JsonConvert.DeserializeObject<List<DectectionItemModel>>(trade.DetectionItems)
                : new List<DectectionItemModel>();
            return View(items);
        }

        [HttpPost]
        public string DectectionItem(long id, List<DectectionItemModel> items)
        {
            var trade = Db.Trades.Find(id);
            if (trade == null)
            {
                return null;
            }
            var userId = User.Identity.GetUserId();
            var user = UserManager.FindById(userId);
            if (!(trade.Status == (int) EnumTradeStatus.Create && trade.ManufacturerId == userId)
                &&
                !(trade.Status == (int) EnumTradeStatus.ProductInfoChecked && user.Type == (int) EnumUserType.Controller))
            {
                return null;
            }
            trade.DetectionItems = JsonConvert.SerializeObject(items);
            Db.Entry(trade).State = EntityState.Modified;
            Db.SaveChanges();
            return "ok";
        }


        /// <summary>
        ///     确定检测项目
        /// </summary>
        /// <param name="id"></param>
        /// <param name="items"></param>
        /// <returns></returns>
        public bool WriteDetectionItem(long id, List<DectectionItemModel> items)
        {
            var trade = Db.Trades.Find(id);
            if (trade == null)
            {
                return false;
            }
            var userId = User.Identity.GetUserId();
            var user = UserManager.FindById(userId);
            if (!(trade.Status == (int) EnumTradeStatus.Create && trade.ManufacturerId == userId)
                &&
                !(trade.Status == (int) EnumTradeStatus.ProductInfoChecked && user.Type == (int) EnumUserType.Controller))
            {
                return false;
            }
            trade.DetectionItems = JsonConvert.SerializeObject(items);
            Db.Entry(trade).State = EntityState.Modified;
            Db.SaveChanges();
            return true;
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


        public ActionResult GetFiles(long id)
        {
            var list = (JArray) JsonConvert.DeserializeObject(Db.Trades.Find(id).Files);
            ViewBag.list = list;
            return View();
        }
    }
}