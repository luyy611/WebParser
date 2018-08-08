using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebParser.DataAccess
{
    /*
     * 水位数据管理类
     */
    public class WaterHelper
    {
        /// <summary>
        /// 查询url是否已插入
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public static bool QueryUrlExist(string url){
            string tableName = "WaterLevelInfo";
            return CommonHelper.QueryByUrl(url, tableName);
        }

        /// <summary>
        /// 将抓取到的数据存入数据库水位表中
        /// </summary>
        /// <param name="title"></param>
        /// <param name="articlefrom"></param>
        /// <param name="updatetime"></param>
        /// <param name="url"></param>
        /// <param name="content"></param>
        /// <returns></returns>
        public static bool SaveToWaterLevelInfo(string title, string articlefrom, string updatetime, string url, string content)
        {
            try
            {
                string tableName = "WaterLevelInfo";
                return CommonHelper.SaveToOneTable(title, articlefrom, updatetime, url, content, tableName);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }

        }
    }
}
