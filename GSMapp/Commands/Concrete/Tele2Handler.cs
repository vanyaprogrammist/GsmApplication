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
        public string Name { get; }

        private SimCard card;

        public Tele2Handler(SimCard card)
        {
            this.card = card;
        }

        public string[] Request()
        {
            string[] request = {"AT"};
            Console.WriteLine("Card_operator is: "+card.Operator);
            return request;
        }

        public bool Responce(string responce)
        {
            throw new NotImplementedException();
        }
    }
}
