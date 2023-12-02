using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Reflection;
using System.Security.Claims;
using TazaFood.Core.IRepository;
using TazaFood.Core.Models;

namespace TazaFood_Api.Controllers
{

    public class BasketController : BaseApiController
    {
        private readonly IBasketRepository _basketRepo;

        public BasketController(IBasketRepository basket)
        {
            _basketRepo = basket;
        }
        
        [HttpGet("GetBasket")]
        public async Task<ActionResult<UserBasket>> GetBasket(string ID)
        {
            var BasketID = User.FindFirstValue(ClaimTypes.Email.Split("@")[0]);
            if (string.IsNullOrEmpty(BasketID))
            {
                BasketID = "DefaultUserID";
            }
            var basket = await _basketRepo.GetUserBasketAsync(BasketID);
            return basket ?? new UserBasket(ID);
        }
        [HttpPost("UpdateOrCreateBasket")]
        public async Task<ActionResult<UserBasket>> Updatebasket(UserBasket basket)
        {
            var CreatedOrUpdatedBasket = await _basketRepo.UpdateBasketAsync(basket);
            if (CreatedOrUpdatedBasket is null) return BadRequest("there is error occuerd when add or update cart");
            return CreatedOrUpdatedBasket;
        }
        [HttpDelete]
        public async Task<ActionResult<bool>> DeleteBasket(string ID)
        {
            return await _basketRepo.DeleteBasketAsync( ID);
        }
    }
}
