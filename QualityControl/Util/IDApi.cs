using System.IO;
using System.Net;
using System.Text;

namespace QualityControl.Util
{
    public class IDApi
    {
        public static string Query(string identity)
        {
            var strURL = "http://apis.baidu.com/apistore/idservice/id?id=" + identity;
            HttpWebRequest request;
            request = (HttpWebRequest) WebRequest.Create(strURL);
            request.Method = "GET";
            // 添加header
            request.Headers.Add("apikey", "62d6242fd3c377866df22836204e4677");
            HttpWebResponse response;
            response = (HttpWebResponse) request.GetResponse();
            Stream s;
            s = response.GetResponseStream();
            var strValue = "";
            var Reader = new StreamReader(s, Encoding.UTF8);
            strValue = Reader.ReadToEnd();
            return strValue;
        }
    }
}