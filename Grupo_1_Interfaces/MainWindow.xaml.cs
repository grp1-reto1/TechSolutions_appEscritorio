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
        //Observables para 'binding' en datagrid
        private ObservableCollection<Stock> stocksObservable;
        private ObservableCollection<Cliente> clientesObservable;
        public MainWindow()
        {
            InitializeComponent();
            _apiService = new ApiOdooService();
            //Inicia la ventana y carga los datos de forma asincrona
            _ = InicializarAsync();
            //login para no tener que estar pidiendo todo el rato registarse cada vez que se abra una ventana
            _ = _apiService.LoginAsync("xiaoenbergaretxe@gmail.com", "YRU475I2", "TechSolutions2.0");

        }


        //Inicializa la ventana cargando, ventas, pedidos, fcturas, stock y clientes
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


        //Obtiene todas las ventas del mes
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

            ProgressBarVentas.Minimum = 0;
            ProgressBarVentas.Maximum = objetivoMensual;
            ProgressBarVentas.Value = numeroVentas;

            // mostrar porcentaje
            double porcentaje = (double)numeroVentas / objetivoMensual * 100;
            //para el label:
            //lblNumeroVentas.Content = $"Ventas del mes: {numeroVentas} ({porcentaje:F1}% del objetivo)";
        }


        //Carga las facturas pagadas, obtiene el total de facturas y el formato monetario
        private async Task CargarFacturas()
        {
            var facturas = await _apiService.GetFacturacionAsync();
            var facturasPagadas = facturas.Where(f => f.payment_state.ToLower() == "paid");

            decimal totalFacturado = facturasPagadas.Sum(f => f.amount_total);

            //formato monetario
            lblTotalFacturado.Content = totalFacturado.ToString("C2", new CultureInfo("es-ES")); 
            //C2 = C-> formato monetario y 2-> cantidad de decimales y 'es-ES'-> Símbolo de moneda
        }


        //Obtiene los pedidos pendientes de envío  para visualizar en datagrid
        private async Task CargarPedidos()
        {
            var pedidos = await _apiService.GetPedidoPendienteAsync();
            var pedidosPendientes = pedidos.Where(p => p.state == "assigned" || p.state == "confirmed").ToList();

            dataGridPedidos.ItemsSource = pedidosPendientes;
        }


        //Se obtiene todos los clientes que estén en 'Destacado'
        private async Task CargarClientes()
        {
            var clientes = await _apiService.GetClientesDestacadosAsync();
            clientesObservable = new ObservableCollection<Cliente>(clientes);

            var view = CollectionViewSource.GetDefaultView(clientesObservable);
            view.SortDescriptions.Clear();

            view.SortDescriptions.Add(new SortDescription("nombre_cliente", ListSortDirection.Ascending));
            //se ordena la lista por nombre de cliente ascendiente

            dataGridClientes.ItemsSource = view;
            //dataGridClientes.ItemsSource = clientes;
        }


        //Obtiene la información del stock para visualizarlo en datagrid
        private async Task CargarStocks()
        {
            var stocks = await _apiService.GetStockAsync();
            stocksObservable = new ObservableCollection<Stock>(stocks);

            var view = CollectionViewSource.GetDefaultView(stocksObservable);
            view.SortDescriptions.Clear();

            view.SortDescriptions.Add(new SortDescription("nombre_producto", ListSortDirection.Ascending)); 
            //se ordena la lista mediante el nombre del producto ascendiente

            dataGridStock.ItemsSource = view;
            //dataGridStock.ItemsSource = stocksObservable;
        }


        //Al seleccionar el botón se manda por parametro el 'login'
        //Evento de 'más detalles' se abre la ventana de ventas del mes
        private void Boton_ventas(object sender, RoutedEventArgs e)
        {
            NumeroVentasMes ventas = new NumeroVentasMes(_apiService);
            ventas.ShowDialog();

        }


        //Al seleccionar el botón se manda por parametro el 'login'
        //Evento de 'más detalles' se abre la ventana del total facturado
        private void Boton_facturas(object sender, RoutedEventArgs e)
        {
            TotalFacturado facturas = new TotalFacturado(_apiService);
            facturas.ShowDialog();
        }


        //Datagrid cliente
        //Se abre la ventana al seleccionar un cliente
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


        //Datagrid stock
        //Se abre la ventana al seleccionar un producto
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


        //Datagrid pedido
        //Se abre la ventana al seleccionar un pedido
        private async void DataGrid_SelectionChangedPedido(object sender, SelectionChangedEventArgs e)
        {
            if (dataGridPedidos.SelectedItem is Pedido pedidoSeleccionado)
            {
                await Task.Delay(100);
                PedidosPendienteEnvio detalleWindow = new PedidosPendienteEnvio(pedidoSeleccionado);
                detalleWindow.ShowDialog();

                //dataGridPedidos.SelectedItem = null; //Quita el fondo azul de la selección
            }
        }

        //Cierre de ventana principal
        private void Boton_CerrarVentana(object sender, RoutedEventArgs e)
        {
            // Pregunta al usuario si está seguro de cerrar la ventana
            MessageBoxResult result = MessageBox.Show( "¿Estás seguro de cerrar la ventana?",
                "Confirmar cierre",
                MessageBoxButton.YesNo,
                MessageBoxImage.Question
            );

            // Si el usuario pulsa "Sí", se cierra la ventana
            if (result == MessageBoxResult.Yes)
            {
                this.Close();
            }
        }
    }
}
