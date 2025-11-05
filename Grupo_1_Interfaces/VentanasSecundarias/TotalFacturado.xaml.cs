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
    /// Lógica de interacción para TotalFacturado.xaml
    /// Se muestran todas las facturas del mes y permite filtrarlas por estado
    /// </summary>
    public partial class TotalFacturado : Window
    {
        private readonly ApiOdooService _apiService;
        private List<Factura> todasLasFacturas;


        //Inicializa la ventana y se carga las facturas desde la API
        public TotalFacturado(ApiOdooService apiService)
        {
            InitializeComponent();
            _apiService = apiService;

            //Carga las facturas de manera asincrona
            _ = CargarFacturas();
        }


        //Carga todas las facturas desde la API y configura los controles de la ventana
        private async Task CargarFacturas()
        {
            try
            {
                // Obtener todas las ventas del mes
                todasLasFacturas = await _apiService.GetFacturacionAsync();

                // Obtener los estados únicos traducidos
                var estados = todasLasFacturas
                    .Select(v => v.EstadoFacturaTraducido)
                    .Distinct()
                    .OrderBy(e => e) //ordena alfabéticamente
                    .ToList(); //convierte en lista


                //Se agrega la sección de 'Todos' en ComboBox para visualizar todas las facturas
                estados.Insert(0, "Todos");
                cmbEstadoFactura.ItemsSource = estados;
                cmbEstadoFactura.SelectedIndex = 0;

                //Se visualiza las facturas en datagrid
                dataGridFacturas.ItemsSource = todasLasFacturas;
            }
            catch (System.Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}");
            }
        }


        //ComboBox filtrado por estado
        //Dependiendo de que estado se seleccione se visualizarán algunas facturas u otras
        private void cmbEstadoFactura_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

            if (todasLasFacturas == null || todasLasFacturas.Count == 0)
                return;

            string estadoSeleccionado = cmbEstadoFactura.SelectedItem as string;

            if (string.IsNullOrEmpty(estadoSeleccionado) || estadoSeleccionado == "Todos")
            {
                dataGridFacturas.ItemsSource = todasLasFacturas;
            }
            else
            {
                var filtradas = todasLasFacturas
                    .Where(v => v.EstadoFacturaTraducido == estadoSeleccionado) //se obtiene el estado seleccionado comparando con el estado traducido
                    .ToList(); //convierte en lista

                //se visualiza las facturas filtradas por el estado en datagrid
                dataGridFacturas.ItemsSource = filtradas;
            }

        }


        //Al pulsar el botón se cierra la ventana
        private void Boton_cerrar(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
