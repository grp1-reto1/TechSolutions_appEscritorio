using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Grupo_1_Interfaces.Models
{
    public class Pedido
    {
        public string name { get; set; }
        public string origin { get; set; }
        public DateTime scheduled_date { get; set; }
        public string state { get; set; }
        public string FechaLimite { get; set; }

    }
}
