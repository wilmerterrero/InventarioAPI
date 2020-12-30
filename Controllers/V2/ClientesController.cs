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

namespace InventarioAPI.Controllers.v2
{
    [Route("api/v2/[controller]")]
    [ApiController]
    public class ClientesController : ControllerBase
    {
        private readonly IClienteService clienteService;

        public ClientesController(IClienteService clienteService)
        {
            this.clienteService = clienteService;
        }

        [HttpGet("presupuesto")]
        public IActionResult GetPresupuestoTotal()
        {
            return Ok(clienteService.GetPresupuestoTotal());
        }
    }
}
