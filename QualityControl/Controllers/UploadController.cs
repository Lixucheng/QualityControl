using Microsoft.AspNet.Identity;
using Newtonsoft.Json;
using QualityControl.Db;
using QualityControl.Enum;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;


namespace QualityControl.Controllers
{
    public class UploadController : BaseController
    {
        // GET: Upload
        public string Index()
        {
            var filecollection = Request.Files;
            string err = string.Empty;
            string subFolder = string.Empty;
            string filePath = string.Empty;
            
            HttpPostedFileBase postedfile = filecollection[0];
            if (postedfile == null)
            {
                return null;
            }
            string fileName = Guid.NewGuid().ToString() + Path.GetExtension(Path.GetFileName(postedfile.FileName));
            string fullUrl = Path.Combine(Server.MapPath(@"~/upload"));
            if (Directory.Exists(fullUrl) == false)
            {
                Directory.CreateDirectory(fullUrl); //如果文件夹不存在，直接创建文件夹。
            }
            var filepath = Path.Combine(fullUrl, fileName);
            postedfile.SaveAs(filepath);
            return JsonConvert.SerializeObject(new
            {
                success = true,
                file_path = @"/upload/" + fileName
            });
        }

        public string Del(string name)
        {
            string fullUrl = Path.Combine(Server.MapPath(@"~"));
            var n = fullUrl + name;
            if (System.IO.File.Exists(n))
            {
                System.IO.File.Delete(n);
            }
            return "";
        }


    }
}