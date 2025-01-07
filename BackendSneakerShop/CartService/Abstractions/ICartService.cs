using CartService.Contracts;

namespace CartService.Abstractions
{
    public interface ICartService
    {
        Task<CartDto?> GetCartByUserIdAsync(Guid userId);
        Task AddItemToCartAsync(Guid userId, CartItemDto item);
        Task RemoveItemFromCartAsync(Guid userId, Guid productId);
    }
}
