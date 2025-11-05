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
using WpfApp.Services;

namespace Grupo_1_Interfaces
{
    /// <summary>
    /// Lógica de interacción para NumeroVentasMes.xaml
    /// Se muestran todas las ventas del mes y permite filtrarlas por estado
    /// </summary>
    public partial class NumeroVentasMes : Window
    {
        private readonly ApiOdooService _apiService;
        private List<Venta> todasLasVentas;

        //Inicializa la ventana y se carga las ventas desde la API
        public NumeroVentasMes(ApiOdooService apiService)
        {
            InitializeComponent();
            _apiService = apiService;
            //carga las ventas de manera asincrona
            _ = CargarVentas();
        }


        //Carga todas las ventas desde la API y configura los controles de la ventana
        private async Task CargarVentas()
        {
            try
            {    
                //obtiene las ventas
                todasLasVentas = await _apiService.GetVentasAsync();

                //obtiene la lista de estados y los ordena
                var estados = todasLasVentas
                    .Select(v => v.EstadoTraducido) //obtiene solo los estados traducidos
                    .Distinct() //quita duplicados
                    .OrderBy(e => e) //lo ordena alfabéticamente
                    .ToList(); //convierte en lista

                //se añade la opción de 'Todos' al principio del 'ComboBox' para visualizar también todas las ventas
                estados.Insert(0, "Todos");
                cmbEstadoDeVentas.ItemsSource = estados;
                cmbEstadoDeVentas.SelectedIndex = 0;


                //pasa todas las ventas que tiene al datagriid
                dataGridVentas.ItemsSource = todasLasVentas;
            }
            catch (System.Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}");
            }
        }

        //ComboBox filtrado por estado
        //Dependiendo de que estado se seleccione se visualizarán algunas ventas u otras
        private void cmbEstadoDeVentas_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (todasLasVentas == null || todasLasVentas.Count == 0)
                return;

            string estadoSeleccionado = cmbEstadoDeVentas.SelectedItem as string;

            if (string.IsNullOrEmpty(estadoSeleccionado) || estadoSeleccionado == "Todos")
            {
                dataGridVentas.ItemsSource = todasLasVentas;
            }
            else
            {
                var filtradas = todasLasVentas
                    .Where(v => v.EstadoTraducido == estadoSeleccionado)  //se obtiene los estados que coinciden entre los estados de la API y del 'EstadoTraducido'
                    .ToList(); //Se convierte en lista

                dataGridVentas.ItemsSource = filtradas; //se muestran las ventas del estado seleccionado
            }
        }


        //Al pulsar el botón se cierra la ventana
        private void Boton_cerrar(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
