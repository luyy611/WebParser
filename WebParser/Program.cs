using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebParser
{
    public class Program
    {
        static void Main(string[] args)
        {
            try
            {
                ParserConfig example = new ParserConfig();
                Console.WriteLine("抓取程序正在运行，请不要关闭窗口");
                example.Run();
                Console.WriteLine("抓取成功！");
            }
            catch (Exception ex)
            {
                Console.WriteLine("抓取程序出错：" + ex.Message);
            }
            Console.ReadLine();

        }
    }
}
