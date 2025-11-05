using Grupo_1_Interfaces;
using Grupo_1_Interfaces.Models; 
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Documents;
using System.Xml.Linq;

namespace WpfApp.Services
{
    //Servicio para conectarse con la API
    public class ApiOdooService
    {
        private readonly HttpClient _client; //Objeto que hace peticiones http
        private readonly string _baseUrl = "http://localhost:5000"; //la url en donde está la API en este momento
        private int? _uid; //almacena el uid cuando se inicia sesión
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
        //Realiza login en Odoo con usuario, contrseña y nombre de la base de datos
        public async Task<bool> LoginAsync(string username, string password, string db)
        {
            var json = JsonConvert.SerializeObject(new
            {
                username,
                password,
                db
            });

            var content = new StringContent(json, Encoding.UTF8, "application/json");//se guarda la información en 'content'
            var response = await _client.PostAsync($"{_baseUrl}/login", content); //y se pasa a la api

            if (!response.IsSuccessStatusCode) //si la respuesta es errónea devuelve 'false'
                return false;

            //sino, obtiene la información que devuelve la api, en este caso 'uid'
            var jsonString = await response.Content.ReadAsStringAsync();
            var data = JsonConvert.DeserializeObject<dynamic>(jsonString);

            if (data.uid != null)
            {
                _uid = (int)data.uid;
                Console.WriteLine($" Login correcto. UID: {_uid}");
            }

            return true;
        }


        //Verifica si el usuario ha hehco login antes de hacer peticiones
        private void VerificarLogin()
        {
            if (_uid == null)
                throw new Exception("Debes iniciar sesión antes de hacer peticiones a la API.");
        }


        // Obtener las ventas del mes
        public async Task<List<Venta>> GetVentasAsync()
        {
            //devuelve ventas del mes de noviembre de 2025
            string url = $"{_baseUrl}/api?table=sale.order&filter=date_order:gte:2025-11-01," +
                         "date_order:lte:2025-11-30&fields=name,partner_id,amount_total,state,date_order&sort=date_order:asc";

            var response = await _client.GetAsync(url);//Hace una petición get

            if (!response.IsSuccessStatusCode)
                throw new Exception($"Error al obtener ventas: {response.StatusCode}");

            //si todo está bien se obtiene la información que devuelve la api
            var jsonString = await response.Content.ReadAsStringAsync();

            var root = JsonConvert.DeserializeObject<dynamic>(jsonString);
            
            var ventasJson = Convert.ToString(root.message);


            var ventaJson = root.message;

            List<Venta> pedidoList = new List<Venta>();

            //recorre cada venta y la convierte en un objeto de 'Venta'
            foreach (var item in ventaJson)
            {
                Venta v = new Venta();

                v.name = item.name;
                v.date_order = item.date_order;
                v.nombre_cliente = item.partner_id[1]; //obtiene la segund aposición ejemplo: "partner_id": [ 11, "Gemini Furniture"]
                v.amount_total = (decimal)item.amount_total;
                v.state = item.state;

                pedidoList.Add(v);

            }
            return pedidoList; //devuelve una lista de ventas
        }


        //Obtener total facturas del mes
        public async Task<List<Factura>> GetFacturacionAsync()
        {
            //devuelve las facturas del mes
            string url = $"{_baseUrl}/api?table=account.move&filter=move_type:eq:out_invoice," +
                         "date:gte:2025-11-01,date:lte:2025-11-30&" +
                         "fields=name,date,partner_id,amount_untaxed,amount_tax,amount_total,state,payment_state&sort=date:asc";

            var response = await _client.GetAsync(url);
            if (!response.IsSuccessStatusCode)
                throw new Exception($"Error al obtener facturación: {response.StatusCode}");

            var jsonString = await response.Content.ReadAsStringAsync();
            var root = JsonConvert.DeserializeObject<dynamic>(jsonString);

            var facturasJson = root.message;
            List<Factura> facturas = new List<Factura>();

            foreach (var item in facturasJson)
            {
                Factura f = new Factura();

                f.name = item.name;
                f.date = item.date;
                f.amount_total = (decimal)item.amount_total;
                f.state = item.state;
                f.payment_state = item.payment_state;

                // Obtener nombre del cliente desde partner_id
                if (item.partner_id != null && item.partner_id.HasValues)
                {
                    f.partner_id = (int)item.partner_id[0];       // ID del cliente
                    f.nombre_cliente = (string)item.partner_id[1]; // Nombre del cliente
                }
                else //Por si la fsctura no tiene ingresado el cliente, asigna valores por defecto
                {
                    f.partner_id = 0;
                    f.nombre_cliente = "Sin cliente";
                }

                facturas.Add(f);
            }
            return facturas;
        }


        // Obtener clientes destacados
        public async Task<List<Cliente>> GetClientesDestacadosAsync()
        {
            //Devuelve clientes destacados
            string url = $"{_baseUrl}/api?table=res.partner&filter=category_id.name:eq:Destacado&" +
                         "fields=name,email,phone,street,city,zip&sort=name:asc";


            var response = await _client.GetAsync(url);

            if (!response.IsSuccessStatusCode)
            {
                throw new Exception($"Error al obtener clientes destacados: {response.StatusCode}");
            }

            var jsonString = await response.Content.ReadAsStringAsync();

            var root = JsonConvert.DeserializeObject<dynamic>(jsonString);

            var clientesJson = Convert.ToString(root.message);

            //if (clientesJson == "NotFound")
            //{
            //    MessageBox.Show("Aún no hay clientes destacados.", "Información", MessageBoxButton.OK, MessageBoxImage.Information);
            //    return new List<Cliente>();
            //}

            var clientes = JsonConvert.DeserializeObject<List<Cliente>>(clientesJson);

            return clientes;
        }


        // Obtener stock
        public async Task<List<Stock>> GetStockAsync()
        {
            //devuelve información de stock de cada producto
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
            //devuelve pedidos que aún no han sido enviados
            string url = $"{_baseUrl}/api?table=stock.picking&filter=state:!=:done,picking_type_code:=:outgoing&fields=name,"+
                "origin,state,scheduled_date,partner_id&sort=scheduled_date:asc";

            var response = await _client.GetAsync(url);

            if (!response.IsSuccessStatusCode)
                throw new Exception($"Error al obtener los pedidos pendientes: {response.StatusCode}");

            var jsonString = await response.Content.ReadAsStringAsync();

            var root = JsonConvert.DeserializeObject<dynamic>(jsonString);

            var pedidosJson = Convert.ToString(root.message);

            var pedidoJson = root.message;

            List<Pedido> pedidoList = new List<Pedido>();

            foreach (var item in pedidoJson)
            {
                Pedido p = new Pedido();

                p.name = item.name;
                p.origin = item.origin;
                p.scheduled_date = item.scheduled_date;
                p.state = item.state;

                p.nombre_cliente = item.partner_id[1].ToString();

                pedidoList.Add(p);
            }
            return pedidoList;
        }
    }
}
