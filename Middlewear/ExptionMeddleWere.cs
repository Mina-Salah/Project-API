using Store.Service.HandelResposive;
using System.Net;
using System.Text.Json;

namespace Store.API.Middlewear
{
	public class ExptionMeddleWere
	{
		private readonly RequestDelegate _next;
		private readonly IHostEnvironment _environment;
		private readonly ILogger<ExptionMeddleWere> _logger;

		public ExptionMeddleWere(
            RequestDelegate next , 
            IHostEnvironment environment,
            ILogger<ExptionMeddleWere> logger
            )
        {
			_next = next;
			_environment = environment;
			_logger = logger;
		}

		public async Task InvokeAsync(HttpContext context)
		{
			try
			{
				await _next(context);
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, ex.Message);
				context.Response.ContentType = "application/json";
				context.Response.StatusCode=(int)HttpStatusCode.InternalServerError;

				var respons = _environment.IsDevelopment()
				? new customExption((int)HttpStatusCode.InternalServerError, ex.Message, ex.StackTrace)
				: new customExption((int)HttpStatusCode.InternalServerError);
				var option = new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };
				var json = JsonSerializer .Serialize(respons, option);	
				await context.Response.WriteAsync(json);
			}
		}
    }
}
