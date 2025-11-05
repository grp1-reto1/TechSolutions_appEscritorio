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
    /// Lógica de interacción para PedidiosPendienteEnvio.xaml
    /// </summary>
    public partial class PedidiosPendienteEnvio : Window
    {
        public PedidiosPendienteEnvio(Pedido pedidoSeleccionado)
        {
            InitializeComponent();

            Nombre_pedido.Content = pedidoSeleccionado.name;
            Fecha_pedido.Content = pedidoSeleccionado.scheduled_date.ToString("dd/MM/yyyy");
            Origen_pedido.Content = pedidoSeleccionado.origin;
            Estado_pedido.Content = pedidoSeleccionado.EstadoPedido;
        }

        private void boton_cerrar(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}