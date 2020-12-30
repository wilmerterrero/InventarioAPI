using InventarioAPI.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InventarioAPI.Controllers.V1
{
    [Route("api")]
    [ApiController]
    public class RootController : ControllerBase
    {
        [HttpGet(Name = "GetRoot")]
        public ActionResult<IEnumerable<Enlace>> Get()
        {
            List<Enlace> enlaces = new List<Enlace>
            {
                //enlaces de nuestra API solo los que no necesitan params
                new Enlace(href: Url.Link("GetRoot", new { }), rel: "self", metodo: "GET"),
                new Enlace(href: Url.Link("ObtenerProductos", new { }), rel: "productos", metodo: "GET"),
                new Enlace(href: Url.Link("CrearProducto", new { }), rel: "crear-producto", metodo: "POST"),
                new Enlace(href: Url.Link("ObtenerClientes", new { }), rel: "clientes", metodo: "GET"),
                new Enlace(href: Url.Link("CrearCliente", new { }), rel: "crear-cliente", metodo: "POST"),
                new Enlace(href: Url.Link("ObtenerProveedores", new { }), rel: "proveedores", metodo: "GET"),
                new Enlace(href: Url.Link("CrearProveedor", new { }), rel: "crear-proveedores", metodo: "POST")
            };

            return enlaces;
        }
    }
}
