using InventarioAPI.Helpers;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace InventarioAPI.Models
{
    public class ProductoDTO
    {
        [Required]
        [Capitalize]
        public string ProdNombre { get; set; }
        [Required]
        [MayorCero]
        public int Precio { get; set; }
        [Required]
        public int ProveedorId { get; set; }
    }
}
