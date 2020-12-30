using InventarioAPI.Context;
using InventarioAPI.Entities;
using InventarioAPI.Models;
using InventarioAPI.Services;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InventarioAPI.Controllers.V1
{
    [Route("api/v1/[controller]")]
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

        /// <summary>
        /// Obtiene todos los clientes en el sistema
        /// </summary>
        /// <returns>IEnumerable de ClienteDTO</returns>
        [HttpGet(Name = "ObtenerClientes")]
        public async Task<ActionResult<IEnumerable<ClienteDTO>>> GetClientes(int numeroDePaginas = 1, int cantidadDeRegistros = 10)
        {
            var query = context.Clientes.AsQueryable();
            var totalRegistros = query.Count();

            var clientes = await clienteService.GetClientes(numeroDePaginas, cantidadDeRegistros);

            int cantidadDePaginas = ((int)Math.Ceiling((double)totalRegistros / cantidadDeRegistros));

            Response.Headers["X-Total-Registros"] = totalRegistros.ToString();
            Response.Headers["X-Cantidad-Paginas"] = cantidadDePaginas.ToString();

            return Ok(clientes);
        }

        /// <summary>
        /// Obtiene un elemento especifico
        /// </summary>
        /// <param name="id">Id del elemento</param>
        /// <returns>ClienteDTO</returns>
        [HttpGet("{id}", Name = "ObtenerCliente")]
        public async Task<ActionResult<ClienteDTO>> GetCliente(int id)
        {
            if(!await clienteService.ClienteExists(id))
            {
                return NotFound($"El Id {id} no se ha encontrado.");
            }

            var cliente = await clienteService.GetCliente(id);

            GenerarEnlaces(cliente);

            return Ok(cliente);
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
        private void GenerarEnlaces(ClienteDTO cliente)
        {
            cliente.Enlaces = new List<Enlace>()
            {
                 new Enlace(href: Url.Link("ObtenerCliente", new { id = cliente.Id }), rel: "self", metodo: "GET"),
                 new Enlace(href: Url.Link("ActualizarCliente", new { id = cliente.Id }), rel: "actualizar-cliente", metodo: "PUT"),
                 new Enlace(href: Url.Link("EliminarCliente", new { id = cliente.Id }), rel: "eliminar-cliente", metodo: "DELETE"),
            };
        }
    }
}
