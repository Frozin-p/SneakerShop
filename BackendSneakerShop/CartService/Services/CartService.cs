using CartService.Contracts;
using CartService.Abstractions;

namespace CartService.Services
{
    public class CartService : ICartService
    {
        private readonly ICartRepository _cartRepository;

        public CartService(ICartRepository cartRepository)
        {
            _cartRepository = cartRepository;
        }

        public async Task<CartDto?> GetCartByUserIdAsync(Guid userId)
        {
            return await _cartRepository.GetCartByUserIdAsync(userId);
        }

        public async Task AddItemToCartAsync(Guid userId, CartItemDto item)
        {
            await _cartRepository.AddItemToCartAsync(userId, item);
        }

        public async Task RemoveItemFromCartAsync(Guid userId, Guid productId)
        {
            await _cartRepository.RemoveItemFromCartAsync(userId, productId);
        }

        public async Task CreateCartAsync(CartDto cart)
        {
            await _cartRepository.CreateCartAsync(cart);
        }
    }
}
