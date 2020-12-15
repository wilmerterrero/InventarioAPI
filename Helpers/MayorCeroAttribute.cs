using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace InventarioAPI.Helpers
{
    public class MayorCeroAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value == null)
            {
                return ValidationResult.Success;
            }

            var presupuesto = (int)value;

            if(presupuesto <= 0)
            {
                return new ValidationResult("Debe ser mayor que 0");
            }

            return ValidationResult.Success;
        }
    }
}
