using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GSMapp.Commands.Abstract;
using GSMapp.DataBase.Abstract;
using GSMapp.DataBase.Concrete;
using GSMapp.Models;


namespace GSMapp.Commands.Concrete
{
    public class SaveCardHandler : IHandler
    {
        private SimCard card;
        private Com com;
        private ICardRepository db;

        public string Name { get; } = nameof(SaveCardHandler);

        public SaveCardHandler(SimCard card, Com com, ICardRepository db)
        {
            this.card = card;
            this.com = com;
            this.db = db;
        }

        public bool Skip()
        {
            if (com.Name == null || com.Description == null || card.Number == null || card.Operator == (OperatorList)0)
            {
                Console.WriteLine("Что-то не заполненно, см ниже");
                Console.WriteLine($"com.Name: {com.Name}\r\ncom.Description: {com.Description}\r\ncard.Number: {card.Number}\r\ncard.Operator: {card.Operator}");
                return true;
            }
            return false;
        }

        public string[] Request()
        {
            return null;
        }

        public bool Responce(string responce)
        {
            return false;
        }

        public bool Action()
        {
            if (com != null)
            {
               DataBase.Entities.Com portCom =
                    new DataBase.Entities.Com { ComName = com.Name, Description = com.Description };
                db.AddCom(portCom);
                if (card != null)
                {
                    DataBase.Entities.SimCard simCard = new DataBase.Entities.SimCard { ComName = com.Name,Operator = card.Operator.ToString(),Number = card.Number};
                    db.AddCard(simCard);
                    Console.WriteLine("Данные сохранены в БД");
                    Console.WriteLine($"com.Name: {com.Name}\r\ncom.Description: {com.Description}\r\ncard.Number: {card.Number}\r\ncard.Operator: {card.Operator}");
                }
            }

            return true;
        }
    }
}
