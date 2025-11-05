using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Grupo_1_Interfaces.Models
{
    public class Venta
    {
        public string name { get; set; }
        public string nombre_cliente { get; set; }
        public decimal amount_total { get; set; }
        //public int name { get; set;}
        public string state { get; set; }

        public DateTime date_order { get; set; }

        //Traducir el estado
        public string EstadoTraducido
        {
            get
            {
                switch (state)
                {
                    case "draft": return "Borrador";
                    case "sent": return "Enviado";
                    case "sale": return "Pedido confirmado";
                    case "done": return "Completado";
                    case "cancel": return "Cancelado";
                    default: return state ?? "";
                }
            }
        }

        public string TotalFormateado => $"{amount_total:N2} €";
    }

}
