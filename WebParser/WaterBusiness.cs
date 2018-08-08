using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebParser.DataAccess;

namespace WebParser
{
    /*
     * 水位信息管理
     */
    public class WaterBusiness
    {
        /// <summary>
        /// 解析详细页面并插入数据库
        /// </summary>
        /// <param name="pageUrl"></param>
        /// <returns></returns>
        public static int ParserPage(string pageUrl)
        {
            //获取目标url
            string targetUrl = CommonParserBusiness.GetUrl(pageUrl);
            //返回结果
            int result = 0;
            //如果目标地址解析成功
            if (targetUrl != "failed")
            {
                bool exist = WaterHelper.QueryUrlExist(targetUrl);
                if(!exist){
                    //获取解析的页面数据
                    string[] waterlevel = new string[6];
                    waterlevel = CommonParserBusiness.GetDataFromTable(targetUrl);
                    if (waterlevel[5] != "failed")
                    {
                        bool sql = WaterHelper.SaveToWaterLevelInfo(waterlevel[0], waterlevel[1], waterlevel[2], waterlevel[3], waterlevel[4]);
                        if (sql)
                        {
                            result = 2;//解析成功，已成功导入到数据库！
                        }
                        else
                        {
                            result = 5;//插入到数据库出错！
                        }
                    }
                    else
                    {
                        result = 4;//详细信息页面解析出错！
                    }
                }
                else
                {
                    result = 1;//"尚未更新！"
                }
            }
            else
            {
                result = 3;//列表信息页面解析出错！
            }
            return result;
        }
    }
}
