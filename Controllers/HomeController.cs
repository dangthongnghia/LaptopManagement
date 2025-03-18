using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using LaptopManagement;
using LaptopManagement.Models;
using LaptopManagement.Data;

namespace LaptopManagement.Controllers
{
    public class HomeController : Controller
    {
        private readonly LaptopManagementContext _context;

        public HomeController(LaptopManagementContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index(string searchString, string brand, decimal? minPrice, decimal? maxPrice, bool? status)
        {
            var laptops = from l in _context.Laptops
                          select l;

            if (!string.IsNullOrEmpty(searchString))
            {
                laptops = laptops.Where(l => l.Name.Contains(searchString) || l.Brand.Contains(searchString));
            }

            if (!string.IsNullOrEmpty(brand))
            {
                laptops = laptops.Where(l => l.Brand == brand);
            }

            if (minPrice.HasValue)
            {
                laptops = laptops.Where(l => l.Price >= minPrice.Value);
            }

            if (maxPrice.HasValue)
            {
                laptops = laptops.Where(l => l.Price <= maxPrice.Value);
            }

            if (status.HasValue)
            {
                laptops = laptops.Where(l => l.Status == status.Value);
            }

            ViewData["CurrentFilter"] = searchString;
            ViewData["CurrentBrand"] = brand;
            ViewData["CurrentMinPrice"] = minPrice;
            ViewData["CurrentMaxPrice"] = maxPrice;
            ViewData["CurrentStatus"] = status;

            return View(await laptops.Include(l => l.Images).ToListAsync());
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();
            var laptop = await _context.Laptops
                .Include(l => l.Images)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (laptop == null) return NotFound();

            // Ensure there is always a primary image
            if (laptop.Images != null && laptop.Images.Any() && !laptop.Images.Any(img => img.IsPrimary))
            {
                var firstImage = laptop.Images.FirstOrDefault();
                if (firstImage != null)
                {
                    firstImage.IsPrimary = true;
                    _context.Update(firstImage);
                    await _context.SaveChangesAsync();
                }
            }

            return View(laptop);
        }
        public async Task<IActionResult> Privacy()
        {
            return View();
        }

        public async Task<IActionResult> About()
        {
            return View();
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