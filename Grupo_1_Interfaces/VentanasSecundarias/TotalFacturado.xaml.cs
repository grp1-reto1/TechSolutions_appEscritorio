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
    /// </summary>
    public partial class TotalFacturado : Window
    {
        private readonly ApiOdooService _apiService;
        private List<Factura> todasLasFacturas;
        public TotalFacturado(ApiOdooService apiService)
        {
            InitializeComponent();
            _apiService = apiService;
            _ = CargarFacturas();
        }
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
                    .OrderBy(e => e)
                    .ToList();

                estados.Insert(0, "Todos");
                cmbEstadoFactura.ItemsSource = estados;
                cmbEstadoFactura.SelectedIndex = 0;

                List<Factura> ventas = await _apiService.GetFacturacionAsync();

                dataGridFacturas.ItemsSource = ventas;
            }
            catch (System.Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}");
            }
        }

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
                    .Where(v => v.EstadoFacturaTraducido == estadoSeleccionado)
                    .ToList();

                dataGridFacturas.ItemsSource = filtradas;
            }

        }

        private void Boton_cerrar(object sender, EventArgs e)
        {
            Close();
        }
    }
}
