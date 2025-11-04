using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Diagnostics;
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
        public string EstadoDePagoTraducido
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

        //public string name { get; set; }
        ////public int partner_id { get; set; }

        //public JArray partner_id { get; set; }

        //// Propiedad calculada: devuelve solo el nombre del cliente
        //public string PartnerName
        //{
        //    get
        //    {
        //        if (partner_id != null && partner_id.Count > 1)
        //            return partner_id[1].ToString();
        //        return "";
        //    }
        //}
    }
}
