using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GSMapp.DataBase.Abstract;
using GSMapp.DataBase.Entities;

namespace GSMapp.DataBase.Concrete
{
    public class CardRep : ICardRepository
    {
        DbConnect db = new DbConnect();

        public IEnumerable<Com> Coms => db.Coms;
        public IEnumerable<SimCard> Cards => db.Cards;
        public void AddCard(SimCard card)
        {
            SimCard simCard = db.Cards.Find(card.ComName);
            if (simCard != null)
            {
                simCard.Number = card.Number;
                simCard.Operator = card.Operator;
            }
            else
            {
                db.Cards.Add(card);
            }
            db.SaveChanges();
            Console.WriteLine($"REP: Card.ComName = {card.ComName}\r\n Card.Operator = {card.Operator}\r\n Card.Number = {card.Number}");
        }

        public void AddCom(Com com)
        {
            Com port = db.Coms.Find(com.ComName);
            if (port != null)
            {
                    port.Description = com.Description;
                
            }
            
            else
            {
                db.Coms.Add(com);
            }
            db.SaveChanges();
        }

        public SimCard DeleteCard(string comPort)
        {
            SimCard simCard = db.Cards.Find(comPort);
            if (simCard != null)
            {
                db.Cards.Remove(simCard);
                db.SaveChanges();
            }
            return simCard;
        }

        public Com DeleteCom(string comPort)
        {
            Com port = db.Coms.Find(comPort);
            if (port != null)
            {
                db.Coms.Remove(port);
                db.SaveChanges();
            }
            return port;
        }
    }
}
