
namespace Talabat.APIs.Errors
{
    public class ApiResponse
    {
        public int StatusCode { get; set; }
        public string Message { get; set; }
        public ApiResponse(int statusCode,string? message=null)
        {
            StatusCode=statusCode;
            Message = message ?? GetDefaultMessageForStatusCode(statusCode);
        }

        private string? GetDefaultMessageForStatusCode(int statusCode)
        {
            return statusCode switch
            {
                400 => "A bad equest, You have made",
                401 => "UnAutherized, You are not",
                404 => "Resource wasn't found",
                500 => "Errors are the path to the dark side, Errors lead to anger, Anger leads to hate, Hate leads to career change",
                _ => null
            };

        }
    }
}