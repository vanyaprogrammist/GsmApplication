using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace GSMapp
{
    public class GsmCom
    {
        public string Name { get; set; }
        public string Description { get; set; }

        public override string ToString() => $"{Description} {Name}";
    }
}
