using System;
using System.Collections;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using GSMapp.Commands.Abstract;
using GSMapp.Connectors;

namespace GSMapp.Commands
{
    public class InitializeSimManager
    {
        private PortConnect PortConnect { get; set; }
        private List<IHandler> handlers;
        public SerialDataReceivedEventHandler Receiver;

        public InitializeSimManager(PortConnect portConnect)
        {
            this.PortConnect = portConnect;
            
            handlers = new List<IHandler>();
        }

        public void AddHandler(IHandler handler)
        {
            if (handlers.Any(h => h==handler))
            {
                throw new Exception("Handler уже добавлен");
            }
            handlers.Add(handler);
        }

        public void RemoveHandler(IHandler handler)
        {
            if (handlers.Any(h => h == handler))
            {
                handlers.Remove(handler);
            }
            else
            {
                throw new Exception("Handler не добавлен или уже удалён");
            }
        }

        public void ProcessNewCard()
        {
            IEnumerator enumerator = handlers.GetEnumerator();
            bool move = enumerator.MoveNext();

            if (move)
            {
                IHandler handler = (IHandler) enumerator.Current;
                foreach (string r in handler.Request())
                {
                    PortConnect.Write(r);
                }

                Receiver = (sender, e) =>
                {
                    SerialPort sp = (SerialPort) sender;
                    string indata = sp.ReadExisting();
                    bool complete = handler.Responce(indata);

                    if (complete)
                    {
                        move = enumerator.MoveNext();
                    }
                };

                PortConnect.AddReceiver(Receiver);
            }
            else
            {
                Console.WriteLine("ProcessNewCard_Конец_Опреации");
            }
        }

        public void AddReceiver()
        {
            PortConnect.AddReceiver(Receiver);
        }
    }
}
