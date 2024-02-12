﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GatesVillaAPI.Models.Models.DTOs.VillaDTOs
{
    public class VillaCreateDTO
    {

        [Required]
        public string Name { get; set; }
        public string? Details { get; set; }
        [Required]
        public double Rate { get; set; }
        public int? Occupancy { get; set; }
        public int? Sqft { get; set; }
        public string? ImageUrl { get; set; }
        public string? Amenity { get; set; }
    }
}
