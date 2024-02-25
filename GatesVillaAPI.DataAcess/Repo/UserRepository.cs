using AutoMapper;
using GatesVillaAPI.DataAcess.Data;
using GatesVillaAPI.DataAcess.Repo.IRepo;
using GatesVillaAPI.Models.Models;
using GatesVillaAPI.Models.Models.DTOs.LoginAndRegisterDTOs;
using GatesVillaAPI.Models.Models.DTOs.UserDTOs;
using GatesVillaAPI.Models.Models.MyModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace GatesVillaAPI.DataAcess.Repo
{
    public class UserRepository : IUserRepository
    {
        private readonly ApplicationDbContext db;
        private readonly IMapper mapper;
        private readonly UserManager<ApplicationUser> userManager;
        private readonly IConfiguration configuration;
        private string securityKay;
        public UserRepository(ApplicationDbContext db, IConfiguration configuration, UserManager<ApplicationUser> userManager,IMapper mapper)
        {
            this.db = db;
            this.configuration = configuration;
            this.userManager = userManager;
            this.mapper = mapper;
            securityKay = configuration.GetValue<string>("ApiSettings:Secret");
        }
        public async Task<bool> IsUniqe(string username)
        {
            var matchUsername = db.ApplicationUsers.FirstOrDefault(x => x.UserName == username);
            if (matchUsername == null)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public async Task<LoginResponseDTO> Login(LoginRequestDTO loginRequestDTO)
        {
            var user = db.ApplicationUsers.FirstOrDefault(u => u.UserName.ToLower() == loginRequestDTO.UserName.ToLower());
            var isValid = await userManager.CheckPasswordAsync(user, loginRequestDTO.Password);
            if (user == null && isValid ==false)
            {
                return new LoginResponseDTO()
                {
                    Token = "",
                    User = null,
                };
            }
            var roles =await userManager.GetRolesAsync(user);
            JwtSecurityTokenHandler handler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(securityKay);
            var tokenDescriper = new SecurityTokenDescriptor()
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name,user.Id.ToString()),
                    new Claim(ClaimTypes.Role,roles.FirstOrDefault())
                }),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new(new SymmetricSecurityKey(key),SecurityAlgorithms.HmacSha256)
            };
            var token = handler.CreateToken(tokenDescriper);
            LoginResponseDTO loginResponseDTO = new LoginResponseDTO()
            {
                Token = handler.WriteToken(token),
                User = mapper.Map<UserDTO>(user),
                Role = roles.FirstOrDefault(),
            };
            return loginResponseDTO;

        }

        public async Task<UserDTO> Register(RegisterRequestDTO registerRequestDTO)
        {
            ApplicationUser user = new()
            {
                UserName = registerRequestDTO.UserName,
                Name = registerRequestDTO.Name,
                Email = registerRequestDTO.UserName,
                NormalizedEmail = registerRequestDTO.UserName.ToUpper(),
            };
            try
            {
                var result = await userManager.CreateAsync(user, registerRequestDTO.Password);
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(user, "admin");
                    var userToReturn = db.ApplicationUsers
                        .FirstOrDefault(u => u.UserName == registerRequestDTO.UserName);
                    return mapper.Map<UserDTO>(userToReturn);

                }
            }
            catch (Exception e)
            {

            }

            return new UserDTO();
        }
    }
}
