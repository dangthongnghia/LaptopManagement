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
using LaptopManagement.ViewModels;

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
            var cartItems = _cartService.GetCartItems();

            // Debug info
            ViewBag.CartId = _cartService.GetCartId(); // You'll need to make this public
            ViewBag.CartItemCount = cartItems.Count;

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
        public IActionResult Checkout()
        {
            // Get cart items using the cart service
            var cartItems = _cartService.GetCartItems();

            // Log the cart ID for debugging
            Console.WriteLine($"Checkout: Cart ID = {_cartService.GetCartId()}");
            Console.WriteLine($"Checkout: Found {cartItems.Count} items");

            // If cart is empty, redirect to cart index
            if (cartItems == null || !cartItems.Any())
            {
                TempData["ErrorMessage"] = "Giỏ hàng của bạn đang trống.";
                return RedirectToAction("Index");
            }

            // Get user details if authenticated
            string customerName = string.Empty;
            string customerEmail = string.Empty;
            string customerPhone = string.Empty;

            if (User.Identity.IsAuthenticated)
            {
                try
                {
                    var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                    if (!string.IsNullOrEmpty(userId))
                    {
                        var user = _context.Set<User>().Find(int.Parse(userId));
                        if (user != null)
                        {
                            customerName = user.FullName;
                            customerEmail = user.Email;
                            // customerPhone = user.Phone;
                        }
                    }
                }
                catch (Exception ex)
                {
                    // Log exception
                    Console.WriteLine($"Error getting user details: {ex.Message}");
                }
            }

            // Create a CheckoutViewModel with direct cart items
            var viewModel = new CheckoutViewModel
            {
                CustomerName = customerName,
                CustomerEmail = customerEmail,
                CustomerPhone = customerPhone,
                CartItems = cartItems
            };

            return View(viewModel);
        }

        // POST: Cart/PlaceOrder
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> PlaceOrder(CheckoutViewModel model)
        {
            try
            {
                Console.WriteLine("PlaceOrder method started");

                // Remove validation for payment fields that aren't being used
                ModelState.Remove("Address");
                ModelState.Remove("City");
                ModelState.Remove("State");
                ModelState.Remove("ZipCode");
                ModelState.Remove("CardNumber");
                ModelState.Remove("ExpirationDate");
                ModelState.Remove("CVV");

                // Log ModelState errors
                if (!ModelState.IsValid)
                {
                    Console.WriteLine("ModelState is invalid:");
                    foreach (var modelState in ModelState.Values)
                    {
                        foreach (var error in modelState.Errors)
                        {
                            Console.WriteLine($"- {error.ErrorMessage}");
                        }
                    }
                }
                else
                {
                    Console.WriteLine("ModelState is valid, proceeding with order creation");

                    var cartItems = _cartService.GetCartItems();
                    Console.WriteLine($"Retrieved {cartItems.Count} cart items");

                    if (cartItems == null || !cartItems.Any())
                    {
                        TempData["ErrorMessage"] = "Giỏ hàng của bạn đang trống.";
                        return RedirectToAction("Index");
                    }

                    // Tạo đơn hàng mới
                    var order = new Order
                    {
                        CustomerName = model.CustomerName,
                        CustomerEmail = model.CustomerEmail,
                        CustomerPhone = model.CustomerPhone,
                        ShippingAddress = model.ShippingAddress,
                        Notes = model.Notes,
                        OrderDate = DateTime.Now,
                        CreatedDate = DateTime.Now,
                        Status = "Pending"
                    };

                    Console.WriteLine($"Created order object: Name={order.CustomerName}, Email={order.CustomerEmail}");

                    // Set LaptopId to the first item's laptop ID 
                    order.LaptopId = cartItems.FirstOrDefault()?.LaptopId ?? 0;
                    Console.WriteLine($"Set order.LaptopId to {order.LaptopId}");

                    // Set TotalAmount
                    order.TotalAmount = cartItems.Sum(i => i.Quantity * i.Laptop.Price);
                    Console.WriteLine($"Set order.TotalAmount to {order.TotalAmount}");

                    // Lưu đơn hàng
                    Console.WriteLine("Adding order to context");
                    _context.Orders.Add(order);

                    Console.WriteLine("Saving changes to database");
                    var saveResult = await _context.SaveChangesAsync();
                    Console.WriteLine($"SaveChanges result: {saveResult} rows affected");

                    // Tạo chi tiết đơn hàng
                    Console.WriteLine("Creating order details");
                    foreach (var item in cartItems)
                    {
                        var orderDetail = new OrderDetail
                        {
                            OrderId = order.Id,
                            LaptopId = item.LaptopId,
                            Quantity = item.Quantity,
                            UnitPrice = item.Laptop.Price
                        };
                        Console.WriteLine($"Adding order detail: OrderId={orderDetail.OrderId}, LaptopId={orderDetail.LaptopId}, Quantity={orderDetail.Quantity}");
                        _context.OrderDetails.Add(orderDetail);
                    }

                    Console.WriteLine("Saving order details");
                    saveResult = await _context.SaveChangesAsync();
                    Console.WriteLine($"SaveChanges result: {saveResult} rows affected");

                    // Xóa giỏ hàng sau khi đặt hàng thành công
                    Console.WriteLine("Clearing cart");
                    _cartService.ClearCart();

                    TempData["SuccessMessage"] = "Đặt hàng thành công! Mã đơn hàng của bạn là #" + order.Id;
                    return RedirectToAction("OrderConfirmation", new { id = order.Id });
                }
            }
            catch (Exception ex)
            {
                // Log exception details
                Console.WriteLine($"ERROR in PlaceOrder: {ex.Message}");
                Console.WriteLine($"Stack trace: {ex.StackTrace}");

                if (ex.InnerException != null)
                {
                    Console.WriteLine($"Inner exception: {ex.InnerException.Message}");
                }

                TempData["ErrorMessage"] = "Đã xảy ra lỗi khi đặt hàng. Vui lòng thử lại.";
            }

            // If we got this far, something failed; redisplay form with cart items
            model.CartItems = _cartService.GetCartItems();
            return View("Checkout", model);
        }

        // GET: Cart/OrderConfirmation/5
        public async Task<IActionResult> OrderConfirmation(int id)
        {
            // Include OrderDetails and related Laptop data
            var order = await _context.Orders
                .Include(o => o.OrderDetails)
                    .ThenInclude(od => od.Laptop)
                        .ThenInclude(l => l.Images)
                .FirstOrDefaultAsync(o => o.Id == id);

            if (order == null)
            {
                return NotFound();
            }

            return View(order);
        }
        // GET: Cart/MyOrders
        public async Task<IActionResult> MyOrders()
        {
            // Check if user is authenticated
            if (!User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Login", "Account", new { returnUrl = Url.Action("MyOrders", "Cart") });
            }

            try
            {
                // Get current user ID
                var userId = int.Parse(User.FindFirstValue("UserId"));

                // Get user's email to match orders
                var user = await _context.Users.FindAsync(userId);
                if (user == null)
                {
                    return NotFound();
                }

                // Find all orders for this user's email
                var orders = await _context.Orders
                    .Where(o => o.CustomerEmail == user.Email)
                    .OrderByDescending(o => o.OrderDate)
                    .Include(o => o.OrderDetails)
                        .ThenInclude(od => od.Laptop)
                    .ToListAsync();

                return View(orders);
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "Có lỗi xảy ra khi tải đơn hàng của bạn.";
                return RedirectToAction("Index", "Home");
            }
        }
        // POST: Cart/CancelOrder
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CancelOrder(int orderId)
        {
            if (!User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Login", "Account");
            }

            // Get current user's email
            var userId = int.Parse(User.FindFirstValue("UserId"));
            var user = await _context.Users.FindAsync(userId);

            if (user == null)
            {
                return NotFound();
            }

            // Find the order and verify it belongs to the current user
            var order = await _context.Orders.FindAsync(orderId);

            if (order == null)
            {
                TempData["ErrorMessage"] = "Không tìm thấy đơn hàng.";
                return RedirectToAction(nameof(MyOrders));
            }

            // Make sure it's the user's own order
            if (order.CustomerEmail != user.Email)
            {
                TempData["ErrorMessage"] = "Bạn không được phép hủy đơn hàng này.";
                return RedirectToAction(nameof(MyOrders));
            }

            // Can only cancel if status is Pending or Confirmed
            if (order.Status != "Pending" && order.Status != "Confirmed")
            {
                TempData["ErrorMessage"] = "Chỉ có thể hủy đơn hàng ở trạng thái chờ xử lý hoặc đã xác nhận.";
                return RedirectToAction(nameof(MyOrders));
            }

            // Update status to Cancelled
            order.Status = "Cancelled";
            _context.Update(order);

            try
            {
                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = "Đơn hàng đã được hủy thành công.";
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "Có lỗi xảy ra khi hủy đơn hàng.";
            }

            return RedirectToAction(nameof(MyOrders));
        }
    }
}
