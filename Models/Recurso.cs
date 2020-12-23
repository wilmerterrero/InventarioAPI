using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InventarioAPI.Models
{
    public class Recurso
    {
        public List<Enlace> Enlaces { get; set; } = new List<Enlace>();
    }
}
