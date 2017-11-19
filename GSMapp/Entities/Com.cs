using System;
using System.Collections;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Management;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace GSMapp.Entities
{
    public class Com
    {
        public string Name { get; set; }
        public string Description { get; set; }

        public override string ToString() => $"{Description} {Name}";
    }
}
