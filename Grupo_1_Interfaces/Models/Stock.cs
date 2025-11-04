using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Grupo_1_Interfaces.Models
{
    public class Stock
    {
        public int id { get; set; } //id del producto
        public int id_producto { get; set;}
        public string nombre_producto { get; set; }
        //public string product_id { get; set; } //id del producto más el nombre
        public string location_id { get; set; } //id de la ubicación y el nombre del almacén
        public float available_quantity { get; set; } //Stock
        public float reserved_quantity { get; set; } //Unidades reservados
        public float Quantity { get; set; } //Cantidad total registrado más los reservados



    }
}
