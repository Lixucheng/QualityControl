using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Text;
using System.Web;
using System.Web.Mvc;
using QualityControl.Db;
using ThoughtWorks.QRCode.Codec;
using Microsoft.AspNet.Identity;
using QualityControl.Enum;
using System.Data.Entity;
using org.in2bits.MyXls;

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
            var listdown = new List<string>();
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
            if (user.Type == (int)EnumUserType.Producer)
            {
                ViewBag.t = 1;
            }
            ViewBag.tid = tradeid;
            return View();
        }
        
        public string MakeQrCode(long tradeid, long num, string batch, long productid)
        {
            var guid = Guid.NewGuid().ToString();
            var name = tradeid + "_" + productid + "_" + batch + "_" + num + ".jpg";
            var url = HttpRuntime.AppDomainAppPath;
            var path = url + "\\Image\\" + tradeid + "\\" + batch;
            if(System.IO.File.Exists(path + "\\" + name))
            {
                return "/Image/" + tradeid + "/" + batch + "/" + name;
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


            var down = "/Image/" + tradeid + "/" + batch + "/" + name;
            return down;
        }


        public bool BatchQrcode(long id)
        {
            var trade = Db.Trades.Find(id);
            var bs = trade.Batches;
            bs.ForEach(e =>
            {
              MakeBatchQrCode(id,e.Id);
            });         
            return true;
        }
        public string MakeBatchQrCode(long tradeid, long batchid)
        {
            var guid = getString(4);
            var name = batchid + ".jpg";
            var url = HttpRuntime.AppDomainAppPath;
            var path = url + "\\Image\\" + tradeid + "\\" + "批次二维码";
            if (System.IO.File.Exists(path + "\\" + name))
            {
                return "/Image/" + tradeid + "/" + "批次二维码" + "/" + name;
            }

            var encoder = new QRCodeEncoder();
            encoder.QRCodeEncodeMode = QRCodeEncoder.ENCODE_MODE.BYTE; //编码方法
            encoder.QRCodeScale = 4; //大小
            encoder.QRCodeVersion = 0; //版本
            encoder.QRCodeErrorCorrect = QRCodeEncoder.ERROR_CORRECTION.M;
            var qrdata = Request.Url.GetLeftPart(UriPartial.Authority).ToString()+"/Trade/GetBatchStatus/" +batchid;
            var bp = encoder.Encode(qrdata, Encoding.UTF8);
            Image image = bp;

            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }


            image.Save(path + "\\" + name, ImageFormat.Jpeg);


          


            var down = "/Image/" + tradeid + "/" + "批次二维码" + "/" + name;
            return down;
        }

        /// <summary>
        /// 随机字符串
        /// </summary>
        /// <param name="count"></param>
        /// <returns></returns>
        public string getString(int count)
        {
            int number;
            string checkCode = String.Empty;     //存放随机码的字符串   

            System.Random random = new Random();

            for (int i = 0; i < count; i++) //产生4位校验码   
            {
                number = random.Next();
                number = number % 36;
                if (number < 10)
                {
                    number += 48;    //数字0-9编码在48-57   
                }
                else
                {
                    number += 55;    //字母A-Z编码在65-90   
                }

                checkCode += ((char)number).ToString();
            }
            return checkCode;
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
            if (user.Type == (int)EnumUserType.Controller)
            {
                trade.Status = (int)EnumTradeStatus.FinishMakeQrCode;
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
        /// 生成控制码
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
            if(trade.Status!=(int)EnumTradeStatus.FinishMakeQrCode)
            {
                throw new Exception("访问错误！");
            }
            var listdown = new List<ListB>();
            var list = trade.Batches;
            list.ForEach(e =>
            {
                var lists =new  List<string>();
                for (long i = 1; i <= e.Count; i++)
                {
                    lists.Add(id+"_"+e.ProductId+"_"+e.BatchName+"_"+i.ToString());
                }
                listdown.Add(new ListB { BName = e.BatchName, Value = lists });
            });
            Out(id, listdown);
            return View(id);
        }


        public void Out(long id,List<ListB> list)
        {
            ushort bcount = (ushort)list.Count;
            //生成Excel开始

            XlsDocument xls = new XlsDocument();//创建空xls文档

            xls.FileName = Server.MapPath("~/Excel/"+id+".xls");//保存路径，如果直接发送到客户端的话只需要名称 生成名称

            org.in2bits.MyXls.Worksheet sheet = xls.Workbook.Worksheets.AddNamed("No"); //创建一个工作页为Dome            

            //设置文档列属性
            ColumnInfo cinfo = new ColumnInfo(xls, sheet);//设置xls文档的指定工作页的列属性
            cinfo.Collapsed = true;
            //设置列的范围 如 0列-10列
            cinfo.ColumnIndexStart = 0;//列开始
            cinfo.ColumnIndexEnd = bcount;//列结束
            cinfo.Collapsed = true;
            cinfo.Width = 90 * 60;//列宽度
            sheet.AddColumnInfo(cinfo);
            //设置文档列属性结束

            //设置指定工作页跨行跨列
            MergeArea ma = new MergeArea(1, 1, 1, 5);//从第1行跨到第二行，从第一列跨到第5列
            sheet.AddMergeArea(ma);
            //设置指定工作页跨行跨列结束

            //创建列样式创建列时引用
            XF cellXF = xls.NewXF();
            cellXF.VerticalAlignment = VerticalAlignments.Centered;
            cellXF.HorizontalAlignment = HorizontalAlignments.Centered;
            cellXF.Font.Height = 24 * 12;
            cellXF.Font.Bold = true;
            cellXF.Pattern = 1;//设定单元格填充风格。如果设定为0，则是纯色填充
            cellXF.PatternBackgroundColor = Colors.Red;//填充的背景底色
            cellXF.PatternColor = Colors.Green;//设定填充线条的颜色
                                             //创建列样式结束

            //创建列
            Cells cells = sheet.Cells; //获得指定工作页列集合
                                       //列操作基本
            Cell cell = cells.Add(1, 1, "控制码", cellXF);//添加标题列返回一个列  参数：行 列 名称 样式对象
                                                                    //设置XY居中
            cell.HorizontalAlignment = HorizontalAlignments.Centered;
            cell.VerticalAlignment = VerticalAlignments.Centered;
            //设置字体
            cell.Font.Bold = true;//设置粗体
            cell.Font.ColorIndex = 0;//设置颜色码          
            cell.Font.FontFamily = FontFamilies.Roman;//设置字体 默认为宋体              
                                                      //创建列结束  
            int x = 1;
            list.ForEach(e=>
            {
              
                //创建列表头
                Cell title = cells.Add(2, x, e.BName);
                title.HorizontalAlignment = HorizontalAlignments.Right;
                title.VerticalAlignment = VerticalAlignments.Centered;

                int i = 0;
                e.Value.ForEach(a =>
                {                    
                    cells.Add(i + 3, x, a);
                    i++;
                });
                x++;

            });
           
            //生成保存到服务器如果存在不会覆盖并且报异常所以先删除在保存新的
            System.IO.File.Delete(Server.MapPath("~/Excel/" + id + ".xls"));//删除
                                                                //保存文档
            xls.Save();//保存到服务器

        }

        public class ListB
        {
            public string BName;
            public List<string> Value;
        }
    }
}