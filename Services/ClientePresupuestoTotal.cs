using InventarioAPI.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InventarioAPI.Services
{
    public class ClientePresupuestoTotal : IClientesPresupuestoTotal
    {
        private readonly AppDbContext context;

        public ClientePresupuestoTotal(AppDbContext context)
        {
            this.context = context;
        }

        public int GetPresupuestoTotal()
        {
            return context.Clientes.Sum(x => x.Presupuesto);
        }
    }
}
