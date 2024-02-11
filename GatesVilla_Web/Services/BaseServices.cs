using GatesVilla_Utility;
using GatesVilla_Web.Models;
using GatesVilla_Web.Services.IServices;
using GatesVillaAPI.Models.Models.APIResponde;
using Newtonsoft.Json;
using System.Text;

namespace GatesVilla_Web.Services
{
    public class BaseServices : IBaseServices
    {
        public APIResponse apiResponse { get; set; }
        IHttpClientFactory httpClient { get; set; }
        public BaseServices(IHttpClientFactory httpClient)
        {
            this.httpClient = httpClient;
            apiResponse = new();
        }

        public async Task<T> SendAsync<T>(APIRequest apiRequest)
        {
            try
            {
                var client = httpClient.CreateClient("VillaApi");
                HttpRequestMessage msg = new HttpRequestMessage();
                msg.Headers.Add("Content-Type", "application/json");
                msg.RequestUri = new Uri(apiRequest.Url);
                if (apiRequest.Data != null)
                {
                    msg.Content = new StringContent(JsonConvert.SerializeObject(apiRequest.Data), Encoding.UTF8, "application/json");
                }
                switch (apiRequest.ApiType)
                {
                    case SD.APIType.POST:
                        msg.Method = HttpMethod.Post;
                        break;
                    case SD.APIType.PUT:
                        msg.Method = HttpMethod.Put;
                        break;
                    case SD.APIType.DELETE:
                        msg.Method = HttpMethod.Delete;
                        break;
                    default:
                        msg.Method = HttpMethod.Get;
                        break;
                }

                HttpResponseMessage responseMessage = null;

                responseMessage = await client.SendAsync(msg);

                var apiContant = await responseMessage.Content.ReadAsStringAsync();
                var APIResponse = JsonConvert.DeserializeObject<T>(apiContant);
                return APIResponse;


            }
            catch (Exception ex)
            {
                var dto = new APIResponse
                {
                    ErrorMessages = new List<string> { ex.Message },
                    IsSuccess = false
                };
                var res = JsonConvert.SerializeObject(dto);
                var APIResponse = JsonConvert.DeserializeObject<T>(res);
                return APIResponse;
            }
        
        }
    }
}
