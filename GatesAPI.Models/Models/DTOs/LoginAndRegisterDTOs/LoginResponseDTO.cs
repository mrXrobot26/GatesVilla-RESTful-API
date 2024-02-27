using GatesVillaAPI.Models.Models.DTOs.UserDTOs;
using GatesVillaAPI.Models.Models.MyModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GatesVillaAPI.Models.Models.DTOs.LoginAndRegisterDTOs
{
    public class LoginResponseDTO
    {
        public UserDTO User { get; set; }
        public string Token { get; set; }
    }
}
