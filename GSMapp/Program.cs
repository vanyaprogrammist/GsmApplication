using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GSMapp
{
    class Program
    {
        static void Main(string[] args)
        {
            GSMsms sms = new GSMsms();

            foreach (GSMcom c in sms.List())
            {
                Console.WriteLine(c.Description + " "+ c.Name);
            }
            Console.ReadLine();
        }
    }
}
