WebCrawler
#这是一个基于Quartz的网页爬虫程序

#爬取目标网页是长江海事局的每日水位和气温等信息：http://www.cjmsa.gov.cn/

#为了完成自动爬取，这里使用了一种任务调度框架-----Quartz

#Quartz可以满足在某一个有规律的时间点干某件事。并且时间的触发的条件可以非常复杂

#这里爬取的信息是每日11点和17点的水位和气温信息

1.启动程序Program.cs

2.配置调度器、job和触发器

            //引用一个调度器
            ISchedulerFactory sf = new StdSchedulerFactory();
            //返回客户端可用的句柄
            IScheduler sched = sf.GetScheduler();
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
3.添加配置文件和必要引用

          log4net.config(配置日志输出)
          quartz_jobs.xml(触发器和job的映射)
          App.config（配置数据库连接，日志，quart等）
          Quartz.dll
          Common.Logging.Core.dll
          Common.Logging.dll
          Common.Logging.Log4Net1215.dll
          log4net.dll
4.调用Quartz配置类：

            ParserConfig example = new ParserConfig();
            Console.WriteLine("抓取程序正在运行，请不要关闭窗口");
            example.Run();
            Console.WriteLine("抓取成功！");
5.执行Job

Excute()方法自动执行

爬取网页源代码：

            //获取或设置用于向Internet资源的请求进行身份验证的网络凭据
            WebClient MyWebClient = new WebClient();
            MyWebClient.Credentials = CredentialCache.DefaultCredentials;
            //从指定网站下载数据 
            Byte[] pageData = MyWebClient.DownloadData(url);
把源代码转成xml格式：

        //获取网页源代码
        string pageContent = WebCapture.CapturePage(url);
        //利用内置解析类 开始解析成xml格式
        Lexer lex = new Lexer(pageContent);
        Parser parser = new Parser(lex);
        NodeList nodeList = parser.Parse(null);
        //添加过滤条件
        nodeList = nodeList.ExtractAllNodesThatMatch(filter, true);
之后就可以专心完成对xml的解析了
