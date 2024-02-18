using GatesVillaAPI.DataAcess.Data;
using GatesVillaAPI.DataAcess.Repo.IRepo;
using GatesVillaAPI.Models.Models.DTOs.LoginAndRegisterDTOs;
using GatesVillaAPI.Models.Models.MyModels;
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
        private readonly IConfiguration configuration;
        private string securityKay;
        public UserRepository(ApplicationDbContext db, IConfiguration configuration)
        {
            this.db = db;
            this.configuration = configuration;
            securityKay = configuration.GetValue<string>("ApiSettings:Secret");
        }
        public async Task<bool> IsUniqe(string username)
        {
            var matchUsername = db.localUsers.FirstOrDefault(x => x.UserName == username);
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
            var user = db.localUsers.FirstOrDefault(x => x.UserName == loginRequestDTO.UserName && x.Password == loginRequestDTO.Password);
            if (user == null)
            {
                return new LoginResponseDTO()
                {
                    Token = "",
                    User = null,
                };
            }

            JwtSecurityTokenHandler handler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(securityKay);
            var tokenDescriper = new SecurityTokenDescriptor()
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name,user.Id.ToString()),
                    new Claim(ClaimTypes.Role,user.Role)
                }),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new(new SymmetricSecurityKey(key),SecurityAlgorithms.HmacSha256)
            };
            var token = handler.CreateToken(tokenDescriper);
            LoginResponseDTO loginResponseDTO = new LoginResponseDTO()
            {
                Token = handler.WriteToken(token),
                User = user,
            };
            return loginResponseDTO;

        }

        public async Task<LocalUser> Register(RegisterRequestDTO registerRequestDTO)
        {
            LocalUser user = new()
            {
                UserName = registerRequestDTO.UserName,
                Password = registerRequestDTO.Password,
                Name = registerRequestDTO.Name,
                Role = registerRequestDTO.Role,
            };
            db.localUsers.Add(user);
            await db.SaveChangesAsync();
            user.Password = "";
            return user;
        }
    }
}
