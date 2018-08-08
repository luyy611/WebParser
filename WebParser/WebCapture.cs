using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace WebParser
{
    /*
     * 获取网页内容类
     */
    public class WebCapture
    {
        /// <summary>
        /// 根据url下载网页源代码
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public static string CapturePage(string url)
        {
            StringBuilder pageContent = new StringBuilder();
            try
            {
                //获取或设置用于向Internet资源的请求进行身份验证的网络凭据
                WebClient MyWebClient = new WebClient();
                MyWebClient.Credentials = CredentialCache.DefaultCredentials;
                //从指定网站下载数据
                Byte[] pageData = MyWebClient.DownloadData(url);
                pageContent.Append(Encoding.UTF8.GetString(pageData));
            }
            catch (WebException webEx)
            {
                Console.WriteLine(webEx.Message.ToString());
            }
            return pageContent.ToString();
        }
    }
}
