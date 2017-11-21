using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GSMapp.Commands.Abstract;
using GSMapp.Entities;

namespace GSMapp.Commands.Concrete
{
    public class OperatorHandler : IHandler
    {
        public string Name { get; } = nameof(OperatorHandler);

        private SimCard card;

        public OperatorHandler(SimCard card)
        {
            this.card = card;
        }

        public bool Skip()
        {
            return false;
        }

        public string[] Request()
        {
            Console.WriteLine(this.Name+" request->");
            string[] request = { "AT+COPS?"};

        return request;
        }

        public bool Responce(string responce)
        {
            Console.WriteLine(this.Name+" responce<-");
           
                string handlString = Handler(responce);
                if (handlString != null)
                {
                    Console.WriteLine("Operator обработан");
                    bool success = SaveModel(handlString);

                    if (success)
                        return true;
                    else
                        Console.WriteLine("Не получилось сохранить operator");
                }
                return false;
            
        }

        

        private string Handler(string responce)
        {
            string a = responce;
            int startIndex = a.IndexOf("\"", StringComparison.Ordinal) + 4;
            int lastIndex = a.LastIndexOf("\"", StringComparison.Ordinal) - startIndex;
            if (responce.Contains("+COPS:"))
            {
                a = a.Substring(startIndex, lastIndex);
                return a;
            }
            return null;
        }

        private bool SaveModel(string change)
        {
            int number = Int32.Parse(change);

            foreach (OperatorList o in Enum.GetValues(typeof(OperatorList)))
            {
                if (number == (int)o)
                {
                    card.Operator =  o;
                    return true;
                }
            }
        return false;
        }
    }

    
}
