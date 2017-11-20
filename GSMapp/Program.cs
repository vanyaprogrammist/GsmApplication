using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using GSMapp.Commands;
using GSMapp.Connectors;
using GSMapp.Entities;
using GSMapp.Hellpers;


namespace GSMapp
{
    class Program
    {
        static void Main(string[] args)
        {
            
            PortConnect sms = new PortConnect();
            SimCard card = new SimCard();
            GeneralCommands gc = new GeneralCommands();

            string result = 
                "04230441043B0443043304300020043D04350434043E044104420443043F043D04300020043D043000200412043004480435043C002004420430044004380444043D043E043C0020043F043B0430043D0435002E".UnicodeStrToUcs2Str();
            Console.WriteLine(result);
            /*gc.ReceiverTest();
            sms.AddReceiver(gc.Receiver);
            


            sms.Connect();

            Console.WriteLine(sms.IsConnected);
            
            if (sms.IsConnected)
            {
                sms.Number();
            }
            /*
            Init.PortConnect = sms;
            Init.SimCard = card;
            Init.Excecute();

            */
            Console.ReadLine();

        }
    }
}
