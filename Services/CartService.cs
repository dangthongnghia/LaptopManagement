using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using LaptopManagement;
using LaptopManagement.Models;
using LaptopManagement.Data;

namespace LaptopManagement.Services
{
    public class CartService
    {
        private readonly LaptopManagementContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly string _cartSessionKey = "CartId";

        public CartService(LaptopManagementContext context, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _httpContextAccessor = httpContextAccessor;
        }

        // Lấy ID giỏ hàng từ session hoặc tạo mới
        public string GetCartId()
        {
            if (_httpContextAccessor.HttpContext.Session.GetString(_cartSessionKey) == null)
            {
                // If the cart ID doesn't exist, create a new random ID and store it in session
                var cartId = Guid.NewGuid().ToString();
                _httpContextAccessor.HttpContext.Session.SetString(_cartSessionKey, cartId);
            }

            return _httpContextAccessor.HttpContext.Session.GetString(_cartSessionKey);
        }

        // Thêm sản phẩm vào giỏ hàng
        public void AddToCart(int laptopId, int quantity)
        {
            var cartId = GetCartId();

            // Find the laptop with price - modified to include images
            var laptop = _context.Laptops
                .Include(l => l.Images)
                .FirstOrDefault(l => l.Id == laptopId);

            if (laptop == null) return;

            // Check if the item is already in cart - modified query
            var cartItem = _context.CartItems
                .Include(c => c.Laptop)
                .FirstOrDefault(c => c.CartId == cartId && c.LaptopId == laptopId);

            if (cartItem == null)
            {
                // Create new cart item
                cartItem = new CartItem
                {
                    LaptopId = laptopId,
                    CartId = cartId,
                    Quantity = quantity,
                    Price = laptop.Price,
                    DateCreated = DateTime.Now
                };
                _context.CartItems.Add(cartItem);
            }
            else
            {
                // Update quantity if already exists
                cartItem.Quantity += quantity;
            }

            _context.SaveChanges();

            // Refresh the session to ensure cart persistence
            _httpContextAccessor.HttpContext.Session.SetString(_cartSessionKey, cartId);
        }

        // Cập nhật số lượng sản phẩm trong giỏ hàng
        public void UpdateQuantity(int id, int quantity)
        {
            var cartItem = _context.Set<CartItem>().Find(id);
            if (cartItem != null)
            {
                if (quantity > 0)
                {
                    cartItem.Quantity = quantity;
                    _context.SaveChanges();
                }
                else
                {
                    RemoveFromCart(id);
                }
            }
        }

        // Xóa sản phẩm khỏi giỏ hàng
        public void RemoveFromCart(int id)
        {
            var cartItem = _context.Set<CartItem>().Find(id);
            if (cartItem != null)
            {
                _context.Set<CartItem>().Remove(cartItem);
                _context.SaveChanges();
            }
        }

        // Lấy tất cả sản phẩm trong giỏ hàng
        public List<CartItem> GetCartItems()
        {
            var cartId = GetCartId();

            // Add logging for debugging
            Console.WriteLine($"Getting cart items for cart ID: {cartId}");

            var items = _context.CartItems
                .Where(c => c.CartId == cartId)
                .Include(c => c.Laptop)
                    .ThenInclude(l => l.Images)
                .AsNoTracking()
                .ToList();

            // Add logging for debugging
            Console.WriteLine($"Found {items.Count} items in cart");

            return items;
        }

        // Tính tổng tiền giỏ hàng
        public decimal GetTotal()
        {
            var cartId = GetCartId();
            return _context.Set<CartItem>()
                .Where(c => c.CartId == cartId)
                .Select(c => c.Laptop.Price * c.Quantity)
                .Sum();
        }

        // Xóa toàn bộ giỏ hàng
        public void ClearCart()
        {
            var cartId = GetCartId();
            var cartItems = _context.Set<CartItem>().Where(c => c.CartId == cartId);
            _context.Set<CartItem>().RemoveRange(cartItems);
            _context.SaveChanges();
        }

        // Đếm số lượng sản phẩm trong giỏ hàng
        public int GetCount()
        {
            var cartId = GetCartId();
            return _context.Set<CartItem>()
                .Where(c => c.CartId == cartId)
                .Sum(c => c.Quantity);
        }
        public bool HasItems()
        {
            var cartId = GetCartId();
            return _context.CartItems
                .Any(c => c.CartId == cartId);
        }
    }
}
