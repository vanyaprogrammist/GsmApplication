using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using GSMapp.Connectors;

namespace GSMapp
{
    class Program
    {
        static void Main(string[] args)
        {
            
            PortConnect sms = new PortConnect();
            GeneralCommands gc = new GeneralCommands();
            sms.AddReceiver(gc.Receiver);

            
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
