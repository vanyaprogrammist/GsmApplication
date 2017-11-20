using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GSMapp.Commands.Concrete;
using GSMapp.Connectors;
using GSMapp.Entities;

namespace GSMapp.Commands
{
    public static class Init
    {
        private static PortConnect portConnect = null;
        private static SimCard simCard = null;

        public static PortConnect PortConnect
        {
            get
            {
                if (portConnect != null)
                {
                    return portConnect;
                }
                else
                {
                    throw new Exception("PortConnect не задано");
                }
            }

            set
            {
                if (value != null)
                {
                    portConnect = value;
                }
                else
                {
                    throw new Exception("Нельзя задавать PortConnect = null");
                }
            }
        }

        public static SimCard SimCard
        {
            get
            {
                if (simCard != null)
                {
                    return simCard;
                }
                else
                {
                    throw new Exception("SimCard не задано");
                }
            }

            set
            {
                if (value != null)
                {
                    simCard = value;
                }
                else
                {
                    throw new Exception("Нельзя задавать SimCard = null");
                }
            }
        }
        

        

        private static InitializeSimManager CreateSimManager()
        {
            
                InitializeSimManager manager = new InitializeSimManager(PortConnect);

                manager.AddHandler(new OperatorHandler(SimCard));
                manager.AddHandler(new Tele2Handler(SimCard));

            return manager;
        }

        public static void Excecute()
        {
            InitializeSimManager manager = CreateSimManager();

            
            manager.StartManager();
            
        }
    }
}
