using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using StackExchange.Redis;
using Store.API.Extentions;
using Store.API.Helper;
using Store.API.Middlewear;
using Store.Data.Context;
using Store.Repository.InterFace;
using Store.Repository.Repository;
using Store.Service.HandelResposive;
using Store.Service.Services.product;
using Store.Service.Services.product.Dtos;
using System.Linq;

namespace Store.API
{
	public class Program
	{
		public static async Task Main(string[] args)
		{
			var builder = WebApplication.CreateBuilder(args);

			// Add services to the container.

			builder.Services.AddControllers();
			builder.Services.AddDbContext<StoreDbContext>(Options =>
			{
				Options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
			});

			builder.Services.AddSingleton<IConnectionMultiplexer>(config =>
			{
				var configuration = ConfigurationOptions.Parse(builder.Configuration.GetConnectionString("Redis"));
				return ConnectionMultiplexer.Connect(configuration);
			});
				
				builder.Services.AddApplicationService();


				// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
				builder.Services.AddEndpointsApiExplorer();
				builder.Services.AddSwaggerGen();

				var app = builder.Build();
				await ApplySeeding.ApplySeedingAsync(app);	
				// Configure the HTTP request pipeline.
				if (app.Environment.IsDevelopment())
			{
				app.UseSwagger();
				app.UseSwaggerUI();
			}
			app.UseMiddleware<ExptionMeddleWere>();

			app.UseHttpsRedirection();

			app.UseStaticFiles();

			app.UseAuthorization();

			app.MapControllers();

			app.Run();
		}
	}
}
