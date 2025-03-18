using LaptopManagement.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using LaptopManagement;
using LaptopManagement.Data; // Ensure this namespace exists and is correctly referenced
using Microsoft.AspNetCore.Authorization;

// Controllers/CategoryController.cs
    [Authorize(Roles = "Admin")]
public class CategoryController : Controller
{
    private readonly LaptopManagementContext _context;

    public CategoryController(LaptopManagementContext context)
    {
        _context = context;
    }

    // GET: Category
    public async Task<IActionResult> Index()
    {
        return View(await _context.Categories.ToListAsync());
    }

    // GET: Category/Create
    public IActionResult Create()
    {
        return View();
    }

    // POST: Category/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("Name")] Category category)
    {
        try
        {
            if (ModelState.IsValid)
            {
                Console.WriteLine($"Attempting to create category: {category.Name}");
                _context.Add(category);
                await _context.SaveChangesAsync();
                Console.WriteLine($"Category created successfully with ID: {category.Id}");
                return RedirectToAction(nameof(Index));
            }
            else
            {
                Console.WriteLine("ModelState is invalid. Validation errors:");
                foreach (var modelStateEntry in ModelState.Values)
                {
                    foreach (var error in modelStateEntry.Errors)
                    {
                        Console.WriteLine($"- {error.ErrorMessage}");
                    }
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error creating category: {ex.Message}");
            ModelState.AddModelError("", "Không thể tạo danh mục. Vui lòng thử lại.");
        }
        return View(category);
    }
}