using static GatesVilla_Utility.SD;

namespace GatesVilla_Web.Models
{
    public class APIRequest
    {
        public APIType ApiType {  get; set; } 
        public string Url { get; set; }
        public Object Data { get; set; }
        public string Token { get; set; }

    }
}
