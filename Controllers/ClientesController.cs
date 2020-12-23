using AutoMapper;
using InventarioAPI.Context;
using InventarioAPI.Entities;
using InventarioAPI.Models;
using InventarioAPI.Services;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CientesAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClientesController : ControllerBase
    {
        private readonly AppDbContext context;
        private readonly IClienteService clienteService;

        public ClientesController(AppDbContext context, IClienteService clienteService)
        {
            this.context = context;
            this.clienteService = clienteService;
        }

        [HttpGet(Name = "ObtenerClientes")]
        public async Task<ActionResult<IEnumerable<ClienteDTO>>> Get()
        {
            var clientes = await clienteService.GetClientes();
            return Ok(clientes);
        }

        [HttpGet("{id}", Name = "ObtenerCliente")]
        public async Task<ActionResult<ClienteDTO>> Get(int id)
        {
            if(!await clienteService.ClienteExists(id))
            {
                return NotFound($"El Id {id} no se ha encontrado.");
            }

            var cliente = await clienteService.GetCliente(id);
            return Ok(cliente);
        }

        [Route("presupuesto")]
        public IActionResult GetPresupuestoTotal()
        {
            return Ok(clienteService.GetPresupuestoTotal());
        }

        [HttpPost(Name = "CrearCliente")]
        public async Task<ActionResult> Post([FromBody] ClienteCreacionDTO clienteFromBody)
        {
            var cliente = await clienteService.PostClientes(clienteFromBody);
            return new CreatedAtRouteResult("ObtenerCliente", new { id = cliente.Id }, cliente);
        }

        [HttpPut("{id}", Name = "ActualizarCliente")]
        public ActionResult Put(int id, [FromBody] ClienteCreacionDTO clienteFromBody)
        {
            clienteService.PutClientes(id, clienteFromBody);

            return NoContent();
        }

        [HttpPatch("{id}", Name = "PatchCliente")]
        public async Task<ActionResult> Patch(int id, [FromBody] JsonPatchDocument<Cliente> patchDocument)
        {
            if (patchDocument == null)
            {
                return BadRequest();
            }

            if (!await clienteService.ClienteExists(id))
            {
                return NotFound($"El Id {id} no se ha encontrado.");
            }

            var cliente = await context.Clientes.FirstOrDefaultAsync(x => x.Id == id);

            patchDocument.ApplyTo(cliente, ModelState);

            var isValid = TryValidateModel(cliente);

            if (!isValid)
            {
                return BadRequest(ModelState);
            }

            await context.SaveChangesAsync();

            return NoContent();
        }

        [HttpDelete("{id}", Name = "EliminarCliente")]
        public async Task<ActionResult<Cliente>> Delete(int id)
        {
            if(!await clienteService.ClienteExists(id))
            {
                return NotFound($"El Id {id} no se ha encontrado.");
            }

            var cliente = await clienteService.DeleteCliente(id);

            return cliente;
        }
    }
}
