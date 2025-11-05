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
        public string name { get; set; }

        public string payment_state {  get; set; }
        public string state { get; set; }
        public decimal amount_total { get; set; }
        public DateTime date {  get; set; }
        public string nombre_cliente { get; set; }
        public int partner_id { get; set; }
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
