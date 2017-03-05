using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace zhangziao
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.Write("x = ");

            string x;
            x= Console.ReadLine();
            Console.Write("输入x值是：{0}", x);

            Int16 f;
            f = Convert.ToInt16(x);
            Console.Write("转换成整数是：{0}", f);




            Console.ReadKey();
        }
    }
}
