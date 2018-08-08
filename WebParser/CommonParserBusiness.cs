using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Winista.Text.HtmlParser;
using Winista.Text.HtmlParser.Filters;
using Winista.Text.HtmlParser.Lex;
using Winista.Text.HtmlParser.Tags;
using Winista.Text.HtmlParser.Util;

namespace WebParser
{
    /*
     * 所有网页解析公共类
     */
    public class CommonParserBusiness
    {
        /// <summary>
        /// 对第一层页面进行解析，返回第二层页面url
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public static string GetUrl(string url)
        {
            try
            {
                StringBuilder linkUrl = new StringBuilder();
                //过滤条件：获得所有li标签
                NodeFilter filterFirst = new AndFilter(new TagNameFilter("li"), new HasAttributeFilter("class", "hui3"));
                //获得所有li标签
                NodeList linkNodeList = GetNodeList(url,filterFirst);
                //获取第一条数据(最新的一条)
                ITag tempTag1 = linkNodeList[0] as ITag;
                ITag tempTag2 = tempTag1.LastChild as ITag;
                //拼接url
                linkUrl.Append("http://www.cjmsa.gov.cn");
                linkUrl.Append(tempTag2.GetAttribute("href"));
                return linkUrl.ToString();
            }
            catch (Exception ex)
            {
                Console.WriteLine("解析第一层出错" + ex.Message);
                return "failed";
            }       

        }
        /// <summary>
        /// 第一种页面：数据格式为table
        /// </summary>
        /// <param name="link"></param>
        /// <returns></returns>
        public static string[] GetDataFromTable(string link)
        {
            //定义一个数组存放需要的解析数据
            string[] tempData = new string[6];
            try
            {
                //过滤条件：添加第二次筛选条件
                NodeFilter secondFilter = new AndFilter(new TagNameFilter("div"), new HasAttributeFilter("class", "nr1"));
                NodeList secondNodeList = GetNodeList(link, secondFilter);
                //获取目标div标签的子节点
                secondNodeList = secondNodeList.ElementAt(0).Children;
                //开始解析需要的数据项
                TableTag targetTable = (TableTag)secondNodeList[1];
                //获取表中的所有行
                TableRow[] tr = targetTable.Rows;
                //标题
                tempData[0] = tr[0].ToPlainTextString().Trim();
                //文章来源及更新时间
                string temp = tr[1].ToPlainTextString().Trim();
                int index1 = temp.IndexOf('：');
                int index2 = temp.LastIndexOf('：');
                tempData[1] = temp.Substring(index1 + 1, 3);
                temp = temp.Substring(index2 + 1).Trim();
                int year, month, date;
                year = temp.LastIndexOf('年');
                month = temp.LastIndexOf('月');
                date = temp.LastIndexOf('日');
                tempData[2] = temp.Substring(0, year) + "-" + temp.Substring(year + 1, month-year-1)+"-"
                     +temp.Substring(month + 1, date-month-1);
                //网址
                tempData[3] = link;
                //水位表
                TableColumn[] tc = tr[3].Columns;
                tempData[4] = tc[0].ChildAt(4).ToHtml();
            }
            catch (Exception ex)
            {
                Console.WriteLine("第二层解析出错啦" + ex.Message);
                tempData[5] = "failed";
            }
            return tempData;
        }
        /// <summary>
        /// 第二种页面：数据格式为文本
        /// </summary>
        /// <returns></returns>
        public static string[] GetDataFromText(string link)
        {
            //定义一个数组存放需要的解析数据
            string[] tempData = new string[6];
            try
            {
                //过滤条件：添加第二次筛选条件
                NodeFilter secondFilter = new AndFilter(new TagNameFilter("div"), new HasAttributeFilter("class", "nrl"));
                NodeList secondNodeList = GetNodeList(link, secondFilter);
                //获取目标div标签的子节点
                secondNodeList = secondNodeList.ElementAt(0).Children;
                //开始解析需要的数据项
                TableTag targetTable = (TableTag)secondNodeList[0];
                //获取表中的所有行
                TableRow[] tr = targetTable.Rows;
                //标题
                tempData[0] = tr[0].ToPlainTextString().Trim();
                //文章来源及更新时间
                string temp = tr[1].ToPlainTextString().Trim();
                int index1 = temp.IndexOf('：');
                int index2 = temp.LastIndexOf('：');
                tempData[1] = temp.Substring(index1 + 1, 3);
                temp = temp.Substring(index2 + 1).Trim();
                int year, month, date;
                year = temp.LastIndexOf('年');
                month = temp.LastIndexOf('月');
                date = temp.LastIndexOf('日');
                tempData[2] = temp.Substring(0, year) + "-" + temp.Substring(year + 1, month - year - 1)
                     + temp.Substring(month + 1, date - month - 1);
                //网址
                tempData[3] = link;
                //水位表
                TableColumn[] tc = tr[3].Columns;
                tempData[4] = tc[0].ChildAt(2).ToHtml();
            }
            catch (Exception ex)
            {
                Console.WriteLine("第二层解析出错啦" + ex.Message);
                tempData[5] = "failed";
            }
            return tempData;
        }
        /// <summary>
        /// 解析html页代码 根据过滤条件获取xml节点
        /// </summary>
        /// <param name="url"></param>
        /// <param name="filter"></param>
        /// <returns></returns>
        public static NodeList GetNodeList(string url, NodeFilter filter)
        {
            //获取网页源代码
            string pageContent = WebCapture.CapturePage(url);
            //利用内置解析类 开始解析成xml格式
            Lexer lex = new Lexer(pageContent);
            Parser parser = new Parser(lex);
            NodeList nodeList = parser.Parse(null);
            //添加过滤条件
            nodeList = nodeList.ExtractAllNodesThatMatch(filter, true);
            return nodeList;
        }
    }
}
