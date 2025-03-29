/// Sets the creation timestamp of the order to the current date and time
using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using LaptopManagement;
using LaptopManagement.Models;
using LaptopManagement.Services;
using LaptopManagement.Data;
using System.Security.Claims;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace LaptopManagement.Controllers
{
    public class CartController : Controller
    {
        private readonly LaptopManagementContext _context;
        private readonly CartService _cartService;

        public CartController(LaptopManagementContext context, CartService cartService)
        {
            _context = context;
            _cartService = cartService;
        }

        // GET: Cart
        public IActionResult Index()
        {
            var cartItems = _cartService.GetCartItems(); // List<CartItem>
            return View(cartItems);
        }

        // POST: Cart/AddToCart/5
        [HttpPost]
        public IActionResult AddToCart(int laptopId, int quantity = 1)
        {
            // Kiểm tra sản phẩm tồn tại
            var laptop = _context.Laptops.Find(laptopId);
            if (laptop == null)
            {
                TempData["ErrorMessage"] = "Sản phẩm không tồn tại.";
                return RedirectToAction("Index", "Laptops");
            }

            _cartService.AddToCart(laptopId, quantity);
            TempData["SuccessMessage"] = $"Đã thêm {laptop.Name} vào giỏ hàng.";
            
            return RedirectToAction("Index");
        }

        // POST: Cart/UpdateQuantity/5
        [HttpPost]
        public IActionResult UpdateQuantity(int id, int quantity)
        {
            _cartService.UpdateQuantity(id, quantity);
            return RedirectToAction("Index");
        }

        // POST: Cart/RemoveFromCart/5
        [HttpPost]
        public IActionResult RemoveFromCart(int id)
        {
            _cartService.RemoveFromCart(id);
            TempData["SuccessMessage"] = "Đã xóa sản phẩm khỏi giỏ hàng.";
            return RedirectToAction("Index");
        }

        // Phương thức để lấy giỏ hàng theo userId
        private async Task<Cart> GetCartByUserIdAsync(int userId)
        {
            // Tìm giỏ hàng hiện tại của người dùng
            var cart = await _context.Carts
                .Include(c => c.Items)
                    .ThenInclude(i => i.Laptop)
                        .ThenInclude(l => l.Images)
                .FirstOrDefaultAsync(c => c.UserId == userId);

            // Nếu người dùng chưa có giỏ hàng, tạo mới
            if (cart == null)
            {
                cart = new Cart
                {
                    UserId = userId,
                    CreatedAt = DateTime.Now,
                    Items = new List<CartItem>()
                };
                _context.Carts.Add(cart);
                await _context.SaveChangesAsync();
            }

            return cart;
        }

        // GET: Cart/Checkout
        public async Task<IActionResult> Checkout()
        {
            var userId = int.Parse(User.FindFirstValue("UserId"));
            var cart = await GetCartByUserIdAsync(userId);
            
            if (cart == null || !cart.Items.Any())
            {
                TempData["ErrorMessage"] = "Giỏ hàng của bạn đang trống.";
                return RedirectToAction("Index");
            }

            var user = await _context.Set<User>().FindAsync(userId);
            
            var order = new Order
            {
                CustomerName = user.FullName,
                CustomerEmail = user.Email,
                CustomerPhone = "", // Bạn có thể thêm trường phone vào User model nếu cần
                OrderDate = DateTime.Now,
                Status = "Pending"
            };

            ViewBag.Cart = cart;
            
            return View(order);
        }

        // POST: Cart/Checkout
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Checkout([Bind("CustomerName,CustomerEmail,CustomerPhone,Address,Note")] Order order)
        {
            if (ModelState.IsValid)
            {
                var cartItems = _cartService.GetCartItems();
                if (cartItems.Count == 0)
                {
                    TempData["ErrorMessage"] = "Giỏ hàng của bạn đang trống.";
                    return RedirectToAction("Index");
                }

                // Tạo đơn hàng mới
                order.OrderDate = DateTime.Now;
                order.Status = "Pending";
                order.TotalAmount = _cartService.GetTotal();
                
                // Lưu đơn hàng
                _context.Add(order);
                await _context.SaveChangesAsync();

                // Tạo chi tiết đơn hàng
                foreach (var item in cartItems)
                {
                    var orderDetail = new OrderDetail
                    {
                        OrderId = order.Id,
                        LaptopId = item.LaptopId,
                        Quantity = item.Quantity,
                        UnitPrice = item.Laptop.Price
                    };
                    _context.Add(orderDetail);
                }
                
                await _context.SaveChangesAsync();
                
                // Xóa giỏ hàng sau khi đặt hàng thành công
                _cartService.ClearCart();
                
                TempData["SuccessMessage"] = "Đặt hàng thành công! Mã đơn hàng của bạn là #" + order.Id;
                return RedirectToAction("OrderConfirmation", new { id = order.Id });
            }
            
            ViewBag.Total = _cartService.GetTotal();
            return View(order);
        }

        // GET: Cart/OrderConfirmation/5
        public async Task<IActionResult> OrderConfirmation(int id)
        {
            var order = await _context.Set<Order>().FindAsync(id);
            
            if (order == null)
            {
                return NotFound();
            }

            return View(order);
        }
    }
}
