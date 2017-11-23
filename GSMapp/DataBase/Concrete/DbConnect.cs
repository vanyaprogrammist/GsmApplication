using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GSMapp.DataBase.Entities;

namespace GSMapp.DataBase.Concrete
{
    [DbConfigurationType(typeof(MySql.Data.Entity.MySqlEFConfiguration))]
    public class DbConnect : DbContext
    {
        public DbConnect() : base("conn")
        {
            
        }

        public DbSet<Com> Coms { get; set; }
        public DbSet<SimCard> Cards { get; set; }
    }
}
