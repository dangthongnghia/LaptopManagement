using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using LaptopManagement;
using LaptopManagement.Models;
using System.Linq;
using System.Threading.Tasks;
using LaptopManagement.Data;

namespace LaptopManagement.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        private readonly LaptopManagementContext _context;

        public AdminController(LaptopManagementContext context)
        {
            _context = context;
        }

        // GET: Admin/Dashboard
        public async Task<IActionResult> Dashboard()
        {
            // Đếm số lượng các đối tượng trong hệ thống
            ViewBag.LaptopCount = await _context.Laptops.CountAsync();
            ViewBag.CategoryCount = await _context.Set<Category>().CountAsync();
            ViewBag.OrderCount = await _context.Set<Order>().CountAsync();
            ViewBag.UserCount = await _context.Set<User>().CountAsync();
            
            // Lấy 5 đơn hàng mới nhất
            ViewBag.RecentOrders = await _context.Set<Order>()
                .OrderByDescending(o => o.OrderDate)
                .Take(5)
                .ToListAsync();
            
            // Lấy 5 laptop mới nhất
            ViewBag.RecentLaptops = await _context.Laptops
                .OrderByDescending(l => l.Id)
                .Take(5)
                .ToListAsync();
                
            return View();
        }
        
        // GET: Admin/Users
        public async Task<IActionResult> Users()
        {
            var users = await _context.Set<User>().ToListAsync();
            return View(users);
        }
        
        // GET: Admin/UserDetails/5
        public async Task<IActionResult> UserDetails(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = await _context.Set<User>()
                .FirstOrDefaultAsync(m => m.Id == id);
                
            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }
        
        // GET: Admin/EditUser/5
        public async Task<IActionResult> EditUser(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = await _context.Set<User>().FindAsync(id);
            if (user == null)
            {
                return NotFound();
            }
            return View(user);
        }

        // POST: Admin/EditUser/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditUser(int id, [Bind("Id,UserName,Email,FullName,Role,IsActive")] User user)
        {
            if (id != user.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var existingUser = await _context.Set<User>().FindAsync(id);
                    
                    // Cập nhật chỉ những trường được phép
                    existingUser.FullName = user.FullName;
                    existingUser.Role = user.Role;
                    existingUser.IsActive = user.IsActive;
                    
                    _context.Update(existingUser);
                    await _context.SaveChangesAsync();
                    
                    TempData["SuccessMessage"] = "Cập nhật người dùng thành công!";
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UserExists(user.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Users));
            }
            return View(user);
        }
        
        // GET: Admin/Orders
        public async Task<IActionResult> Orders()
        {
            var orders = await _context.Set<Order>()
                .OrderByDescending(o => o.OrderDate)
                .ToListAsync();
                
            return View(orders);
        }
        
        // GET: Admin/OrderDetails/5
        public async Task<IActionResult> OrderDetails(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var order = await _context.Set<Order>()
                .FirstOrDefaultAsync(m => m.Id == id);
                
            if (order == null)
            {
                return NotFound();
            }

            return View(order);
        }
        
        // GET: Admin/Statistics
        public async Task<IActionResult> Statistics()
        {
            // Thống kê đơn hàng theo tháng
            var currentYear = DateTime.Now.Year;
            var ordersByMonth = await _context.Set<Order>()
                .Where(o => o.OrderDate.Year == currentYear)
                .GroupBy(o => o.OrderDate.Month)
                .Select(g => new { Month = g.Key, Count = g.Count() })
                .ToListAsync();
                
            ViewBag.OrdersByMonth = ordersByMonth;
            
            // Thống kê laptop theo danh mục
            var laptopsByCategory = await _context.Laptops
                .GroupBy(l => l.CategoryId)
                .Select(g => new { CategoryId = g.Key, Count = g.Count() })
                .ToListAsync();
                
            ViewBag.LaptopsByCategory = laptopsByCategory;
            
            return View();
        }
        
        private bool UserExists(int id)
        {
            return _context.Set<User>().Any(e => e.Id == id);
        }
    }
}
