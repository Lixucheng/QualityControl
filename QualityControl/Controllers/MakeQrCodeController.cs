﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Text;
using System.Web;
using System.Web.Mvc;
using QualityControl.Db;
using ThoughtWorks.QRCode.Codec;

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

            //压缩
            var urlz = HttpRuntime.AppDomainAppPath;
            var zipfile = urlz + "Image\\" + tradeid;
            var zipname = urlz + "Image\\" + tradeid + ".zip";
            Zip(zipfile, zipname);

            ViewBag.zipurl = "/Image/" + tradeid + ".zip";
            var url = Request.Url.ToString();
            ViewBag.list = listdown;
            return View();
        }
        
        public string MakeQrCode(long tradeid, long num, string batch, long productid)
        {
            var guid = Guid.NewGuid().ToString();
            var name = tradeid + "_" + productid + "_" + batch + "_" + num + ".jpg";
            var url = HttpRuntime.AppDomainAppPath;
            var encoder = new QRCodeEncoder();
            encoder.QRCodeEncodeMode = QRCodeEncoder.ENCODE_MODE.BYTE; //编码方法
            encoder.QRCodeScale = 4; //大小
            encoder.QRCodeVersion = 0; //版本
            encoder.QRCodeErrorCorrect = QRCodeEncoder.ERROR_CORRECTION.M;
            var qrdata = name + "_" + guid;
            var bp = encoder.Encode(qrdata, Encoding.UTF8);
            Image image = bp;
            var path = url + "\\Image\\" + tradeid + "\\" + batch;
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }


            image.Save(path + "\\" + name, ImageFormat.Jpeg);


            var qr = new QrCodeInfo();
            qr.IdCode = guid;
            qr.QrName = name;
            Db.QrCodeInfos.Add(qr);
            Db.SaveChanges();


            var down = "/Image/" + tradeid + "/" + batch + "/" + name;
            return down;
        }

        public bool Zip(string zipfile, string zipname)
        {
            Models.Zip.PackageFolder(zipfile, zipname, true);
            return true;
        }
    }
}