using GatesVilla_Web.Models;
using GatesVillaAPI.Models.Models.APIResponde;
using static GatesVilla_Utility.SD;

namespace GatesVilla_Web.Services.IServices
{
    public interface IBaseServices
    {
        public APIResponse apiResponse { get; set; }
        public Task<T> SendAsync<T>(APIRequest apiRequest);

    }
}
