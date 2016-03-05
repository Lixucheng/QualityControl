using System;
using System.IO;
using Newtonsoft.Json;

namespace QualityControl.Controllers
{
    public class UploadController : BaseController
    {
        // GET: Upload
        public string Index()
        {
            var filecollection = Request.Files;
            var err = string.Empty;
            var subFolder = string.Empty;
            var filePath = string.Empty;

            var postedfile = filecollection[0];
            if (postedfile == null)
            {
                return null;
            }
            var fileName = Guid.NewGuid() + Path.GetExtension(Path.GetFileName(postedfile.FileName));
            var fullUrl = Path.Combine(Server.MapPath(@"~/upload"));
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
            var fullUrl = Path.Combine(Server.MapPath(@"~"));
            var n = fullUrl + name;
            if (System.IO.File.Exists(n))
            {
                System.IO.File.Delete(n);
            }
            return "";
        }
    }
}