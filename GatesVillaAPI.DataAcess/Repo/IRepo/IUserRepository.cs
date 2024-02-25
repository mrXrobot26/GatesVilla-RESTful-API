using GatesVillaAPI.Models.Models.DTOs.LoginAndRegisterDTOs;
using GatesVillaAPI.Models.Models.DTOs.UserDTOs;
using GatesVillaAPI.Models.Models.MyModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GatesVillaAPI.DataAcess.Repo.IRepo
{
    public interface IUserRepository
    {
        Task<bool> IsUniqe(string username);
        Task<LoginResponseDTO> Login(LoginRequestDTO loginRequestDTO);
        Task<UserDTO> Register(RegisterRequestDTO registerRequestDTO);
    }
}
