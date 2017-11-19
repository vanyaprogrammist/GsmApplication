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
        private List<ICommand> handlers;
        private SerialDataReceivedEventHandler Receiver;

        public InitializeSimManager(PortConnect portConnect)
        {
            this.PortConnect = portConnect;
            portConnect.AddReceiver(this.Receiver);
        }

        public void AddHandler(ICommand handler)
        {
            if (handlers.Any(h => h==handler))
            {
                throw new Exception("Handler уже добавлен");
            }
            handlers.Add(handler);
        }

        public void RemoveHandler(ICommand handler)
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
                ICommand command = (ICommand) enumerator.Current;
                foreach (string r in command.Request())
                {
                    PortConnect.Write(r);
                    Thread.Sleep(500);
                }

                Receiver = (sender, e) =>
                {
                    SerialPort sp = (SerialPort) sender;
                    string indata = sp.ReadExisting();
                    bool complete = command.Responce(indata);

                    if (complete)
                    {
                        move = enumerator.MoveNext();
                    }
                };
            }
            else
            {
                Console.WriteLine("ProcessNewCard_Конец_Опреации");
            }
        }

        
    }
}
