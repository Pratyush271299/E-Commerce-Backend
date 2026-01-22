using System.Net;

namespace E_Commerce.Models
{
    public class ApiResponse
    {
        public bool Success { get; set; }
        public HttpStatusCode StatusCode { get; set; }
        public dynamic Data { get; set; }
        public dynamic Errors { get; set; }
        public string? Token { get; set; }
    }
}
