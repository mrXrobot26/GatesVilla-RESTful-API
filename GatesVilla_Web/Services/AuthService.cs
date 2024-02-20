using GatesVilla_Utility;
using GatesVilla_Web.Models;
using GatesVilla_Web.Services.IServices;
using GatesVillaAPI.Models.Models.DTOs.LoginAndRegisterDTOs;

namespace GatesVilla_Web.Services
{
    public class AuthService : BaseServices , IAuthService
    {

        private readonly IHttpClientFactory _clientFactory;
        private string villaUrl;

        public AuthService(IHttpClientFactory clientFactory, IConfiguration configuration) : base(clientFactory)
        {
            _clientFactory = clientFactory;
            villaUrl = configuration.GetValue<string>("ServiceUrls:VillaUrl");

        }

        public Task<T> LoginAsync<T>(LoginRequestDTO obj)
        {
            return SendAsync<T>(new APIRequest()
            {
                ApiType = SD.APIType.POST,
                Data = obj,
                Url = villaUrl + "/api/UsersAuth/login"
            });
        }

        public Task<T> RegisterAsync<T>(RegisterRequestDTO obj)
        {
            return SendAsync<T>(new APIRequest()
            {
                ApiType = SD.APIType.POST,
                Data = obj,
                Url = villaUrl + "/api/UsersAuth/register"
            });
        }

    }
}
