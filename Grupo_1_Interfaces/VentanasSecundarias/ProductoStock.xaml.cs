using Grupo_1_Interfaces.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Xml.Linq;
using static System.Net.Mime.MediaTypeNames;

namespace Grupo_1_Interfaces
{
    /// <summary>
    /// Lógica de interacción para ProductosStock.xaml
    /// </summary>
    public partial class ProductosStock : Window
    {
        public ProductosStock(Stock productoSeleccionado)
        {
            InitializeComponent();
            Nombre_stock.Content = productoSeleccionado.nombre_producto;
            StockDisponible_stock.Content = productoSeleccionado.available_quantity;
            StockTotal_stock.Content= productoSeleccionado.Quantity;
            Reservado_stock.Content = productoSeleccionado.reserved_quantity;
            Ubicacion_stock.Content = productoSeleccionado.location_id;
        }

        private void boton_cerrar(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}