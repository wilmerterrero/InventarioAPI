using AutoMapper;
using InventarioAPI.Context;
using InventarioAPI.Entities;
using InventarioAPI.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InventarioAPI.Services
{
    public interface IProveedorService
    {
        public Task<IEnumerable<Proveedor>> GetProveedores();
        public Task<Proveedor> GetProveedor(int id);
        public Task<Proveedor> PostProveedor(ProveedorDTO proveedorFromBody);
        public Task PutProveedor(int id, ProveedorDTO proveedorFromBody);
        public Task<Proveedor> DeleteProveedor(int id);
        public Task<bool> ProveedorExists(int id);
    }
    public class ProveedorService : IProveedorService
    {
        private readonly AppDbContext context;
        private readonly IMapper mapper;

        public ProveedorService(AppDbContext context, 
                                IMapper mapper)
        {
            this.context = context;
            this.mapper = mapper;
        }

        public async Task<IEnumerable<Proveedor>> GetProveedores()
        {
            var proveedores = await context.Proveedores
                .Include(x => x.Productos)
                .ToListAsync();
            return proveedores;
        }

        public async Task<Proveedor> GetProveedor(int id)
        {
            var proveedor = await context.Proveedores
                .Include(x => x.Productos)
                .FirstOrDefaultAsync(x => x.Id == id);
            return proveedor;
        }

        public async Task<Proveedor> PostProveedor(ProveedorDTO proveedorFromBody)
        {
            var proveedor = mapper.Map<Proveedor>(proveedorFromBody);
            context.Proveedores.Add(proveedor);
            await context.SaveChangesAsync();

            return proveedor;
        }

        public async Task PutProveedor(int id, ProveedorDTO proveedorFromBody)
        {
            var proveedor = mapper.Map<Proveedor>(proveedorFromBody);
            proveedor.Id = id;
            context.Entry(proveedor).State = EntityState.Modified;

            await context.SaveChangesAsync();
        }

        public async Task<Proveedor> DeleteProveedor(int id)
        {
            var proveedor = await context.Proveedores.FindAsync(id);
            context.Proveedores.Remove(proveedor);
            await context.SaveChangesAsync();

            return proveedor;
        }

        public async Task<bool> ProveedorExists(int id)
        {
            return await context.Proveedores.AnyAsync(e => e.Id == id);
        }
    }
}
