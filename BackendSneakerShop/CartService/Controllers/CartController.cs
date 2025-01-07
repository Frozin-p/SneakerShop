using CartService.Contracts;
using CartService.Abstractions;
using Microsoft.AspNetCore.Mvc;

namespace CartService.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CartController : ControllerBase
    {
        private readonly ICartService _cartService;

        public CartController(ICartService cartService)
        {
            _cartService = cartService;
        }

        [HttpGet("{userId}")]
        public async Task<ActionResult<CartDto>> GetCartByUserId(Guid userId)
        {
            var cart = await _cartService.GetCartByUserIdAsync(userId);
            if (cart == null)
                return NotFound();

            return Ok(cart);
        }

        [HttpPost("{userId}")]
        public async Task<ActionResult> AddItemToCart(Guid userId, [FromBody] CartItemDto item)
        {
            await _cartService.AddItemToCartAsync(userId, item);
            return NoContent();
        }

        [HttpDelete("{userId}/items/{productId}")]
        public async Task<ActionResult> RemoveItemFromCart(Guid userId, Guid productId)
        {
            try
            {
                await _cartService.RemoveItemFromCartAsync(userId, productId);
                return NoContent();
            }
            catch (InvalidOperationException ex)
            {
                return NotFound(new { Message = ex.Message });
            }
        }
    }
}
