using Grupo_1_Interfaces.Models;
using Newtonsoft.Json;
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
using System.Windows.Navigation;
using System.Windows.Shapes;
using WpfApp.Services;

namespace Grupo_1_Interfaces
{
    /// <summary>
    /// Lógica de interacción para MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly ApiOdooService _apiService;
        public MainWindow()
        {
            InitializeComponent();
            _apiService = new ApiOdooService();
            _ = InicializarAsync();
            _ = _apiService.LoginAsync("xiaoenbergaretxe@gmail.com", "YRU475I2", "TechSolutions2.0");

        }



        private async Task InicializarAsync()
        {
            try
            {
                bool logged = await _apiService.LoginAsync("xiaoenbergaretxe@gmail.com", "YRU475I2", "TechSolutions2.0");
                if (!logged)
                {
                    MessageBox.Show("Error al autenticarse");
                    return;
                }

                await CargarPedidos();
                await CargarFacturas();
                await CargarClientes();
                await CargarStocks();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}");
            }
        }

        private async Task CargarPedidos()
        {
            var pedidos = await _apiService.GetPedidoPendienteAsync();
            var pedidosPendientes = pedidos.Where(p => p.state == "assigned" || p.state == "confirmed").ToList();

            dataGridPedidos.ItemsSource = pedidosPendientes;
        }

        private async Task CargarFacturas()
        {
            var facturas = await _apiService.GetFacturacionAsync();
            var facturasPagadas = facturas.Where(f => f.payment_state.ToLower() == "paid");

            decimal totalFacturado = facturasPagadas.Sum(f => f.amount_total);

            lblTotalFacturado.Content = $"{totalFacturado:N2} €";
        }


        private async Task CargarClientes()
        {
            var clientes = await _apiService.GetClientesDestacadosAsync();
            dataGridClientes.ItemsSource = clientes;
        }
        private async Task CargarStocks()
        {
            var stocks = await _apiService.GetStockAsync();
            dataGridStock.ItemsSource = stocks;
        }

        private void Boton_ventas(object sender, RoutedEventArgs e)
        {
            NumeroVentasMes ventas = new NumeroVentasMes(_apiService);
            ventas.ShowDialog();

        }

        private void Boton_facturas(object sender, RoutedEventArgs e)
        {
            TotalFacturado facturas = new TotalFacturado(_apiService);
            facturas.ShowDialog();
        }

        private void DataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void DataGrid_SelectionChangedStock(object sender, SelectionChangedEventArgs e)
        {
            if (dataGridStock.SelectedItem is Stock stockSeleccionado)
            {
                ProductosStock detalleWindow = new ProductosStock(stockSeleccionado);
                detalleWindow.ShowDialog();

                dataGridStock.SelectedItem = null;
            }
        }
    }
}
