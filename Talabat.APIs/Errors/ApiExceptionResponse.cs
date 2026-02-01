

namespace Talabat.APIs.Errors
{
    public class ApiExceptionResponse:ApiResponse
    {   //Server error
        public string Details { get; set; }
        public ApiExceptionResponse(int statusCode,string? message=null,string? details=null):base(statusCode,message)
        {
            Details = details;
        }
    }
}
