using InventarioAPI.Entities;
using InventarioAPI.Models;
using InventarioAPI.Services;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace InventarioAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProveedoresController: ControllerBase
    {
        private readonly IProveedorService proveedorService;

        public ProveedoresController(IProveedorService proveedorService)
        {
            this.proveedorService = proveedorService;
        }

        [HttpGet(Name = "ObtenerProveedores")]
        public async Task<ActionResult<IEnumerable<Proveedor>>> Get()
        {
            var proveedores = await proveedorService.GetProveedores();
            return Ok(proveedores);
        }

        [HttpGet("{id}", Name = "ObtenerProveedor")]
        public async Task<ActionResult<Proveedor>> Get(int id)
        {
            var proveedor = await proveedorService.GetProveedor(id);

            if (!await proveedorService.ProveedorExists(id))
            {
                return NotFound($"El Id {id} no se ha encontrado.");
            }

            return proveedor;
        }

        [HttpPost(Name = "CrearProveedor")]
        public async Task<ActionResult> Post([FromBody] ProveedorDTO proveedorFromBody)
        {
            var proveedor = await proveedorService.PostProveedor(proveedorFromBody);
            return new CreatedAtRouteResult("ObtenerProveedor", new { id = proveedor.Id }, proveedor);
        }

        [HttpPut("{id}", Name = "ActualizarProveedor")]
        public async Task<ActionResult> Put(int id, [FromBody] ProveedorDTO proveedorFromBody)
        {
            if (!await proveedorService.ProveedorExists(id))
            {
                return NotFound($"El Id {id} no se ha encontrado.");
            }

            await proveedorService.PutProveedor(id, proveedorFromBody);

            return NoContent();
        }

        [HttpDelete("{id}", Name = "EliminarProveedor")]
        public async Task<ActionResult<Proveedor>> Delete(int id)
        {
            if (!await proveedorService.ProveedorExists(id))
            {
                return NotFound($"El Id {id} no se ha encontrado.");
            }

            var proveedor = await proveedorService.DeleteProveedor(id);

            return proveedor;
        }
    }
}
