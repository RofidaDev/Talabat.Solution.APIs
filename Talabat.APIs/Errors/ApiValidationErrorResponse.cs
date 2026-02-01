namespace Talabat.APIs.Errors
{
    public class ApiValidationErrorResponse:ApiResponse  //through change factory that generate responce for validation error in(AppServiceExtension)
    {  //bad request
        public IEnumerable<string> Errors { get; set; }
        public ApiValidationErrorResponse():base(400)
        {
            //message="A bad equest, You have made"
            Errors = new List<string>();
        }
    }
}
