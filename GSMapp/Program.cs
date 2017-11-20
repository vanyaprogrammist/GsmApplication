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

            
            

            gc.ReceiverTest();
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
