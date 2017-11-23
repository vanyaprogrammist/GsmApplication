using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using GSMapp.DataBase.Entities;

namespace GSMapp.DataBase.Abstract
{
    public interface ICardRepository
    {
        IEnumerable<Com> Coms { get; }
        IEnumerable<SimCard> Cards { get; }

        void AddCard(SimCard card);
        void AddCom(Com com);

        SimCard DeleteCard(string comPort);
        Com DeleteCom(string comPort);
    }
}
