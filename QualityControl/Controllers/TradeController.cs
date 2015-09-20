using QualityControl.Db;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace QualityControl.Controllers
{
    public class TradeController : BaseController
    {
        // GET: Trade
        public ActionResult Index()
        {
            return View();
        }

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
            if (type == EnumSample.Random)
            {
                Random random = new Random();
                foreach (var b in batches)
                {
                    List<string> QrCodes = new List<string>();
                    for(var i = 0; i < b.SampleCount; i ++)
                    {
                        int code;
                        do
                        {
                            code = random.Next(0, b.Count);
                        } while (QrCodes.Contains(trade.Id.ToString("00000000000000")));
                    }
                    
                }
            }else
            {

            }
            

            Db.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}