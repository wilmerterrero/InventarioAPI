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
        public Task<ClienteDTO> GetCliente(int id);
        public Task<Cliente> PostClientes(ClienteCreacionDTO clienteFromBody);
        public Task PutClientes(int id, ClienteCreacionDTO clienteFromBody);
        public Task<Cliente> DeleteCliente(int id);
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

        public async Task<ClienteDTO> GetCliente(int id)
        {
            var cliente = await context.Clientes.FirstOrDefaultAsync(x => x.Id == id);
            var _cliente = mapper.Map<ClienteDTO>(cliente);
            return _cliente;
        }
        public async Task<Cliente> PostClientes(ClienteCreacionDTO clienteFromBody)
        {
            var cliente = mapper.Map<Cliente>(clienteFromBody);

            context.Clientes.Add(cliente);
            await context.SaveChangesAsync();

            return cliente;
        }
        public async Task PutClientes(int id, ClienteCreacionDTO clienteFromBody)
        {
            var cliente = mapper.Map<Cliente>(clienteFromBody);
            cliente.Id = id;
            context.Entry(cliente).State = EntityState.Modified;
            await context.SaveChangesAsync();
        }
        public async Task<Cliente> DeleteCliente(int id)
        {
            var cliente = await context.Clientes.FindAsync(id);
            context.Clientes.Remove(cliente);
            await context.SaveChangesAsync();

            return cliente;
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
