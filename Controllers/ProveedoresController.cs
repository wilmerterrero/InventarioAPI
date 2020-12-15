using InventarioAPI.Context;
using InventarioAPI.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InventarioAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProveedoresController: ControllerBase
    {
        private readonly AppDbContext context;
        private readonly ILogger<ProveedoresController> logger;

        public ProveedoresController(AppDbContext context, ILogger<ProveedoresController> logger)
        {
            this.context = context;
            this.logger = logger;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Proveedor>>> Get()
        {
            return await context.Proveedores.Include(x => x.Productos).ToListAsync();
        }

        [HttpGet("{id}", Name = "GetProveedor")]
        public async Task<ActionResult<Proveedor>> Get(int id)
        {
            var proveedor = await context.Proveedores.Include(x => x.Productos).FirstOrDefaultAsync(x => x.Id == id);

            if (proveedor == null)
            {
                logger.LogWarning($"El Id {id} no se ha encontrado.");
                return NotFound();
            }

            return proveedor;
        }

        [HttpPost]
        public async Task<ActionResult> Post([FromBody] Proveedor proveedor)
        {
            context.Proveedores.Add(proveedor);
            await context.SaveChangesAsync();
            return new CreatedAtRouteResult("GetProveedor", new { id = proveedor.Id }, proveedor);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> Put(int id, [FromBody] Proveedor proveedor)
        {
            if (id != proveedor.Id)
            {
                logger.LogWarning($"El Id {id} no se ha encontrado.");
                return NotFound();
            }

            context.Entry(proveedor).State = EntityState.Modified;
            await context.SaveChangesAsync();

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<Proveedor>> Delete(int id)
        {
            var proveedorId = await context.Proveedores.Select(x => x.Id).FirstOrDefaultAsync(x => x == id);

            if (proveedorId == default(int))
            {
                logger.LogWarning($"El Id {id} no se ha encontrado.");
                return NotFound();
            }

            context.Proveedores.Remove(new Proveedor { Id = proveedorId });

            await context.SaveChangesAsync();

            return NoContent();
        }
    }
}
