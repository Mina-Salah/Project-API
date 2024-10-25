using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Store.Service.Services.BascetSevice;
using Store.Service.Services.BascetSevice.DTOS;

namespace Store.API.Controllers
{

	public class BasketController : BaseController
	{
		private readonly IBasketService _basketService;

		public BasketController(IBasketService basketService) 
		{
			_basketService = basketService;
		}
		[HttpGet]
		public async Task<ActionResult<CustomerBasketDTO>> GetBasketBuId(string id)
			=> await _basketService.GetBasketAsynk(id);


		[HttpPost]
		public async Task<ActionResult<CustomerBasketDTO>> UpdateBasketBuId(CustomerBasketDTO basket)
			=> Ok(await _basketService.UpdateBasketAsynk(basket));

		[HttpDelete]
		public async Task<ActionResult> DeleteBasketAsynk(string id)
			=> Ok(await _basketService.DeleteBasketAsynk(id));

	}
}
