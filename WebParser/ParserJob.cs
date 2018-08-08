using Common.Logging;
using Quartz;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebParser
{
    public class ParserJob:IJob
    {
        //定义一个全局静态日志对象
        private static log4net.ILog _logger = log4net.LogManager.GetLogger(typeof(ParserJob));
        //空的构造函数
        public ParserJob()
        {
        }
        //执行作业
        public void Execute(IJobExecutionContext context)
        {
            DateTime startTime = System.DateTime.Now;
            _logger.Info(string.Format("开始解析，当前时间{0}：", startTime.ToString()));
            string waterURL = System.Configuration.ConfigurationManager.AppSettings["waterURL"].ToString();
            string warningURL = System.Configuration.ConfigurationManager.AppSettings["warningURL"].ToString();
            string weatherURL = System.Configuration.ConfigurationManager.AppSettings["weatherURL"].ToString();
            string sailingURL = System.Configuration.ConfigurationManager.AppSettings["sailingURL"].ToString();
            string channelURL = System.Configuration.ConfigurationManager.AppSettings["channelURL"].ToString();
            int id1 = WaterBusiness.ParserPage(waterURL);
            _logger.Info(string.Format(ReturnMessage(id1)));
        }
        //根据返回代码，显示相关信息
        public string ReturnMessage(int rId)
        {
            string msg = "";
            switch (rId)
            {
                case 1:
                    msg = "尚未更新！";
                    break;
                case 2:
                    msg = "解析成功，已成功导入到数据库！";
                    break;
                case 3:
                    msg = "列表信息页面解析出错！";
                    break;
                case 4:
                    msg = "详细信息页面解析出错！";
                    break;
                case 5:
                    msg = "插入到数据库出错！";
                    break;
                default:
                    msg = "未知错误";
                    break;
            }
            return msg;
        }
    }
}
