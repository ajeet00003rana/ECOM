using System.Net;

namespace Project.Models.Api
{
    public class Response
    {
        public HttpStatusCode Status { get; set; }
        public string? Message { get; set; }
    }
}
