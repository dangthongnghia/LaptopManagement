using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using LaptopManagement;
using LaptopManagement.Models;

namespace LaptopManagement.Controllers
{
    public class HomeController : Controller
    {
        private readonly LaptopManagementContext _context;

        public HomeController(LaptopManagementContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index(string searchString)
        {
            var laptops = from l in _context.Laptops
                          select l;

            if (!string.IsNullOrEmpty(searchString))
            {
                laptops = laptops.Where(l => l.Name.Contains(searchString) || l.Brand.Contains(searchString));
            }

            ViewData["CurrentFilter"] = searchString;
            return View(await laptops.Include(l => l.Images).ToListAsync());
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var laptop = await _context.Laptops
    .Include(l => l.Images)
    .FirstOrDefaultAsync(m => m.Id == id);// Hoặc Include(l => l.Images) nếu dùng nhiều ảnh

            if (laptop == null) return NotFound();

            return View(laptop);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Order(int LaptopId, string CustomerName, string CustomerEmail, string CustomerPhone)
        {
            var laptop = await _context.Laptops.FindAsync(LaptopId);
            if (laptop == null || !laptop.Status)
            {
                TempData["ErrorMessage"] = "Sản phẩm không tồn tại hoặc đã hết hàng.";
                return RedirectToAction("Details", new { id = LaptopId });
            }

            if (string.IsNullOrWhiteSpace(CustomerName) || string.IsNullOrWhiteSpace(CustomerEmail) || string.IsNullOrWhiteSpace(CustomerPhone))
            {
                TempData["ErrorMessage"] = "Vui lòng điền đầy đủ thông tin.";
                return RedirectToAction("Details", new { id = LaptopId });
            }

            var order = new Order
            {
                LaptopId = LaptopId,
                CustomerName = CustomerName,
                CustomerEmail = CustomerEmail,
                CustomerPhone = CustomerPhone,
                OrderDate = DateTime.Now,
                Status = "Pending"
            };

            _context.Orders.Add(order);
            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] = "Đơn hàng của bạn đã được đặt thành công! Chúng tôi sẽ liên hệ sớm.";
            return RedirectToAction("Details", new { id = LaptopId });
        }
    }
}