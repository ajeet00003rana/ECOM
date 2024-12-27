using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Project.DataAccess.Services;
using Project.Models.EntityModels;

namespace Project.Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ShoppingCartController : ControllerBase
    {
        private readonly IShoppingCartService _shoppingCartService;

        public ShoppingCartController(IShoppingCartService shoppingCartService)
        {
            _shoppingCartService = shoppingCartService;
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> AddToCart([FromBody] ShoppingCart cart)
        {
            var added = await _shoppingCartService.AddToCartAsync(cart);
            return added ? Ok() : BadRequest("Failed to add item to cart.");
        }

        [HttpGet("{userId}")]
        [Authorize]
        public async Task<IActionResult> GetCartItems(int userId)
        {
            var items = await _shoppingCartService.GetCartItemsAsync(userId);
            return Ok(items);
        }

        [HttpDelete("{userId}/{productId}")]
        [Authorize]
        public async Task<IActionResult> RemoveFromCart(int userId, int productId)
        {
            var removed = await _shoppingCartService.RemoveFromCartAsync(userId, productId);
            return removed ? Ok() : NotFound("Cart item not found.");
        }

        [HttpDelete("{userId}")]
        [Authorize]
        public async Task<IActionResult> ClearCart(int userId)
        {
            var cleared = await _shoppingCartService.ClearCartAsync(userId);
            return cleared ? Ok() : BadRequest("Failed to clear cart.");
        }
    }

}
