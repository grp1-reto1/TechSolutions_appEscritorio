using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Grupo_1_Interfaces.Models
{
    public class Venta
    {
        public string name { get; set; } //nombre de la venta
        public string nombre_cliente { get; set; } //nombre del cliente
        public decimal amount_total { get; set; } //total de la venta
        public string state { get; set; } //estado
        public DateTime date_order { get; set; } //fecha
        public string TotalFormateado => amount_total.ToString("C2", new CultureInfo("es-ES"));


        //Traducir el estado de venta
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
    }

}
