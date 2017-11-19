using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using GSMapp.Commands;
using GSMapp.Connectors;
using GSMapp.Entities;


namespace GSMapp
{
    class Program
    {
        static void Main(string[] args)
        {
            
            PortConnect sms = new PortConnect();
            SimCard card = new SimCard();
            /*GeneralCommands gc = new GeneralCommands();
            sms.AddReceiver(gc.Receiver);
            gc.ReceiverTest();
            

            
            
            Console.WriteLine(sms.IsConnected);
            
            if (sms.IsConnected)
            {
                sms.Operator();
            }*/
            sms.Connect();
            Init.PortConnect = sms;
            Init.SimCard = card;
            Init.Excecute();

            
            Console.ReadLine();

        }
    }
}
