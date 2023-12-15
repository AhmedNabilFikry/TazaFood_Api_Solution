using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Text;
using TazaFood.Core.Services;

namespace TazaFood_Api.Helpers
{
    public class CachedAttribute : Attribute, IAsyncActionFilter
    {
        private readonly int _timeToLiveInSecondes;

        public CachedAttribute(int TimeToLiveInSecondes)
        {
            _timeToLiveInSecondes = TimeToLiveInSecondes;
        }
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {

            var cachedService = context.HttpContext.RequestServices.GetRequiredService<IResponseCachingService>();
            var cacheKey = GenerateCacheKeyFromRequest(context.HttpContext.Request);
            var cachedResponse = await cachedService.GetCachedResponse(cacheKey);

            // If there is a cacheKey, Then Get cachedResponse 
            if (!string.IsNullOrEmpty(cachedResponse))
            {
                var contentResult = new ContentResult()
                { 
                    Content = cachedResponse,
                    ContentType = "Application/json",
                    StatusCode = 200
                };
                context.Result = contentResult;
                return;
            }
            
            //Getting The Value of the Action
            var excutedEndPointContext = await next();
            if (excutedEndPointContext.Result is OkObjectResult okObjectResult)
            {
                await cachedService.CachingResponseAsync(cacheKey, okObjectResult.Value, TimeSpan.FromSeconds(_timeToLiveInSecondes));
            }
        }

        private string GenerateCacheKeyFromRequest(HttpRequest request)
        {
            var keyBuilder = new StringBuilder();
            keyBuilder.Append(request.Path);
            foreach (var (Key, Value) in request.Query)
            {
                keyBuilder.Append($"{Key}-{Value}");
            }
            return keyBuilder.ToString();

        }
    }
}
