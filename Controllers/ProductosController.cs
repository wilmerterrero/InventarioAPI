using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using InventarioAPI.Entities;
using Microsoft.Extensions.Logging;
using InventarioAPI.Context;

namespace InventarioAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductosController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly ILogger<ProductosController> logger;

        public ProductosController(AppDbContext context, ILogger<ProductosController> logger)
        {
            _context = context;
            this.logger = logger;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Producto>>> Get()
        {
            return await _context.Productos.Include(x => x.Proveedor).ToListAsync();
        }

        [HttpGet("{id}", Name = "GetProducto")]
        public async Task<ActionResult<Producto>> Get(int id)
        {
            var producto = await _context.Productos.Include(x => x.Proveedor).FirstOrDefaultAsync(x => x.Id == id);

            if (producto == null)
            {
                logger.LogWarning($"El Id {id} no se ha encontrado.");
                return NotFound();
            }

            return producto;
        }

        [HttpPost]
        public async Task<ActionResult> Post([FromBody]Producto producto)
        {
            var prodCodigo = $"{producto.ProdNombre[0]}-{new Random(1000)}";
            producto.ProdCodigo = prodCodigo;
            _context.Productos.Add(producto);
            await _context.SaveChangesAsync();

            return new CreatedAtRouteResult("GetProducto", new { id = producto.Id }, producto);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, Producto producto)
        {
            if (id != producto.Id)
            {
                logger.LogWarning($"El Id {id} no se ha encontrado.");
                return BadRequest();
            }

            _context.Entry(producto).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ProductoExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<Producto>> DeleteProducto(int id)
        {
            var producto = await _context.Productos.FindAsync(id);
            if (producto == null)
            {
                logger.LogWarning($"El Id {id} no se ha encontrado.");
                return NotFound();
            }

            _context.Productos.Remove(producto);
            await _context.SaveChangesAsync();

            return producto;
        }

        private bool ProductoExists(int id)
        {
            return _context.Productos.Any(e => e.Id == id);
        }
    }
}
