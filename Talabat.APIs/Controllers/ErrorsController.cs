
using Microsoft.AspNetCore.Mvc;
using Talabat.APIs.Errors;

namespace Talabat.APIs.Controllers
{   //errors/404
    [Route("errors/{code}")]  //code==statusCode=>404
    [ApiController]
    [ApiExplorerSettings(IgnoreApi = true)]  //to tell swagger:Don't document this controller(endpoints)
    public class ErrorsController : ControllerBase
    {
        public ActionResult Error(int code)
        {
            return NotFound(new ApiResponse(code));
        }
    }
}
