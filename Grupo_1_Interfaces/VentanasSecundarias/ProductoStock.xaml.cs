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
    /// Esta ventana muestra la información de un producto en stock
    /// </summary>
    public partial class ProductosStock : Window
    {
        //Obtiene la información de 'Stock' y la asigna a los controles de la interfaz
        public ProductosStock(Stock productoSeleccionado)
        {
            InitializeComponent();

            //Se asigna la información de 'Stock' a los controles correspondientes
            Nombre_stock.Content = productoSeleccionado.nombre_producto;
            StockDisponible_stock.Content = productoSeleccionado.available_quantity;
            StockTotal_stock.Content= productoSeleccionado.Quantity;
            Reservado_stock.Content = productoSeleccionado.reserved_quantity;
            Ubicacion_stock.Content = productoSeleccionado.location_id;
        }

        //Al pulsar el botón se cierra la ventana
        private void boton_cerrar(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}