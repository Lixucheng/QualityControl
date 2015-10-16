﻿using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Newtonsoft.Json;
using QualityControl.Db;
using QualityControl.Enum;
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

        public ActionResult Trades()
        {
            return View(Db.Trades.Join(Db.Users, a => a.UserId, a => a.Id, (trade, user) => new TradeInfo
            {
                Trade = trade,
                User = user
            }).ToList());
        }


        public ActionResult SelectSample(long id)
        {
            var trade = Db.Trades.Find(id);
            if (trade == null)
            {
                return Content("错误操作");
            }
            return View(trade);
        }

        /// <summary>
        ///     抽样
        /// </summary>
        /// <param name="tradeId"></param>
        /// <param name="type"></param>
        /// <returns></returns>
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
                    b.SampleCount = countData.FirstOrDefault(a => a.Id == b.Id).SampleCount;
                    var qrCodes = new List<string>();
                    var divider = b.Count/b.SampleCount;
                    var num = random.Next(0, b.SampleCount);
                    for (var i = 0; i < b.SampleCount; i++)
                    {
                        num += i*b.SampleCount;
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
            trade.Status = (int) EnumTradeStatus.Testing;
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

        public ActionResult MakeQrCodeFinish(long id)
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
                trade.Status = (int) EnumTradeStatus.SampleStart;
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
            if (trade.UserId != userId && trade.SgsUserId != userId && user.Type != (int) EnumUserType.Controller)
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
    }
}