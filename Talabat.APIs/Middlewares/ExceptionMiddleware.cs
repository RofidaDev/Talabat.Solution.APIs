using System.Net;
using System.Text.Json;
using Talabat.APIs.Errors;

namespace Talabat.APIs.Middlewares
{
    // By Convension
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate next;
        private readonly ILogger<ExceptionMiddleware> logger;
        private readonly IHostEnvironment env;

        public ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger, IHostEnvironment env)
        {
            this.next = next;
            this.logger = logger;
            this.env = env;
        }
        public async Task InvokeAsync(HttpContext httpContext /*the request*/) {
            try
            {
                //logger.LogInformation("Before Request");  //log & print
                await next.Invoke(httpContext);  //go to next middleware     //in case:Development 
                //logger.LogInformation("After Request");
            }
            catch (Exception ex) {
                logger.LogError(ex,ex.Message);
                httpContext.Response.ContentType="application/json";  //header of response
                httpContext.Response.StatusCode=(int)HttpStatusCode.InternalServerError;   //header
                var responce =env.IsDevelopment()?
                    new ApiExceptionResponse((int)HttpStatusCode.InternalServerError, ex.Message,ex.StackTrace/*details*/.ToString())
                    :new ApiExceptionResponse((int)HttpStatusCode.InternalServerError,ex.Message,ex.StackTrace.ToString());
                var options = new JsonSerializerOptions()
                {
                    PropertyNamingPolicy = JsonNamingPolicy.CamelCase
                };
                var json = JsonSerializer.Serialize(responce,options);
               
                await httpContext.Response.WriteAsync(json);  //write json string in response (body)

                //Log Exception in (Database |Files)  //Production
        }
        }
    } }
