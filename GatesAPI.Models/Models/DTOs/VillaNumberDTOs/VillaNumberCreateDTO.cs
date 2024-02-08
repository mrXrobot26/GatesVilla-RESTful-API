using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GatesVillaAPI.Models.Models.DTOs.VillaDTOs
{
    public class VillaNumberCreateDTO
    {
        public int VillaNum { get; set; } 
        public string SpeacialDetails { get; set; }
        [Required]
        public int VillaId { get; set; }
    }
}
