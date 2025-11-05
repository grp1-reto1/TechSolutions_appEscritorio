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
        public string name { get; set; } //nombre del cliente
        public string email { get; set; } //email del cliente
        public string city { get; set; } //ciudad del cliente
        public string phone { get; set; } //teléfono del cliente
    }
}
