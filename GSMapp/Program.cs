using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace GSMapp
{
    class Program
    {
        static void Main(string[] args)
        {
            
            GsmConnect sms = new GsmConnect();
            GeneralCommands gc = new GeneralCommands();
            

            
            sms.Connect();
            Console.WriteLine(sms.IsConnected);
            
            if (sms.IsConnected)
            {
                sms.Operator();
            }

            Console.ReadLine();

        }
    }
}
