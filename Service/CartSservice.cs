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
                // Nếu người dùng đã đăng nhập, sử dụng ID người dùng
                if (_httpContextAccessor.HttpContext.User.Identity.IsAuthenticated)
                {
                    _httpContextAccessor.HttpContext.Session.SetString(_cartSessionKey, 
                        _httpContextAccessor.HttpContext.User.Identity.Name);
                }
                else
                {
                    // Tạo ID giỏ hàng mới cho khách
                    Guid tempCartId = Guid.NewGuid();
                    _httpContextAccessor.HttpContext.Session.SetString(_cartSessionKey, tempCartId.ToString());
                }
            }

            return _httpContextAccessor.HttpContext.Session.GetString(_cartSessionKey);
        }

        // Thêm sản phẩm vào giỏ hàng
        public void AddToCart(int laptopId, int quantity = 1)
        {
            var cartId = GetCartId();
            
            // Kiểm tra xem sản phẩm đã có trong giỏ hàng chưa
            var cartItem = _context.Set<CartItem>()
                .SingleOrDefault(c => c.CartId == cartId && c.LaptopId == laptopId);

            if (cartItem == null)
            {
                // Tạo mới nếu chưa có
                cartItem = new CartItem
                {
                    LaptopId = laptopId,
                    CartId = cartId,
                    Quantity = quantity,
                    DateCreated = DateTime.Now
                };
                _context.Set<CartItem>().Add(cartItem);
            }
            else
            {
                // Cập nhật số lượng nếu đã có
                cartItem.Quantity += quantity;
            }
            
            _context.SaveChanges();
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
            return _context.Set<CartItem>()
                .Where(c => c.CartId == cartId)
                .Include(c => c.Laptop)
                .ToList();
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
    }
}
