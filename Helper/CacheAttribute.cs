using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Store.Service.Services.CashServer;
using System.Text;

namespace Store.API.Helper
{
	public class CacheAttribute : Attribute, IAsyncActionFilter
	{
		private readonly int _timeToLiveInSeconds;

		public CacheAttribute(int timeToLiveInSeconds)
		{
			_timeToLiveInSeconds = timeToLiveInSeconds;
		}

		public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
		{
			var _cacheService = context.HttpContext.RequestServices.GetRequiredService<ICashServer>();
			var cachkey = GenerateCacheKeyRequest(context.HttpContext.Request);
			var cacheresponce = await _cacheService.getCasheResponseAsync(cachkey);

			if (!string.IsNullOrEmpty(cacheresponce))
			{
				var contentresulte = new ContentResult
				{
					Content = cacheresponce,
					ContentType = "application/json",
					StatusCode = 200
				};
				context.Result = contentresulte;
				return;
			}

			var excutedcontext = await next();
			if (excutedcontext.Result is OkObjectResult responce)
				await _cacheService.setCasheResponseasync(cachkey,responce.Value,TimeSpan.FromSeconds(_timeToLiveInSeconds));	

		}

		private string GenerateCacheKeyRequest(HttpRequest request) 
		{ 
			StringBuilder cacheKE = new StringBuilder();
			cacheKE.Append($"{request.Path}");
			foreach (var (Key, Value) in request.Query.OrderBy(x => x.Key))
				cacheKE.Append($"|{Key}-{Value}");

			return cacheKE.ToString();	
		}
	}
}
