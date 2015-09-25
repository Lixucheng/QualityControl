using Newtonsoft.Json;
using QualityControl.Db;
using QualityControl.Models.Adapters;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;

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
            return View(Db.Trades.Join(Db.Users, a => a.UserId, a => a.Id, (trade, user) => new TradeInfo()
            {
                Trade = trade,
                User = user
            }).ToList());
        }

        /// <summary>
        /// 抽样
        /// </summary>
        /// <param name="tradeId"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public ActionResult SelectSampleType(long tradeId, EnumSample type)
        {
            var trade = Db.Trades.Find(tradeId);
            if (trade == null)
            {
                return Content("警告：错误操作!");
            }
            trade.SampleType = type;
            Db.Entry(trade).Property(a => a.SampleType).IsModified = true;

            //抽样
            var batches = trade.Batches;
            Random random = new Random();
            if (type == EnumSample.Random)
            {
                // 随机
                foreach (var b in batches)
                {
                    List<string> qrCodes = new List<string>();
                    for(var i = 0; i < b.SampleCount; i ++)
                    {
                        string code;
                        do
                        {
                            code = trade.Id.ToString("D15") + "_" +
                            b.ProductId.ToString("D10") + "_" +
                            b.BatchName + "_" +
                            random.Next(0, b.Count).ToString();
                        } while (qrCodes.Contains(code));
                        qrCodes.Add(code);
                    }
                    b.SamplaListJson = JsonConvert.SerializeObject(qrCodes);
                    Db.Entry(b).State = EntityState.Modified;
                }
            }else
            {
                foreach (var b in batches)
                {
                    List<string> qrCodes = new List<string>();
                    int divider = b.Count / b.SampleCount;
                    int num = random.Next(0, b.SampleCount);
                    for (var i = 0; i < b.SampleCount; i++)
                    {
                        num += i * b.SampleCount;
                        string code;
                        code = trade.Id.ToString("D15") + "_" +
                            b.ProductId.ToString("D10") + "_" +
                            b.BatchName + "_" +
                            num.ToString();
                        qrCodes.Add(code);
                    }
                    b.SamplaListJson = JsonConvert.SerializeObject(qrCodes);
                    Db.Entry(b).State = EntityState.Modified;
                }
            }
            trade.Status = (int)EnumTradeStatus.SampleFinshed;
            Db.Entry(trade).State = EntityState.Modified;
            Db.Messages.Add(new Message()
            {
                UserId = trade.ManufacturerId,
                Content = "你的管控合同抽样信息已经完成，请注意查看",
                Time = DateTime.Now
            });
            Db.Messages.Add(new Message()
            {
                UserId = trade.SgsUserId,
                Content = "你的管控合同抽样信息已经完成，请注意查看",
                Time = DateTime.Now
            });
            Db.SaveChanges();
            return RedirectToAction("Index");
        }

        /// <summary>
        /// 处理合格与不合格
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


    }
}