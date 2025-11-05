using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO.Packaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Grupo_1_Interfaces.Models
{
    public class Factura
    {
        public string name { get; set; } //nombre de la factura
        public string payment_state {  get; set; } // estado de pago
        public string state { get; set; } //estado de la factura
        public decimal amount_total { get; set; } //total
        public DateTime date {  get; set; } // fecha
        public string nombre_cliente { get; set; } //nombre del cliente
        public int partner_id { get; set; } //id del cliente

        //Traducción del estado de facturas
        //Se basa en 'payment_state' en lugar de 'state' porque nos interesa saber 
        //si la factura está pagada o no, independiente del estado general de la factura
        public string EstadoFacturaTraducido 
        {
            get
            {
                switch (payment_state)
                {
                    case "paid": return "Pagado";
                    case "not_paid": return "No pagado";
                    default: return payment_state ?? "";
                }
            }
        }
    }
}
