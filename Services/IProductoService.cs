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
    public interface IProductoService
    {
        public Task<IEnumerable<Producto>> GetProductos();
        public Task<Producto> GetProducto(int id);
        public Task<Producto> PostProducto(ProductoDTO productoFromBody);
        public Task PutProducto(int id, ProductoDTO productoFromBody);
        public Task<Producto> DeleteProducto(int id);
        public Task<bool> ProductoExists(int id);
    }

    public class ProductoService : IProductoService
    {
        private readonly AppDbContext context;
        private readonly IMapper mapper;

        public ProductoService(AppDbContext context, IMapper mapper)
        {
            this.context = context;
            this.mapper = mapper;
        }
        public async Task<IEnumerable<Producto>> GetProductos()
        {
            var productosConProveedor = await context.Productos
                .Include(x => x.Proveedor)
                .ToListAsync();
            return productosConProveedor;
        }
        public async Task<Producto> GetProducto(int id)
        {
            var producto = await context.Productos
                .Include(x => x.Proveedor)
                .FirstOrDefaultAsync(x => x.Id == id);
            return producto;
        }
        public async Task<Producto> PostProducto(ProductoDTO productoFromBody)
        {
            var producto = mapper.Map<Producto>(productoFromBody);

            var prodCodigo = $"{producto.ProdNombre[0]}-{producto.ProveedorId}";
            producto.ProdCodigo = prodCodigo;

            context.Productos.Add(producto);

            await context.SaveChangesAsync();

            return producto;
        }
        public async Task PutProducto(int id, ProductoDTO productoFromBody)
        {
            context.Entry(productoFromBody).State = EntityState.Modified;
            await context.SaveChangesAsync();
        }
        public async Task<Producto> DeleteProducto(int id)
        {
            var producto = await context.Productos.FindAsync(id);

            context.Productos.Remove(producto);

            await context.SaveChangesAsync();

            return producto;
        }
        public async Task<bool> ProductoExists(int id)
        {
            return await context.Productos.AnyAsync(e => e.Id == id);
        }
    }
}
