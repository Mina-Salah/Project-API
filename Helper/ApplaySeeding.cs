using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Store.Data.Context;
using Store.Repository;
using System;
using System.Threading.Tasks;

namespace Store.API.Helper
{
	public class ApplySeeding
	{
		public static async Task ApplySeedingAsync(WebApplication app)
		{
			using (var scope = app.Services.CreateScope())
			{
				var service = scope.ServiceProvider;
				var loggerFactory = service.GetRequiredService<ILoggerFactory>();
				try
				{
					var context = service.GetRequiredService<StoreDbContext>();
					await context.Database.MigrateAsync();
					await StoreContextSeed.seedAsync(context, loggerFactory);
				}
				catch (Exception ex)
				{
					var logger = loggerFactory.CreateLogger<ApplySeeding>();
					logger.LogError(ex.Message);
				}
			}
		}
	}
}
