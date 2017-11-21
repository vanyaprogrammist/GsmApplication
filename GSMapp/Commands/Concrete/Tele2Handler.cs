using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GSMapp.Commands.Abstract;
using GSMapp.Entities;
using GSMapp.Hellpers;

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
            Console.WriteLine(this.Name+" responce<-");

            string handlString = Handler(responce);
            if (handlString != null)
            {
                Console.WriteLine("Tele2_Number обработан");
                bool success = SaveModel(handlString);
                if (success)
                {
                    Console.WriteLine("Вот сохраненный номер Tele2: "+handlString);
                    return true;
                }
                else
                    Console.WriteLine("Не получилось сохранить Tele2_Number");
            }

            return false;
        }

        private string Handler(string responce)
        {
            string result = CusdMessageHandler(responce);
            if (result != null)
            {
                result = NumberHandler(result);
                if (result != null)
                {
                    return result;
                }
                else
                {
                    Console.WriteLine("Не получилось получить номер Tele2");
                }
            }
            return null;
        }

        private bool SaveModel(string change)
        {
            if (change != null)
            {
                card.Number = change;
                return true;
            }
            return false;
        }


        private string CusdMessageHandler(string responce)
        {
            int startIndex = responce.IndexOf("\"", StringComparison.Ordinal) + 1;
            int lastIndex = responce.LastIndexOf("\"", StringComparison.Ordinal) - startIndex;
            if (responce.Contains("+CUSD:"))
            {
                responce = responce.Substring(startIndex, lastIndex).Ucs2StrToUnicodeStr();
                return responce;
            }
            return null;
        }

        private string NumberHandler(string message)
        {
            int startIndex = message.IndexOf("+", StringComparison.Ordinal);
            int lastIndex = message.Length - startIndex;
            if (message.Contains("Ваш федеральный номер"))
            {
                message = message.Substring(startIndex, lastIndex);
                return message;
            }
            return null;
        }
    }
}
