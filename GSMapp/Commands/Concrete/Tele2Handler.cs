using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GSMapp.Commands.Abstract;
using GSMapp.Entities;

namespace GSMapp.Commands.Concrete
{
    public class Tele2Handler : IHandler
    {
        public string Name { get; } = nameof(Tele2Handler);

        private SimCard card;

        public Tele2Handler(SimCard card)
        {
            this.card = card;
        }

        public bool Skip()
        {
            if (card.Operator.Equals(OperatorList.Tele2))
            {
                return false;
            }

            return true;
        }

        public string[] Request()
        {
            Console.WriteLine(this.Name+" request->");
            string[] request = { "AT+COPS?" };
            return request;
        }

        public bool Responce(string responce)
        {
            throw new NotImplementedException();
        }

        private string Handler(string responce)
        {
            return null;
        }
    }
}
