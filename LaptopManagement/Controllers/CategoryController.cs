using LaptopManagement.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using LaptopManagement; // Ensure this namespace exists and is correctly referenced

public class CategoryController : Controller
{
    private readonly LaptopManagementContext _context;

    public CategoryController(LaptopManagementContext context)
    {
        _context = context;
    }

    // Hiển thị danh sách Category
    public IActionResult Index()
    {
        var categories = _context.Set<Category>().ToList(); // Use Set<Category>() instead of _context.Categories
        return View(categories);
    }

    // Tạo Category mới (GET)
    public IActionResult Create()
    {
        return View();
    }

    // Tạo Category mới (POST)
    [HttpPost]
    public IActionResult Create(Category category)
    {
        if (ModelState.IsValid)
        {
            _context.Set<Category>().Add(category); // Use Set<Category>() instead of _context.Categories
            _context.SaveChanges();
            return RedirectToAction("Index");
        }
        return View(category);
    }

    // Sửa Category (GET)
    public IActionResult Edit(int id)
    {
        var category = _context.Set<Category>().Find(id); // Use Set<Category>() instead of _context.Categories
        if (category == null) return NotFound();
        return View(category);
    }

    // Sửa Category (POST)
    [HttpPost]
    public IActionResult Edit(Category category)
    {
        if (ModelState.IsValid)
        {
            _context.Set<Category>().Update(category); // Use Set<Category>() instead of _context.Categories
            _context.SaveChanges();
            return RedirectToAction("Index");
        }
        return View(category);
    }

    // Xóa Category
    public IActionResult Delete(int id)
    {
        var category = _context.Set<Category>().Find(id); // Use Set<Category>() instead of _context.Categories
        if (category == null) return NotFound();
        _context.Set<Category>().Remove(category); // Use Set<Category>() instead of _context.Categories
        _context.SaveChanges();
        return RedirectToAction("Index");
    }
}