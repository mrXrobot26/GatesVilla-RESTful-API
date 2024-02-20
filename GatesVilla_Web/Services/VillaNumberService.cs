using GatesVilla_Utility;
using GatesVilla_Web.Models;
using GatesVilla_Web.Services.IServices;
using GatesVillaAPI.Models.Models.APIResponde;
using GatesVillaAPI.Models.Models.DTOs.VillaDTOs;

namespace GatesVilla_Web.Services
{
    public class VillaNumberService : BaseServices, IVillaNumberService
    {
        private readonly IHttpClientFactory _clientFactory;
        private string villaUrl;

		public VillaNumberService(IHttpClientFactory clientFactory, IConfiguration configuration) : base(clientFactory)
        {
            _clientFactory = clientFactory;
            villaUrl = configuration.GetValue<string>("ServiceUrls:VillaUrl");

        }

        public Task<T> CreateAsync<T>(VillaNumberCreateDTO dto, string token)
        {
            return SendAsync<T>(new APIRequest()
            {
                ApiType = SD.APIType.POST,
                Data = dto,
                Url = villaUrl + "/api/VillaNumberAPI/AddVillaNumber",
                Token = token
            })  ;
        }

        public Task<T> DeleteAsync<T>(int id, string token)
        {
            return SendAsync<T>(new APIRequest()
            {
                ApiType = SD.APIType.DELETE,
                Url = villaUrl + "/api/VillaNumberAPI/" + id,
                Token = token


            });
        }

        public Task<T> GetAllAsync<T>(string token)
        {
            return SendAsync<T>(new APIRequest()
            {
                ApiType = SD.APIType.GET,
                Url = villaUrl + "/api/VillaNumberAPI/GetAllVillaNumber",
                Token = token

            });
        }

        public Task<T> GetAsync<T>(int id, string token)
        {
            return SendAsync<T>(new APIRequest()
            {
                ApiType = SD.APIType.GET,
                Url = villaUrl + "/api/VillaNumberAPI/" + id,
                Token = token

            });
        }

        public Task<T> UpdateAsync<T>(VillaNumberUpdateDTO dto, string token)
        {
            return SendAsync<T>(new APIRequest()
            {
                ApiType = SD.APIType.PUT,
                Data = dto,
                Url = villaUrl + "/api/VillaNumberAPI/" + dto.VillaNum,
                Token = token


            });
        }
    }
}
