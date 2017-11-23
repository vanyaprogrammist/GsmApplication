using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GSMapp.Models;

namespace GSMapp.DataBase.Entities
{
    public class Com
    { 
        [Key]
        public string ComName { get; set; }
        public string Description { get; set; }

        public virtual SimCard SimCard { get; set; }
    }

    public class SimCard
    {
        [Key]
        [ForeignKey("Com")]
        public string ComName { get; set; }

        public string Operator { get; set; }
        public string Number { get; set; }

        public virtual Com Com { get; set; }
    }
}
