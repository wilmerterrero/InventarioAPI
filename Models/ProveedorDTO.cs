using InventarioAPI.Helpers;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace InventarioAPI.Models
{
    public class ProveedorDTO
    {
        [Required]
        [Capitalize]
        public string ProvNombre { get; set; }
    }
}
