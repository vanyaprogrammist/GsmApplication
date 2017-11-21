﻿using System;
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
        private IEnumerator enumerator;

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

        public void StartManager()
        {
            enumerator = handlers.GetEnumerator();
            Command();

            PortConnect.AddReceiver(Receiver);
        }

        private void Command()
        {
            
            bool move = enumerator.MoveNext();
            if (this.handlers.Count == 0)
            {
                move = false;
            }
            if (move)
            {
                IHandler handler = (IHandler) enumerator.Current;
                if (handler.Skip())
                {
                    Command();
                }
                foreach (string r in handler.Request())
                {
                    PortConnect.Write(r);
                }
                Feedback(handler);
            }
            else
            {
                Console.WriteLine("ProcessNewCard_Конец_Опреации");
            }
        }

     private int counter = 0;

        private void Feedback(IHandler handler)
        {
            Console.WriteLine("FEEDBACK");
            if (counter != 0)
            {
                Console.WriteLine("Remove Receiver");
                PortConnect.RemoveReceiver(Receiver);
            }
            Receiver = (sender, args) =>
            {
                Console.WriteLine("Receiver --- Handler name: "+handler.Name);
                SerialPort sp = (SerialPort)sender;
                string indata = sp.ReadExisting();
                bool complete = handler.Responce(indata);

                if (complete)
                {
                   Command();
                }
            };
            
                
                counter++;
            
           
        }

        
    }
}
