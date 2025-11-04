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
    /// </summary>
    public partial class NumeroVentasMes : Window
    {
        private readonly ApiOdooService _apiService;
        private List<Venta> todasLasVentas;
        public NumeroVentasMes(ApiOdooService apiService)
        {
            InitializeComponent();
            _apiService = apiService;
            _ = CargarVentas();
        }


        //public NumeroVentasMes(Venta venta)
        //{
        //    InitializeComponent();
        //    DataContext = venta;
        //}

        private async Task CargarVentas()
        {
            try
            {    
                todasLasVentas = await _apiService.GetVentasAsync();


                var estados = todasLasVentas
                    .Select(v => v.EstadoTraducido)
                    .Distinct()
                    .OrderBy(e => e)
                    .ToList();

                estados.Insert(0, "Todos");
                cmbEstadoDeVentas.ItemsSource = estados;
                cmbEstadoDeVentas.SelectedIndex = 0;

                List<Venta> ventas = await _apiService.GetVentasAsync();

                dataGridVentas.ItemsSource = ventas;
            }
            catch (System.Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}");
            }
        }

        //ComboBox filtrado por estado
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
                    .Where(v => v.EstadoTraducido == estadoSeleccionado)
                    .ToList();

                dataGridVentas.ItemsSource = filtradas;
            }
        }

        private void Boton_cerrar(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
