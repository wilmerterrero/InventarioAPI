using InventarioAPI.Helpers;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace InventarioAPI.Entities
{
    public class Cliente
    {
        public int Id { get; set; }
        [Required]
        [Capitalize]
        public string Nombre { get; set; }
        [Required]
        [Capitalize]
        public string Apellido { get; set; }
        [Required]
        [MayorCero]
        public int Presupuesto { get; set; }
    }
}
