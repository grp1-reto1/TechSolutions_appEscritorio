using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Grupo_1_Interfaces.Models
{
    public class Pedido
    {
        public string name { get; set; } //nombre del pedido
        public string origin { get; set; } //el origen del pedido
        public DateTime scheduled_date { get; set; } //fecha de envío 
        public string state { get; set; } //estado
        public string nombre_cliente { get; set; } //nombre del cliente

        //Traduce el estado de pedido
        public string EstadoPedido
        {
            get
            {
                switch (state)
                {
                    case "confirmed": return "En espera";
                    case "assigned": return "Listo";
                    default: return state ?? "";
                }
            }
        }
    }
}
