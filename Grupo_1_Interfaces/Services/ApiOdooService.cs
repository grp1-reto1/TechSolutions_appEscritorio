using Grupo_1_Interfaces;
using Grupo_1_Interfaces.Models; 
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace WpfApp.Services
{
    public class ApiOdooService
    {
        private readonly HttpClient _client;
        private readonly string _baseUrl = "http://localhost:5000"; 
        private int? _uid;
        public ApiOdooService()
        {
            var handler = new HttpClientHandler
            {
                UseCookies = true,
                CookieContainer = new CookieContainer()
            };

            _client = new HttpClient(handler);
        }


        // LOGIN (una sola vez)
        public async Task<bool> LoginAsync(string username, string password, string db)
        {
            var json = JsonConvert.SerializeObject(new
            {
                username,
                password,
                db
            });

            var content = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await _client.PostAsync($"{_baseUrl}/login", content);

            if (!response.IsSuccessStatusCode)
                return false;

            var jsonString = await response.Content.ReadAsStringAsync();
            var data = JsonConvert.DeserializeObject<dynamic>(jsonString);

            if (data.uid != null)
            {
                _uid = (int)data.uid;
                Console.WriteLine($"✅ Login correcto. UID: {_uid}");
            }

            return true;
        }

        private void VerificarLogin()
        {
            if (_uid == null)
                throw new Exception("Debes iniciar sesión antes de hacer peticiones a la API.");
        }


        // Obtener las ventas del mes
        public async Task<List<Venta>> GetVentasAsync()
        {

            string url = $"{_baseUrl}/api?table=sale.order&filter=date_order:gte:2025-11-01," +
                         "date_order:lte:2025-11-30&fields=name,partner_id,amount_total,state&sort=date_order:asc";

            var response = await _client.GetAsync(url);

            if (!response.IsSuccessStatusCode)
                throw new Exception($"Error al obtener ventas: {response.StatusCode}");
            var jsonString = await response.Content.ReadAsStringAsync();

            var root = JsonConvert.DeserializeObject<dynamic>(jsonString);
            
            var ventasJson = Convert.ToString(root.message);

            var ventas = JsonConvert.DeserializeObject<List<Venta>>(ventasJson);

            return ventas;

        }


        public async Task<List<Factura>> GetFacturacionAsync()
        {

            string url = $"{_baseUrl}/api?table=account.move&filter=move_type:eq:out_invoice," +
                         "date:gte:2025-11-01,date:lte:2025-11-30&" +
                         "fields=name,date,partner_id,amount_untaxed,amount_tax,amount_total,state,payment_state&sort=date:asc";
            var response = await _client.GetAsync(url);

            if (!response.IsSuccessStatusCode)
                throw new Exception($"Error al obtener facturación: {response.StatusCode}");

            var jsonString = await response.Content.ReadAsStringAsync();

            var root = JsonConvert.DeserializeObject<dynamic>(jsonString);

            var facturasJson = Convert.ToString(root.message);

            var facturas = JsonConvert.DeserializeObject<List<Factura>>(facturasJson);


            return facturas;
        }


        // Obtener clientes destacados
        public async Task<List<Cliente>> GetClientesDestacadosAsync()
        {
            string url = $"{_baseUrl}/api?table=res.partner&filter=category_id.name:eq:Destacado&" +
                         "fields=name,email,phone,street,city,zip&sort=name:asc";


            var response = await _client.GetAsync(url);

            if (!response.IsSuccessStatusCode)
                throw new Exception($"Error al obtener clientes destacados: {response.StatusCode}");

            var jsonString = await response.Content.ReadAsStringAsync();

            var root = JsonConvert.DeserializeObject<dynamic>(jsonString);

            var clientesJson = Convert.ToString(root.message);
            
            var clientes = JsonConvert.DeserializeObject<List<Cliente>>(clientesJson);

            return clientes;
        }


        // Obtener stock
        public async Task<List<Stock>> GetStockAsync()
        {
            string url = $"{_baseUrl}/api?table=stock.quant&fields=product_id,quantity," +
                         "reserved_quantity,available_quantity,location_id&sort=quantity:desc";

            var response = await _client.GetAsync(url);

            if (!response.IsSuccessStatusCode)
                throw new Exception($"Error al obtener stock: {response.StatusCode}");

            var jsonString = await response.Content.ReadAsStringAsync();

            var root = JsonConvert.DeserializeObject<dynamic>(jsonString);


            var stockArray = root.message;

            List<Stock> stockList = new List<Stock>();


            foreach (var item in stockArray)
            {
                Stock s = new Stock();

                s.id_producto = item.product_id[0];

                s.nombre_producto = item.product_id[1].ToString();

                s.location_id = item.location_id[1].ToString();

                s.available_quantity = float.Parse(item.available_quantity.ToString());
                s.reserved_quantity = float.Parse(item.reserved_quantity.ToString());
                s.Quantity = float.Parse(item.quantity.ToString());

                stockList.Add(s);

            }
            return stockList;
        }



        // Obtener pedidio pendiente de envio
        public async Task<List<Pedido>> GetPedidoPendienteAsync()
        {
            string url = $"{_baseUrl}/api?table=stock.picking&filter=state:!=:done,picking_type_code:=:outgoing&fields=name,origin,state,scheduled_date,partner_id&sort=scheduled_date:asc";

            var response = await _client.GetAsync(url);

            if (!response.IsSuccessStatusCode)
                throw new Exception($"Error al obtener clientes destacados: {response.StatusCode}");

            var jsonString = await response.Content.ReadAsStringAsync();

            var root = JsonConvert.DeserializeObject<dynamic>(jsonString);

            var pedidosJson = Convert.ToString(root.message);

            var pedidos = JsonConvert.DeserializeObject<List<Pedido>>(pedidosJson);

            return pedidos;
        }


        // Obtener pedidio pendiente de envio
        public async Task<List<Pedido>> GetPedidoPendienteAsync()
        {
            string url = $"{_baseUrl}/api?table=stock.picking&filter=state:!=:done,picking_type_code:=:outgoing&fields=name,origin,state,scheduled_date,partner_id&sort=scheduled_date:asc";

            var response = await _client.GetAsync(url);

            if (!response.IsSuccessStatusCode)
                throw new Exception($"Error al obtener clientes destacados: {response.StatusCode}");

            var jsonString = await response.Content.ReadAsStringAsync();

            var root = JsonConvert.DeserializeObject<dynamic>(jsonString);

            var pedidosJson = Convert.ToString(root.message);

            var pedidos = JsonConvert.DeserializeObject<List<Pedido>>(pedidosJson);

            return pedidos;
        }


    }
}
