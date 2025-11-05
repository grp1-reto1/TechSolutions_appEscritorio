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
    /// Lógica de interacción para Clientes.xaml
    /// Esta ventana muestra la información de un cliente específico
    /// </summary>
    public partial class Clientes : Window
    {
        //Obtiene la información del cliente y la asigna a los controles de la interfaz
        public Clientes(Cliente clienteSeleccionado)
        {
            InitializeComponent();

            //Se asigna la información de 'Cliente' a los controles correspondientes
            Nombre_cliente.Content = clienteSeleccionado.name;
            Email_cliente.Content = clienteSeleccionado.email;
            tlf_cliente.Content = clienteSeleccionado.phone;
            ciudad_cliente.Content = clienteSeleccionado.city;

        }

        //Al pulsar el botón se cierra la ventana
        private void boton_cerrar(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}