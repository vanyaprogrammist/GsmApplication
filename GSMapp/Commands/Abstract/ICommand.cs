using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GSMapp.Entities;

namespace GSMapp.Commands.Abstract
{
    public interface ICommand
    {
        string Name { get; }

        string[] Request();
        bool Responce(string responce);
    }
}
