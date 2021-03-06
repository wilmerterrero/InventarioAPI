﻿using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using InventarioAPI.Entities;
using InventarioAPI.Services;
using InventarioAPI.Models;

namespace InventarioAPI.Controllers.V1
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class ProductosController : ControllerBase
    {
        private readonly IProductoService productoService;

        public ProductosController(IProductoService productoService)
        {
            this.productoService = productoService;
        }

        [HttpGet(Name = "ObtenerProductos")]
        public async Task<ActionResult<IEnumerable<Producto>>> Get()
        {
            var productos = await productoService.GetProductos();
            return Ok(productos);
        }

        [HttpGet("{id}", Name = "ObtenerProducto")]
        public async Task<ActionResult<Producto>> Get(int id)
        {
            var producto = await productoService.GetProducto(id);

            if (producto == null)
            {
                return NotFound($"El Id {id} no se ha encontrado.");
            }

            return Ok(producto);
        }

        [HttpPost(Name = "CrearProducto")]
        public async Task<ActionResult> Post([FromBody]ProductoDTO productoFromBody)
        {
            var producto = await productoService.PostProducto(productoFromBody);

            return new CreatedAtRouteResult("ObtenerProducto", new { id = producto.Id }, producto);
        }

        [HttpPut("{id}", Name = "ActualizarProducto")]
        public async Task<IActionResult> Put(int id, ProductoDTO producto)
        {
            if (!await productoService.ProductoExists(id))
            {
                return BadRequest($"El Id {id} no se ha encontrado.");
            }

            await productoService.PutProducto(id, producto);

            return NoContent();
        }

        [HttpDelete("{id}", Name = "EliminarProducto")]
        public async Task<ActionResult<Producto>> DeleteProducto(int id)
        {
            
            if (!await productoService.ProductoExists(id))
            {
                return NotFound($"El Id {id} no se ha encontrado.");
            }

            var producto = await productoService.DeleteProducto(id);

            return producto;
        }
    }
}
