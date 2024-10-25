using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Store.API.Helper;
using Store.Repository.Specification.productSpeci;
using Store.Service.HandelResposive;
using Store.Service.Helper;
using Store.Service.Services.product;
using Store.Service.Services.product.Dtos;

namespace Store.API.Controllers
{

	public class ProductController : BaseController
	{
		private readonly IProductServices _productServices;

		public ProductController(IProductServices productServices)
		{
			_productServices = productServices;
		}
		[HttpGet]
		[ResponseCache]
		public async Task<ActionResult<IReadOnlyList<BrandTypeDetailsDto>>> getAllBrands()
			=> Ok(await _productServices.GetAllBrandsAsync());


		[HttpGet]
		public async Task<ActionResult<IReadOnlyList<BrandTypeDetailsDto>>> getAlltypes()
		=> Ok(await _productServices.GetAllTypesAsync());

		[HttpGet]
		public async Task<ActionResult<PaginatedResultDto<ProblemDetails>>>GetAllProduct([FromQuery]productspecification input)
			=> Ok (await _productServices.GetAllProductAsync(input));

		[HttpGet]
		public async Task<ActionResult<productDatailsDto>> GetProduct(int? id)
		{
			if (id is null)
				return BadRequest(new Respons(400,"id is null"));
			var product = await _productServices.GetProductIdAsync(id);
			if (product is null)
			return NotFound(new Respons(404));

			return Ok(product);
		}
			

	}
}
