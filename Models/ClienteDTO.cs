﻿using InventarioAPI.Helpers;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace InventarioAPI.Models
{
    public class ClienteDTO
    {
        public int Id { get; set; }
        [Required]
        [Capitalize]
        public string Nombre { get; set; }
        [Required]
        [Capitalize]
        public string Apellido { get; set; }
    }
}
