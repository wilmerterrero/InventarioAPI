using AutoMapper;
using InventarioAPI.Context;
using InventarioAPI.Entities;
using InventarioAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InventarioAPI.Services
{
    public interface IClienteService
    {
        public Task<IEnumerable<ClienteDTO>> GetClientes();
        public ClienteDTO GetCliente(Cliente cliente);
        public Task<Cliente> PostClientes(ClienteCreacionDTO cliente);
        public Task PutClientes(ClienteCreacionDTO _cliente, int id);
        public Task DeleteCliente(int id);
        public Task<int> GetPresupuestoTotal();
        public Task<bool> ClienteExists(int id);
    }

    public class ClienteService : IClienteService
    {
        private readonly AppDbContext context;
        private readonly IMapper mapper;

        public ClienteService(AppDbContext context,
                              IMapper mapper)
        {
            this.context = context;
            this.mapper = mapper;
        }

        public async Task<IEnumerable<ClienteDTO>> GetClientes()
        {
            var clientes = await context.Clientes.ToListAsync();
            var _clientes = mapper.Map<List<ClienteDTO>>(clientes);
            return _clientes;
        }

        public ClienteDTO GetCliente(Cliente cliente)
        {
            var _cliente = mapper.Map<ClienteDTO>(cliente);

            return _cliente;
        }
        public async Task<Cliente> PostClientes(ClienteCreacionDTO _cliente)
        {
            var cliente = mapper.Map<Cliente>(_cliente);
            context.Clientes.Add(cliente);
            await context.SaveChangesAsync();
            var clienteDTO = mapper.Map<Cliente>(cliente);
            return clienteDTO;
        }
        public async Task PutClientes(ClienteCreacionDTO _cliente, int id)
        {
            var cliente = mapper.Map<Cliente>(_cliente);
            cliente.Id = id;
            context.Entry(cliente).State = EntityState.Modified;
            await context.SaveChangesAsync();
        }
        public async Task DeleteCliente(int id)
        {
            var clienteId = await context.Clientes.Select(x => x.Id).FirstOrDefaultAsync(x => x == id);
            context.Clientes.Remove(new Cliente { Id = clienteId });
            await context.SaveChangesAsync();
        }
        public async Task<int> GetPresupuestoTotal()
        {
            return await context.Clientes.SumAsync(x => x.Presupuesto);
        }
        public async Task<bool> ClienteExists(int id)
        {
            return await context.Clientes.AnyAsync(e => e.Id == id);
        }

        
    }
}
