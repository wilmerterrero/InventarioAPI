using InventarioAPI.Helpers;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace InventarioAPI.Entities
{
    public class Producto : IValidatableObject
    {
        public int Id { get; set; }
        [Required]
        [Capitalize]
        public string ProdNombre { get; set; }
        public string ProdCodigo { get; set; }
        [Required]
        [MayorCero]
        public int Precio { get; set; }
        [Required]
        public int ProveedorId { get; set; }
        public Proveedor Proveedor { get; set; }
        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            var precioMax = 1000000;
            if(Precio >= precioMax)
            {
                yield return new ValidationResult("No se aceptan productos con un precio de $ 1.000.000 DOP", new string[] { nameof(Precio) } );
            }
        }
    }
}
