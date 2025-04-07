using System.Net;

namespace CollegeApplication.Models
{
    public class ApiResponseDto
    {
        public bool Status { get; set; }
        public HttpStatusCode StatusCode { get; set; }
        public dynamic Data { get; set; }
        public List<string> Error { get; set; }
    }
}
