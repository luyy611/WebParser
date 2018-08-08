using System;
using System.Threading;
using Common.Logging;
#if !NET_20
#endif
using Quartz.Impl;
using Quartz;


namespace WebParser
{
    public class ParserConfig
    {
        //执行Quartz线程
        public void Run()
        {
            try
            {
                log4net.ILog _logger = log4net.LogManager.GetLogger(typeof(ParserConfig));
                _logger.Info("程序初始化------------------");
                //引用一个调度器
                ISchedulerFactory sf = new StdSchedulerFactory();
                //返回客户端可用的句柄
                IScheduler sched = sf.GetScheduler();
                _logger.Info("程序初始化完成----------------");
                _logger.Info("开始调度任务-----------------");
                //定义一个任务，并且和自己定义的ParserJob绑定
                IJobDetail job = JobBuilder.Create<ParserJob>()
                    .WithIdentity("job1", "group1")
                    .Build();
                //定义触发器
                ITrigger trigger = TriggerBuilder.Create()
                    .WithIdentity("trigger1", "group1")
                    .StartNow()
                    .WithSimpleSchedule(x => x.WithIntervalInSeconds(10)
                    .WithIntervalInSeconds(10)
                    .RepeatForever())
                    .Build();
                //告知quartz利用触发器调度任务
                sched.ScheduleJob(job, trigger);
                //开始调度
                sched.Start();
                _logger.Info("启动调度器-------------------");
                //等待足够长时间 以便于系统可以自行调度
                Thread.Sleep(TimeSpan.FromSeconds(60));
                //关闭调度器
                _logger.Info("---------调度器关闭----------");
                sched.Shutdown(true);
                _logger.Info("---------调度完成------------");
            }
            catch (Exception ex)
            {
                Console.WriteLine("任务启动失败" + ex.Message);
            }
        }
    }
}
