using Microsoft.AspNetCore.Mvc;
using Store.Repository.InterFace;
using Store.Repository.Repository;
using Store.Service.Services.product.Dtos;
using Store.Service.Services.product;
using Store.Service.HandelResposive;
using Store.Service.Services.CashServer;
using Store.Repository.BascetRepository;
using Store.Service.Services.BascetSevice.DTOS;
using Store.Service.Services.BascetSevice;

namespace Store.API.Extentions
{
	public static class ApplicationServiceExtention
	{
		public static IServiceCollection AddApplicationService(this IServiceCollection services)
		{
			services.AddScoped<IUnitOfWork, UnitOfWork>();
			services.AddScoped<IProductServices, productservices>();
			services.AddScoped<IBasketService, BasketService>();
			services.AddScoped<ICashServer, CashServer>();
			services.AddScoped<IBasketRepository, BasketRepository>();
			services.AddAutoMapper(typeof(ProductProfile));
			services.AddAutoMapper(typeof(BasketProfile));

			services.Configure<ApiBehaviorOptions>(Options =>
			{
				Options.InvalidModelStateResponseFactory = actionContext =>
				{
					var errors = actionContext.ModelState
								.Where(modle => modle.Value.Errors.Count > 0)
								.SelectMany(modle => modle.Value.Errors)
								.Select(error => error.ErrorMessage)
								.ToList();
					var errorResponse = new validationErrorCustom
					{
						Errors = errors
					};
					return new BadRequestObjectResult(errorResponse);

				};
			});
			return services;
		}
	}
}
