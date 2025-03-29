using LaptopManagement.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using LaptopManagement;
using Microsoft.AspNetCore.Mvc.Rendering;
using LaptopManagement.Data;
using Microsoft.AspNetCore.Authorization;

namespace LaptopManagement.Controllers
{

    [Authorize(Roles ="Admin")]
    public class LaptopsController : Controller
    {
        private readonly LaptopManagementContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment; 

        public LaptopsController(LaptopManagementContext context, IWebHostEnvironment webHostEnvironment) // Modify this line
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment; 
        }

        // GET: Laptops
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





        // GET: Laptops/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();
            var laptop = await _context.Laptops
                .Include(l => l.Images)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (laptop == null) return NotFound();
            return View(laptop);
        }
        // GET: Laptops/Create
        public IActionResult Create()
        {
            // Log để kiểm tra categories có dữ liệu không
            var categories = _context.Categories.ToList();
            Console.WriteLine($"Available Categories: {categories.Count}");
            foreach (var category in categories)
            {
                Console.WriteLine($"Category: Id={category.Id}, Name={category.Name}");
            }

            ViewBag.Categories = new SelectList(categories, "Id", "Name");
            return View();
        }

        // POST: Laptops/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Brand,Price,Description,CPU,RAM,Storage,Status,CategoryId")] Laptop laptop, List<IFormFile> imageFiles)
        {
            Console.WriteLine("=== BEGIN CREATE LAPTOP ===");
            Console.WriteLine($"Input Data: Name={laptop.Name}, Brand={laptop.Brand},Description={laptop.Description}, Price={laptop.Price}, CategoryId={laptop.CategoryId}");

            // Kiểm tra ModelState
            if (!ModelState.IsValid)
            {
                Console.WriteLine("ModelState is invalid. Validation errors:");
                foreach (var modelStateEntry in ModelState.Values)
                {
                    foreach (var error in modelStateEntry.Errors)
                    {
                        Console.WriteLine($"- {error.ErrorMessage}");
                    }
                }
                ViewBag.Categories = new SelectList(_context.Categories, "Id", "Name");
                return View(laptop);
            }

            try
            {
                var images = new List<LaptopImage>();
                Console.WriteLine($"Processing {imageFiles?.Count ?? 0} image files");

                if (imageFiles != null && imageFiles.Count > 0)
                {
                    string uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "images");
                    Console.WriteLine($"Upload folder path: {uploadsFolder}");

                    if (!Directory.Exists(uploadsFolder))
                    {
                        Directory.CreateDirectory(uploadsFolder);
                        Console.WriteLine("Created uploads folder");
                    }

                    foreach (var file in imageFiles)
                    {
                        Console.WriteLine($"Processing file: {file.FileName}, Size: {file.Length} bytes");

                        if (file.Length > 0)
                        {
                            // Validate image file
                            if (!file.ContentType.StartsWith("image/"))
                            {
                                Console.WriteLine($"Invalid file type: {file.ContentType}");
                                ModelState.AddModelError("imageFiles", "Vui lòng chọn file ảnh (jpg, png, etc.).");
                                continue;
                            }
                            if (file.Length > 5 * 1024 * 1024)
                            {
                                Console.WriteLine($"File too large: {file.Length} bytes");
                                ModelState.AddModelError("imageFiles", "Kích thước file không được vượt quá 5MB.");
                                continue;
                            }

                            try
                            {
                                string uniqueFileName = Guid.NewGuid().ToString() + "_" + file.FileName;
                                string filePath = Path.Combine(uploadsFolder, uniqueFileName);
                                Console.WriteLine($"Saving file to: {filePath}");

                                using (var fileStream = new FileStream(filePath, FileMode.Create))
                                {
                                    await file.CopyToAsync(fileStream);
                                }

                                images.Add(new LaptopImage
                                {
                                    ImagePath = "/images/" + uniqueFileName,
                                    IsPrimary = images.Count == 0
                                });
                                Console.WriteLine($"File saved successfully: {uniqueFileName}");
                            }
                            catch (Exception ex)
                            {
                                Console.WriteLine($"Error saving file: {ex.Message}");
                                ModelState.AddModelError("", $"Lỗi khi lưu file: {ex.Message}");
                            }
                        }
                    }
                }
                else
                {
                    Console.WriteLine("No images uploaded, using default image");
                    images.Add(new LaptopImage
                    {
                        ImagePath = "/images/default-laptop.jpg",
                        IsPrimary = true
                    });
                }

                // Save Laptop
                Console.WriteLine("Attempting to save laptop to database");
                try
                {
                    _context.Laptops.Add(laptop);
                    await _context.SaveChangesAsync();
                    Console.WriteLine($"Laptop saved successfully with ID: {laptop.Id}");

                    // Save Images
                    Console.WriteLine($"Saving {images.Count} images");
                    foreach (var image in images)
                    {
                        image.LaptopId = laptop.Id;
                        _context.LaptopImages.Add(image);
                        Console.WriteLine($"Added image to context: {image.ImagePath}");
                    }

                    int changes = await _context.SaveChangesAsync();
                    Console.WriteLine($"Saved {changes} changes to database");

                    Console.WriteLine("=== CREATE LAPTOP COMPLETED SUCCESSFULLY ===");
                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateException dbEx)
                {
                    Console.WriteLine("=== DATABASE ERROR ===");
                    Console.WriteLine($"Error message: {dbEx.Message}");
                    Console.WriteLine($"Inner exception: {dbEx.InnerException?.Message}");
                    Console.WriteLine($"Stack trace: {dbEx.StackTrace}");
                    ModelState.AddModelError("", "Lỗi khi lưu vào database: " + dbEx.InnerException?.Message);
                }
                catch (Exception ex)
                {
                    Console.WriteLine("=== UNEXPECTED ERROR ===");
                    Console.WriteLine($"Error type: {ex.GetType().Name}");
                    Console.WriteLine($"Error message: {ex.Message}");
                    Console.WriteLine($"Stack trace: {ex.StackTrace}");
                    ModelState.AddModelError("", "Lỗi không xác định: " + ex.Message);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("=== CRITICAL ERROR ===");
                Console.WriteLine($"Error type: {ex.GetType().Name}");
                Console.WriteLine($"Error message: {ex.Message}");
                Console.WriteLine($"Stack trace: {ex.StackTrace}");
                ModelState.AddModelError("", "Lỗi nghiêm trọng: " + ex.Message);
            }

            Console.WriteLine("=== CREATE LAPTOP FAILED - RETURNING TO VIEW ===");
            ViewBag.Categories = new SelectList(_context.Categories, "Id", "Name");
            return View(laptop);
        }
        // GET: Laptops/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var laptop = await _context.Laptops
                .Include(l => l.Images) // Load danh sách ảnh nếu có
                .FirstOrDefaultAsync(m => m.Id == id);
            if (laptop == null)
            {
                return NotFound();
            }

            // Load danh sách các danh mục
            ViewBag.Categories = new SelectList(_context.Categories, "Id", "Name", laptop.CategoryId);
            return View(laptop);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Brand,Price,Description,CPU,RAM,Storage,Status,CategoryId")] Laptop laptop, List<IFormFile> imageFiles, int[] deleteImages)
        {
            if (id != laptop.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var existingLaptop = await _context.Laptops
                        .Include(l => l.Images)
                        .FirstOrDefaultAsync(m => m.Id == id);

                    if (existingLaptop == null)
                    {
                        return NotFound();
                    }

                    // Update basic information
                    existingLaptop.Name = laptop.Name;
                    existingLaptop.Brand = laptop.Brand;
                    existingLaptop.Price = laptop.Price;
                    existingLaptop.Description = laptop.Description; 

                    existingLaptop.CPU = laptop.CPU;
                    existingLaptop.RAM = laptop.RAM;
                    existingLaptop.Storage = laptop.Storage;
                    existingLaptop.Status = laptop.Status;
                    existingLaptop.CategoryId = laptop.CategoryId;

                    // Delete selected images
                    if (deleteImages != null && deleteImages.Length > 0)
                    {
                        foreach (var imageId in deleteImages)
                        {
                            var image = existingLaptop.Images.FirstOrDefault(img => img.Id == imageId);
                            if (image != null)
                            {
                                // Delete the image file from the server
                                var imagePath = Path.Combine(_webHostEnvironment.WebRootPath, image.ImagePath.TrimStart('/'));
                                if (System.IO.File.Exists(imagePath))
                                {
                                    System.IO.File.Delete(imagePath);
                                }

                                // Remove the image from the database
                                _context.LaptopImages.Remove(image);
                            }
                        }
                    }

                    // Handle new image uploads
                    if (imageFiles != null && imageFiles.Count > 0)
                    {
                        string uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "images");
                        if (!Directory.Exists(uploadsFolder))
                        {
                            Directory.CreateDirectory(uploadsFolder);
                        }

                        foreach (var file in imageFiles)
                        {
                            if (file.Length > 0)
                            {
                                if (!file.ContentType.StartsWith("image/"))
                                {
                                    ModelState.AddModelError("imageFiles", "Please select image files (jpg, png, etc.).");
                                    continue;
                                }
                                if (file.Length > 5 * 1024 * 1024)
                                {
                                    ModelState.AddModelError("imageFiles", "File size should not exceed 5MB.");
                                    continue;
                                }

                                string uniqueFileName = Guid.NewGuid().ToString() + "_" + file.FileName;
                                string filePath = Path.Combine(uploadsFolder, uniqueFileName);

                                using (var fileStream = new FileStream(filePath, FileMode.Create))
                                {
                                    await file.CopyToAsync(fileStream);
                                }

                                existingLaptop.Images.Add(new LaptopImage
                                {
                                    ImagePath = "/images/" + uniqueFileName,
                                    IsPrimary = existingLaptop.Images.Count == 0
                                });
                            }
                        }
                    }

                    _context.Update(existingLaptop);
                    await _context.SaveChangesAsync();

                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateException ex)
                {
                    Console.WriteLine("Database Error: " + ex.InnerException?.Message);
                    ModelState.AddModelError("", "Error saving data to the database.");
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", "Unexpected error: " + ex.Message);
                }
            }

            // Load danh sách các danh mục nếu có lỗi
            ViewBag.Categories = new SelectList(_context.Categories, "Id", "Name", laptop.CategoryId);
            return View(laptop);
        }



        // GET: Laptops/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var laptop = await _context.Laptops
                .FirstOrDefaultAsync(m => m.Id == id);
            if (laptop == null)
            {
                return NotFound();
            }

            return View(laptop);
        }

        // POST: Laptops/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var laptop = await _context.Laptops
                .Include(l => l.Images)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (laptop != null)
            {
                // Xóa các tệp ảnh khỏi máy chủ
                foreach (var image in laptop.Images)
                {
                    var imagePath = Path.Combine(_webHostEnvironment.WebRootPath, image.ImagePath.TrimStart('/'));
                    if (System.IO.File.Exists(imagePath))
                    {
                        System.IO.File.Delete(imagePath);
                    }
                }

                // Xóa laptop và các ảnh liên quan khỏi cơ sở dữ liệu
                _context.Laptops.Remove(laptop);
                _context.LaptopImages.RemoveRange(laptop.Images);

                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }
    }
}