using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Grupo_1_Interfaces.Models
{
    public class Cliente
    {

        public string name { get; set; }
        //public int partner_id { get; set; }

        public JArray partner_id { get; set; }

        //devuelve solo el nombre del cliente
        public string PartnerName
        {
            get
            {
                if (partner_id != null && partner_id.Count > 1)
                    return partner_id[1].ToString();
                return "";
            }
        }
    }
}
