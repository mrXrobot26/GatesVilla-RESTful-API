using GatesVillaAPI.Models.Models.DTOs.LoginAndRegisterDTOs;

namespace GatesVilla_Web.Services.IServices
{
    public interface IAuthService
    {
        public Task<T> LoginAsync<T>(LoginRequestDTO login);
        public Task<T> RegisterAsync<T>(RegisterRequestDTO register);




    }
}
