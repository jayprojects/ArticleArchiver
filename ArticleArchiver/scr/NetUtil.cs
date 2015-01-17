using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
namespace ArticleArchiver
{
    public class NetUtil
    {
        public static string getContentWC(string sourceUrl)
        {
            if (sourceUrl != null && sourceUrl.Contains("http"))
            {
                CookieAwareWebClient client = new CookieAwareWebClient();

                client.Headers.Add(HttpRequestHeader.UserAgent, "Mozilla/5.0 (Windows NT 6.1; WOW64; rv:33.0) Gecko/20100101 Firefox/33.0");
                client.Headers.Add(HttpRequestHeader.ContentType, "text/html; charset=UTF-8");
                client.Headers.Add(HttpRequestHeader.Accept, "text/html,application/xhtml+xml,application/xml;q=0.9,*/*;q=0.8");
                client.Headers.Add(HttpRequestHeader.AcceptCharset, "UTF-8");
                client.Headers.Add(HttpRequestHeader.Referer, "http://www.google.com/");
                client.Headers.Add(HttpRequestHeader.AcceptLanguage, "en-US,en;q=0.5");
                client.Headers.Add(HttpRequestHeader.KeepAlive, "TRUE");
                string sourceData = client.DownloadString(sourceUrl);
                return sourceData;
            }
            return "";
        }
        public class CookieAwareWebClient : WebClient
        {
            private CookieContainer cookie = new CookieContainer();

            protected override WebRequest GetWebRequest(Uri address)
            {
                WebRequest request = base.GetWebRequest(address);
                if (request is HttpWebRequest)
                {
                    (request as HttpWebRequest).CookieContainer = cookie;
                }
                return request;
            }
        }
    }
}
