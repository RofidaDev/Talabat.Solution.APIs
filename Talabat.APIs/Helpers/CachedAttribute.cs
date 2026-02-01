using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Text;
using Talabat.Core.Services.Contract;

namespace Talabat.APIs.Helpers
{
    public class CachedAttribute : Attribute, IAsyncActionFilter
    {
        private readonly int _timeToLiveInSeconds;

        public CachedAttribute(int timeToLiveInSeconds)
        {
            _timeToLiveInSeconds = timeToLiveInSeconds;
        }
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        //excute before action, after model binding
        {
            var responseCacheService = context.HttpContext.RequestServices/*DependencyInjectionContainer*/.GetRequiredService<IResponseCacheService>();
            //Ask CLR For Creating Object from "ResponseCacheService" Explicitly
            var cacheKey = GenerateCacheKeyFromRequest(context.HttpContext.Request);
            var reponse = await responseCacheService.GetCachedResponseAsync(cacheKey);
            if (!string.IsNullOrEmpty(reponse)) {
                var result = new ContentResult()
                {
                    Content = reponse,
                    ContentType = "application/json",
                    StatusCode = 200
                };
                context.Result = result;
                return;
            }
            //Response is not cached:
            var excutedActionContext = await next.Invoke(); //will excute next action filter or action itself(endpoint)
            if (excutedActionContext.Result is OkObjectResult okObjectResult && okObjectResult.Value is not null)
            {
                await responseCacheService.CacheResponseAsync(cacheKey, okObjectResult.Value/*Response itself*/, TimeSpan.FromSeconds(_timeToLiveInSeconds));
            }
        }

        private string GenerateCacheKeyFromRequest(HttpRequest request)
        { //Generate cacheKey from Request

            //{{Url}}/api/Proucts?bageIndex=1&pageSize=5&sort=name
            var keyBuilder =new StringBuilder();
            keyBuilder.Append(request.Path); //  /api/Products
            foreach (var (key, value) in request.Query.OrderBy(x=>x.Key)){
                //key=value
                //pageIndex=1
                //pageSize=5
                //sort=name
                keyBuilder.Append($"|{key}-{value}");
                // /api/Products/pageIndex-1
                // /api/Products/pageIndex-1|pageSize-5
                // /api/Products/pageIndex-1|pageSize-5|sort-name
            }
            return keyBuilder.ToString();
        }
    } }
