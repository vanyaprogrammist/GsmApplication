using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using GSMapp.Commands;
using GSMapp.Connectors;
using GSMapp.DataBase.Concrete;
using GSMapp.Hellpers;
using GSMapp.Models;
using Com = GSMapp.DataBase.Entities.Com;
using SimCard = GSMapp.DataBase.Entities.SimCard;


namespace GSMapp
{
    class Program
    {
        static void Main(string[] args)
        {
            
            PortConnect sms = new PortConnect();
            GSMapp.Models.SimCard card = new GSMapp.Models.SimCard();
            

            CardRep rep = new CardRep();
           


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
            Init.Repository = rep;
            Init.Excecute();
            

            Console.ReadLine();

        }
    }
}
