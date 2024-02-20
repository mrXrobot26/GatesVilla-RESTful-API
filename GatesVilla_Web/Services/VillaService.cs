using GatesVilla_Utility;
using GatesVilla_Web.Models;
using GatesVilla_Web.Services.IServices;
using GatesVillaAPI.Models.Models.APIResponde;
using GatesVillaAPI.Models.Models.DTOs.VillaDTOs;

namespace GatesVilla_Web.Services
{
	public class VillaService : BaseServices, IVillaService
	{
		private readonly IHttpClientFactory _clientFactory;
		private string villaUrl;

		public VillaService(IHttpClientFactory clientFactory, IConfiguration configuration) : base(clientFactory)
		{
			_clientFactory = clientFactory;
			villaUrl = configuration.GetValue<string>("ServiceUrls:VillaUrl");

		}

		public Task<T> CreateAsync<T>(VillaCreateDTO dto, string token)
		{
			return SendAsync<T>(new APIRequest()
			{
				ApiType = SD.APIType.POST,
				Data = dto,
				Url = villaUrl + "/api/villaAPI/AddVilla",
				Token = token
			});
		}

		public Task<T> DeleteAsync<T>(int id,string token)
		{
			return SendAsync<T>(new APIRequest()
			{
				ApiType = SD.APIType.DELETE,
				Url = villaUrl + "/api/villaAPI/" + id,
				Token = token
			});
		}

		public Task<T> GetAllAsync<T>(string token)
		{
			return SendAsync<T>(new APIRequest()
			{
				ApiType = SD.APIType.GET,
				Url = villaUrl + "/api/villaAPI/GetVillas",
				Token = token
			});
		}

		public Task<T> GetAsync<T>(int id, string token)
		{
			return SendAsync<T>(new APIRequest()
			{
				ApiType = SD.APIType.GET,
				Url = villaUrl + "/api/villaAPI/" + id,
				Token = token
			});
		}

		public Task<T> UpdateAsync<T>(VillaUpdateDTO dto, string token)
		{
			return SendAsync<T>(new APIRequest()
			{
				ApiType = SD.APIType.PUT,
				Data = dto,
				Url = villaUrl + "/api/villaAPI/UpdateVilla/" + dto.Id,
				Token = token
			});
		}
	}
}