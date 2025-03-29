using LaptopManagement;
using LaptopManagement.Data;
using LaptopManagement.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

public class OrdersController : Controller
{
    private readonly LaptopManagementContext _context;

    public OrdersController(LaptopManagementContext context)
    {
        _context = context;
    }

    public IActionResult Index()
    {
        return View(_context.Orders.ToList());
    }

    // GET: Orders/Create
    public IActionResult Create(int laptopId)
    {
        var laptop = _context.Laptops.Find(laptopId); // Sửa từ Laptop thành Laptops
        if (laptop == null)
        {
            return NotFound();
        }

        var order = new Order
        {
            LaptopId = laptopId,
            Laptop = laptop,
            OrderDate = DateTime.Now,
            Status = "Pending" // Thêm status mặc định
        };

        return View(order);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("LaptopId,CustomerName,CustomerEmail,CustomerPhone,OrderDate,Status")] Order order)
    {
        if (ModelState.IsValid)
        {
            order.OrderDate = DateTime.Now; // Đảm bảo OrderDate được set
            _context.Orders.Add(order);
            await _context.SaveChangesAsync();
            return RedirectToAction("Details", "Home", new { id = order.LaptopId });
        }

        // Nếu ModelState không valid, lấy lại thông tin Laptop
        var laptop = await _context.Laptops.FindAsync(order.LaptopId);
        order.Laptop = laptop;
        return View(order);
    }
}