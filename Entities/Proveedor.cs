using InventarioAPI.Helpers;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace InventarioAPI.Entities
{
    public class Proveedor
    {
        public int Id { get; set; }
        [Required]
        [Capitalize]
        public string ProvNombre { get; set; }
        public List<Producto> Productos { get; set; }
    }
}
