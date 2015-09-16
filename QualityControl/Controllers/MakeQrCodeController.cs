using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using ThoughtWorks.QRCode;
using ThoughtWorks.QRCode.Codec;

namespace QualityControl.Controllers
{
    public class MakeQrCodeController : Controller
    {
        // GET: MakeQrCode
        public ActionResult Index()
        {
            var url = HttpRuntime.AppDomainAppPath;
            ThoughtWorks.QRCode.Codec.QRCodeEncoder encoder = new QRCodeEncoder();
            encoder.QRCodeEncodeMode = QRCodeEncoder.ENCODE_MODE.BYTE;//编码方法
            encoder.QRCodeScale = 4;//大小
            encoder.QRCodeVersion = 4;//版本
            encoder.QRCodeErrorCorrect = QRCodeEncoder.ERROR_CORRECTION.M;
            String qrdata = "内容";
            System.Drawing.Bitmap bp = encoder.Encode(qrdata, ASCIIEncoding.UTF8);
            Image image = bp;
            image.Save(url+"\\"+"image"+".jpg",ImageFormat.Jpeg);
            return View();
        }
    }
}