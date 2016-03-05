using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using org.in2bits.MyXls;
using QualityControl.Db;
using QualityControl.Enum;
using ThoughtWorks.QRCode.Codec;
using ZXing;
using ZXing.Common;

namespace QualityControl.Controllers
{
    public class MakeQrCodeController : BaseController
    {
        // GET: MakeQrCode
        public ActionResult Index(long tradeid)
        {
            var trade = Db.Trades.Find(tradeid);
            if (trade == null)
            {
                throw new Exception("访问错误！");
            }
            var listdown = new List<Qr>();
            var list = trade.Batches;
            list.ForEach(e =>
            {
                for (long i = 1; i <= e.Count; i++)
                {
                    listdown.Add(MakeQrCode(tradeid, i, e.BatchName, e.ProductId));
                }
            });

            BatchQrcode(tradeid);

            //压缩
            var urlz = HttpRuntime.AppDomainAppPath;
            var zipfile = urlz + "Image\\" + tradeid;
            var zipname = urlz + "Image\\" + tradeid + ".zip";
            Zip(zipfile, zipname);

            ViewBag.zipurl = "/Image/" + tradeid + ".zip";
            var url = Request.Url.ToString();
            ViewBag.list = listdown;
            MakeQrCodeFinish(tradeid);


            var userId = User.Identity.GetUserId();
            var user = UserManager.FindById(userId);
            if (user.Type == (int) EnumUserType.Producer)
            {
                ViewBag.t = 1;
            }
            ViewBag.tid = tradeid;
            return View();
        }

        public Qr MakeQrCode(long tradeid, long num, string batch, long productid)
        {
            var guid = getString(4);
            var name = tradeid + "_" + productid + "_" + batch + "_" + num + ".jpg";
            var url = HttpRuntime.AppDomainAppPath;
            var path = url + "\\Image\\" + tradeid + "\\" + batch;
            if (System.IO.File.Exists(path + "\\" + name))
            {
                var r = new Qr();
                r.name = tradeid + "_" + productid + "_" + batch + "_" + num;
                r.url = "/Image/" + tradeid + "/" + batch + "/" + name;
                ;
                r.code =
                    Db.QrCodeInfos.FirstOrDefault(
                        e => e.TradeId == tradeid && e.QrName == tradeid + "_" + productid + "_" + batch + "_" + num)
                        .IdCode;
                return r;
            }

            var encoder = new QRCodeEncoder();
            encoder.QRCodeEncodeMode = QRCodeEncoder.ENCODE_MODE.BYTE; //编码方法
            encoder.QRCodeScale = 4; //大小
            encoder.QRCodeVersion = 0; //版本
            encoder.QRCodeErrorCorrect = QRCodeEncoder.ERROR_CORRECTION.M;
            var qrdata = name + "_" + guid;
            var bp = encoder.Encode(qrdata, Encoding.UTF8);
            Image image = bp;

            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }


            image.Save(path + "\\" + name, ImageFormat.Jpeg);


            var qr = new QrCodeInfo();
            qr.IdCode = guid;
            qr.QrName = tradeid + "_" + productid + "_" + batch + "_" + num;
            qr.TradeId = tradeid;
            Db.QrCodeInfos.Add(qr);
            Db.SaveChanges();

            //条形码
            MakeBarCode(guid, url + "\\Image\\" + tradeid + "\\BarCode" + batch,
                tradeid + "_" + productid + "_" + batch + "_" + num);

            var down = "/Image/" + tradeid + "/" + batch + "/" + name;

            var ret = new Qr();
            ret.name = tradeid + "_" + productid + "_" + batch + "_" + num;
            ret.url = down;
            ret.code = guid;
            return ret;
        }


        public bool BatchQrcode(long id)
        {
            var trade = Db.Trades.Find(id);
            var bs = trade.Batches;
            bs.ForEach(e => { MakeBatchQrCode(id, e.Id); });
            return true;
        }

        public string MakeBatchQrCode(long tradeid, long batchid)
        {
            var guid = getString(4);
            var name = batchid + ".jpg";
            var url = HttpRuntime.AppDomainAppPath;
            var path = url + "Image\\" + tradeid + "\\" + "BatchsQrcode";
            if (System.IO.File.Exists(path + "\\" + name))
            {
                return "/Image/" + tradeid + "/" + "BatchsQrcode" + "/" + name;
            }

            var encoder = new QRCodeEncoder();
            encoder.QRCodeEncodeMode = QRCodeEncoder.ENCODE_MODE.BYTE; //编码方法
            encoder.QRCodeScale = 4; //大小
            encoder.QRCodeVersion = 0; //版本
            encoder.QRCodeErrorCorrect = QRCodeEncoder.ERROR_CORRECTION.M;
            var qrdata = Request.Url.GetLeftPart(UriPartial.Authority) + "/Trade/GetBatchStatus/" + batchid;
            var bp = encoder.Encode(qrdata, Encoding.UTF8);
            Image image = bp;

            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }


            image.Save(path + "\\" + name, ImageFormat.Jpeg);


            var down = "/Image/" + tradeid + "/" + "BatchsQrcode" + "/" + name;
            return down;
        }

        /// <summary>
        ///     随机字符串
        /// </summary>
        /// <param name="count"></param>
        /// <returns></returns>
        public string getString(int count)
        {
            var rad = new Random(); //实例化随机数产生器rad；
            var value = rad.Next(1000, 10000); //用rad生成大于等于1000，小于等于9999的随机数；
            return value.ToString();
        }

        public bool Zip(string zipfile, string zipname)
        {
            Models.Zip.PackageFolder(zipfile, zipname, true);
            return true;
        }

        public bool MakeQrCodeFinish(long id)
        {
            var userId = User.Identity.GetUserId();
            var user = Db.Users.Find(userId);
            var trade = Db.Trades.Find(id);
            if (trade == null)
            {
                return false;
            }
            if (user.Type == (int) EnumUserType.Controller)
            {
                trade.Status = (int) EnumTradeStatus.FinishMakeQrCode;
            }
            else
            {
                return false;
            }
            Db.Entry(trade).State = EntityState.Modified;
            Db.SaveChanges();
            return true;
        }

        /// <summary>
        ///     生成控制码
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult MakeNo(long id)
        {
            var trade = Db.Trades.Find(id);
            if (trade == null)
            {
                throw new Exception("访问错误！");
            }
            if (trade.Status != (int) EnumTradeStatus.FinishMakeQrCode)
            {
                throw new Exception("访问错误！");
            }
            var listdown = new List<ListB>();
            var list = trade.Batches;
            list.ForEach(e =>
            {
                var lists = new List<string>();
                for (long i = 1; i <= e.Count; i++)
                {
                    lists.Add(id + "_" + e.ProductId + "_" + e.BatchName + "_" + i.ToString());
                }
                listdown.Add(new ListB {BName = e.BatchName, Value = lists});
            });
            Out(id, listdown);
            return View(id);
        }


        public void MakeBarCode(string s, string dic, string name)
        {
            var op = new EncodingOptions();
            op.Width = 400;
            op.Height = 200;
            var writer = new BarcodeWriter();
            writer.Options = op;
            writer.Format = BarcodeFormat.CODE_39;

            var img = writer.Write(s);

            //自动保存图片到当前目录
            var fullUrl = dic + "\\";
            if (Directory.Exists(fullUrl) == false)
            {
                Directory.CreateDirectory(fullUrl); //如果文件夹不存在，直接创建文件夹。
            }
            var filename = fullUrl + name + ".jpg";
            img.Save(filename, ImageFormat.Jpeg);
        }


        public void Out(long id, List<ListB> list)
        {
            var bcount = (ushort) list.Count;
            //生成Excel开始

            var xls = new XlsDocument(); //创建空xls文档

            xls.FileName = Server.MapPath("~/Excel/" + id + ".xls"); //保存路径，如果直接发送到客户端的话只需要名称 生成名称

            var sheet = xls.Workbook.Worksheets.AddNamed("No"); //创建一个工作页为Dome            

            //设置文档列属性
            var cinfo = new ColumnInfo(xls, sheet); //设置xls文档的指定工作页的列属性
            cinfo.Collapsed = true;
            //设置列的范围 如 0列-10列
            cinfo.ColumnIndexStart = 0; //列开始
            cinfo.ColumnIndexEnd = bcount; //列结束
            cinfo.Collapsed = true;
            cinfo.Width = 90*60; //列宽度
            sheet.AddColumnInfo(cinfo);
            //设置文档列属性结束

            //设置指定工作页跨行跨列
            var ma = new MergeArea(1, 1, 1, 5); //从第1行跨到第二行，从第一列跨到第5列
            sheet.AddMergeArea(ma);
            //设置指定工作页跨行跨列结束

            //创建列样式创建列时引用
            var cellXF = xls.NewXF();
            cellXF.VerticalAlignment = VerticalAlignments.Centered;
            cellXF.HorizontalAlignment = HorizontalAlignments.Centered;
            cellXF.Font.Height = 24*12;
            cellXF.Font.Bold = true;
            cellXF.Pattern = 1; //设定单元格填充风格。如果设定为0，则是纯色填充
            cellXF.PatternBackgroundColor = Colors.Red; //填充的背景底色
            cellXF.PatternColor = Colors.Green; //设定填充线条的颜色
            //创建列样式结束

            //创建列
            var cells = sheet.Cells; //获得指定工作页列集合
            //列操作基本
            var cell = cells.Add(1, 1, "序列码", cellXF); //添加标题列返回一个列  参数：行 列 名称 样式对象
            //设置XY居中
            cell.HorizontalAlignment = HorizontalAlignments.Centered;
            cell.VerticalAlignment = VerticalAlignments.Centered;
            //设置字体
            cell.Font.Bold = true; //设置粗体
            cell.Font.ColorIndex = 0; //设置颜色码          
            cell.Font.FontFamily = FontFamilies.Roman; //设置字体 默认为宋体              
            //创建列结束  
            var x = 1;
            list.ForEach(e =>
            {
                //创建列表头
                var title = cells.Add(2, x, e.BName);
                title.HorizontalAlignment = HorizontalAlignments.Right;
                title.VerticalAlignment = VerticalAlignments.Centered;

                var i = 0;
                e.Value.ForEach(a =>
                {
                    cells.Add(i + 3, x, a);
                    i++;
                });
                x++;
            });

            //生成保存到服务器如果存在不会覆盖并且报异常所以先删除在保存新的
            System.IO.File.Delete(Server.MapPath("~/Excel/" + id + ".xls")); //删除
            //保存文档
            xls.Save(); //保存到服务器
        }

        public class ListB
        {
            public string BName;
            public List<string> Value;
        }

        public class Qr
        {
            public string code;
            public string name;
            public string url;
        }
    }
}