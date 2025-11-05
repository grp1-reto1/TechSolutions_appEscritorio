using Grupo_1_Interfaces.Models;
using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Globalization;
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
        private ObservableCollection<Stock> stocksObservable;
        private ObservableCollection<Cliente> clientesObservable;
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

                await cargarVentas();
                await CargarPedidos();
                await CargarFacturas();
                await CargarStocks();
                await CargarClientes();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}");
            }
        }

        private async Task cargarVentas()
        {
            //Obtener todas las ventas del mes
            var ventas = await _apiService.GetVentasAsync();

            //Se filtra por ventas confirmadas o enviadas
            var ventasReales = ventas.Where(v => v.state == "sale" || v.state == "sent").ToList();

            //Cuenta cuantas ventas hay
            int numeroVentas = ventasReales.Count;

            //Se define por defecto un objetivo de venta mensual, en este caso 20
            int objetivoMensual = 20;

            // 5. Configurar ProgressBar
            ProgressBarVentas.Minimum = 0;
            ProgressBarVentas.Maximum = objetivoMensual;
            ProgressBarVentas.Value = numeroVentas;


            // 7. Opcional: mostrar porcentaje
            double porcentaje = (double)numeroVentas / objetivoMensual * 100;
            //para el label:
            //lblNumeroVentas.Content = $"Ventas del mes: {numeroVentas} ({porcentaje:F1}% del objetivo)";


        }

        private async Task CargarFacturas()
        {
            var facturas = await _apiService.GetFacturacionAsync();
            var facturasPagadas = facturas.Where(f => f.payment_state.ToLower() == "paid");

            decimal totalFacturado = facturasPagadas.Sum(f => f.amount_total);

            //formato monetario
            lblTotalFacturado.Content = totalFacturado.ToString("C2", new CultureInfo("es-ES")); //C2 = C-> formato monetario y 2-> cantidad de decimales y 'es-ES'-> Símbolo de moneda
        }


        private async Task CargarPedidos()
        {
            var pedidos = await _apiService.GetPedidoPendienteAsync();
            var pedidosPendientes = pedidos.Where(p => p.state == "assigned" || p.state == "confirmed").ToList();

            dataGridPedidos.ItemsSource = pedidosPendientes;
        }



        private async Task CargarClientes()
        {
            var clientes = await _apiService.GetClientesDestacadosAsync();
            clientesObservable = new ObservableCollection<Cliente>(clientes);

            var view = CollectionViewSource.GetDefaultView(clientesObservable);
            view.SortDescriptions.Clear();

            view.SortDescriptions.Add(new SortDescription("nombre_cliente", ListSortDirection.Ascending));

            dataGridClientes.ItemsSource = view;
            //dataGridClientes.ItemsSource = clientes;
        }

        private async Task CargarStocks()
        {
            var stocks = await _apiService.GetStockAsync();
            stocksObservable = new ObservableCollection<Stock>(stocks);

            var view = CollectionViewSource.GetDefaultView(stocksObservable);
            view.SortDescriptions.Clear();

            view.SortDescriptions.Add(new SortDescription("nombre_producto", ListSortDirection.Ascending));

            dataGridStock.ItemsSource = view;
            //dataGridStock.ItemsSource = stocksObservable;
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

        private async void DataGrid_SelectionChangedClienteDestacado(object sender, SelectionChangedEventArgs e)
        {
            if (dataGridClientes.SelectedItem is Cliente clienteSeleccionado)
            {
                await Task.Delay(100);
                Clientes detalleWindow = new Clientes(clienteSeleccionado);
                detalleWindow.ShowDialog();

                //dataGridStock.SelectedItem = null; //Quita el fondo azul de la selección
            }
        }


        private async void DataGrid_SelectionChangedStock(object sender, SelectionChangedEventArgs e)
        {
            if (dataGridStock.SelectedItem is Stock stockSeleccionado)
            {
                await Task.Delay(100);
                ProductosStock detalleWindow = new ProductosStock(stockSeleccionado);
                detalleWindow.ShowDialog();

                //dataGridStock.SelectedItem = null; //Quita el fondo azul de la selección
            }
        }
        private async void DataGrid_SelectionChangedPedido(object sender, SelectionChangedEventArgs e)
        {
            if (dataGridPedidos.SelectedItem is Pedido pedidoSeleccionado)
            {
                await Task.Delay(100);
                PedidiosPendienteEnvio detalleWindow = new PedidiosPendienteEnvio(pedidoSeleccionado);
                detalleWindow.ShowDialog();

                //dataGridPedidos.SelectedItem = null; //Quita el fondo azul de la selección
            }
        }
    }
}
