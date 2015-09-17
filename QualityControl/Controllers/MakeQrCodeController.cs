using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using ThoughtWorks.QRCode;
using ThoughtWorks.QRCode.Codec;

namespace QualityControl.Controllers
{
    public class MakeQrCodeController : BaseController
    {
        // GET: MakeQrCode
        public ActionResult Index(string checknum)
        {
            if (string.IsNullOrEmpty(checknum))
            {
                throw new Exception("访问错误！");
            }
            var list=Db.ProductBatchs.Where(e => e.CheckNum == checknum).ToList();        
            list.ForEach(e =>
            {             
                for (long i = 1; i <= e.Count; i++)
                {
                    MakeQrCode(checknum, i, e.BatchName, e.ProductId);
                }
            });
                       
            return View();
        }

        public string MakeQrCode(string checknum,long num,string batch,long productid)
        {
            var guid = Guid.NewGuid().ToString();
            var name = checknum + "_" + productid + "_" + batch + "_" + num;
            var url = HttpRuntime.AppDomainAppPath;
            ThoughtWorks.QRCode.Codec.QRCodeEncoder encoder = new QRCodeEncoder();
            encoder.QRCodeEncodeMode = QRCodeEncoder.ENCODE_MODE.BYTE;//编码方法
            encoder.QRCodeScale = 4;//大小
            encoder.QRCodeVersion = 4;//版本
            encoder.QRCodeErrorCorrect = QRCodeEncoder.ERROR_CORRECTION.M;
            String qrdata = name+"_"+guid;
            System.Drawing.Bitmap bp = encoder.Encode(qrdata, ASCIIEncoding.UTF8);
            Image image = bp;
            var path = url + "\\Image\\" + checknum+"\\"+batch;
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }


            image.Save(path+"\\" + name + ".jpg", ImageFormat.Jpeg);

            
            return guid;
        }

    }
}