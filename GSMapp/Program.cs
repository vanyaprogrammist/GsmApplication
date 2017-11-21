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
            GeneralCommands gc = new GeneralCommands(sms);




             /*gc.ReceiverTest(" test ");
             gc.port.AddReceiver(gc.Receiver);



             sms.Connect();
             Console.WriteLine(sms.IsConnected);

             if (sms.IsConnected)
             {
                 sms.Number();
                //gc.port.RemoveReceiver(gc.Receiver);
                gc.DeleteReceiver();
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
