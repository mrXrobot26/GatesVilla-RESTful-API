﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GatesVillaAPI.Models.Models.MyModels
{
    public class VillaNumber
    {
        [Key , DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int VillaNum { get; set; }
        public string SpeacialDetails { get; set; } 
        public DateTime CraetedDate { get; set; }
        public DateTime UpdatedDate { get; set; }

    }
}
