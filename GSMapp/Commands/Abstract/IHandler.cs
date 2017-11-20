using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GSMapp.Entities;

namespace GSMapp.Commands.Abstract
{
    public interface IHandler
    {
        string Name { get; }

        bool Skip();
        string[] Request();
        bool Responce(string responce);
    }
}
