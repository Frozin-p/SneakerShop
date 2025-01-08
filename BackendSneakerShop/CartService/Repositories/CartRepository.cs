using CartService.Data.Entities;
using CartService.Contracts;
using CartService.Data;
using CartService.Abstractions;
using Microsoft.EntityFrameworkCore;

namespace CartService.Repositories
{
    public class CartRepository : ICartRepository
    {
        private readonly CartDbContext _context;
        private readonly ILogger<CartRepository> _logger;

        public CartRepository(CartDbContext context, ILogger<CartRepository> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<CartDto?> GetCartByUserIdAsync(Guid userId)
        {
            var cart = await _context.Carts
                .Include(c => c.CartItems)
                .FirstOrDefaultAsync(c => c.UserId == userId);

            if (cart == null) return null;

            return new CartDto
            {
                Id = cart.Id,
                UserId = cart.UserId,
                Items = cart.CartItems.Select(ci => new CartItemDto
                {
                    Id = ci.Id,
                    ProductId = ci.ProductId,
                    Quantity = ci.Quantity
                }).ToList()
            };
        }

        public async Task AddItemToCartAsync(Guid userId, CartItemDto item)
        {
            var cart = await _context.Carts
                .Include(c => c.CartItems)
                .FirstOrDefaultAsync(c => c.UserId == userId);

            if (cart == null)
            {
                cart = new Cart { UserId = userId };
                _context.Carts.Add(cart);
                await _context.SaveChangesAsync();
            }

            var cartItem = cart.CartItems.FirstOrDefault(ci => ci.ProductId == item.ProductId);

            if (cartItem != null)
            {
                cartItem.Quantity += item.Quantity;
            }
            else
            {
                cart.CartItems.Add(new CartItem
                {
                    CartId = cart.Id,
                    ProductId = item.ProductId,
                    Quantity = item.Quantity
                });
            }

            await _context.SaveChangesAsync();
        }

        public async Task RemoveItemFromCartAsync(Guid userId, Guid productId)
        {
            var cart = await _context.Carts
                .Include(c => c.CartItems)
                .FirstOrDefaultAsync(c => c.UserId == userId);

            if (cart == null)
                throw new InvalidOperationException("Cart not found.");

            var cartItem = cart.CartItems.FirstOrDefault(ci => ci.ProductId == productId);

            if (cartItem == null)
                throw new InvalidOperationException("Product not found in cart.");

            _context.CartItems.Remove(cartItem);
            await _context.SaveChangesAsync();
        }

        public async Task CreateCartAsync(CartDto cart)
        {
            _logger.LogInformation("Creating cart for user: " + cart.UserId);

            var newCart = new Cart
            {
                UserId = cart.UserId
            };

            _context.Carts.Add(newCart);
            await _context.SaveChangesAsync();

            _logger.LogInformation("Cart successfully created for user: " + cart.UserId);
        }
    }
}
