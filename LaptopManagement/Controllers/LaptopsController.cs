using LaptopManagement.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using LaptopManagement;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace LaptopManagement.Controllers
{
    public class LaptopsController : Controller
    {
        private readonly LaptopManagementContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment; // Add this line

        public LaptopsController(LaptopManagementContext context, IWebHostEnvironment webHostEnvironment) // Modify this line
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment; // Add this line
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
            ViewBag.Categories = new SelectList(_context.Categories, "Id", "Name");

            return View();
        }

        // POST: Laptops/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Brand,Price,CPU,RAM,Storage,Status")] Laptop laptop, List<IFormFile> imageFiles)
        {
            if (ModelState.IsValid)
            {
                var images = new List<LaptopImage>();

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
                                ModelState.AddModelError("imageFiles", "Vui lòng chọn file ảnh (jpg, png, etc.).");
                                continue;
                            }
                            if (file.Length > 5 * 1024 * 1024) // Giới hạn 5MB
                            {
                                ModelState.AddModelError("imageFiles", "Kích thước file không được vượt quá 5MB.");
                                continue;
                            }

                            string uniqueFileName = Guid.NewGuid().ToString() + "_" + file.FileName;
                            string filePath = Path.Combine(uploadsFolder, uniqueFileName);

                            using (var fileStream = new FileStream(filePath, FileMode.Create))
                            {
                                await file.CopyToAsync(fileStream);
                            }
                            images.Add(new LaptopImage
                            {
                                ImagePath = "/images/" + uniqueFileName,
                                IsPrimary = images.Count == 0
                            });
                            Console.WriteLine($"File saved to disk: {filePath}");
                        }
                    }
                    Console.WriteLine($"Số ảnh upload: {imageFiles.Count}");
                }
                else
                {
                    images.Add(new LaptopImage
                    {
                        ImagePath = "/images/default-laptop.jpg",
                        IsPrimary = true
                    });
                    Console.WriteLine("Không có ảnh upload, sử dụng ảnh mặc định.");
                }

                if (ModelState.IsValid)
                {
                    _context.Laptops.Add(laptop);
                    await _context.SaveChangesAsync();
                    Console.WriteLine($"Laptop Id sau khi lưu: {laptop.Id}");

                    foreach (var image in images)
                    {
                        image.LaptopId = laptop.Id;
                        _context.LaptopImages.Add(image);
                        Console.WriteLine($"Image added to context: ImagePath={image.ImagePath}, LaptopId={image.LaptopId}");
                    }

                    try
                    {
                        int changes = await _context.SaveChangesAsync();
                        Console.WriteLine($"Số thay đổi được commit: {changes}");
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Lỗi khi lưu ảnh: {ex.Message}");
                        ModelState.AddModelError("", "Lỗi khi lưu ảnh vào database: " + ex.Message);
                        return View(laptop);
                    }

                    return RedirectToAction(nameof(Index));
                }
            }
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
            return View(laptop);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Brand,Price,CPU,RAM,Storage,Status")] Laptop laptop, List<IFormFile> imageFiles)
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
                        .Include(l => l.Images) // Đảm bảo load danh sách ảnh hiện tại
                        .FirstOrDefaultAsync(m => m.Id == id);

                    if (existingLaptop == null)
                    {
                        return NotFound();
                    }

                    // Cập nhật thông tin cơ bản
                    existingLaptop.Name = laptop.Name;
                    existingLaptop.Brand = laptop.Brand;
                    existingLaptop.Price = laptop.Price;
                    existingLaptop.CPU = laptop.CPU;
                    existingLaptop.RAM = laptop.RAM;
                    existingLaptop.Storage = laptop.Storage;
                    existingLaptop.Status = laptop.Status;

                    // Xử lý upload ảnh mới
                    if (imageFiles != null && imageFiles.Count > 0)
                    {
                        string uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "images");
                        if (!Directory.Exists(uploadsFolder))
                        {
                            Directory.CreateDirectory(uploadsFolder); // Tạo thư mục nếu chưa tồn tại
                        }

                        foreach (var file in imageFiles)
                        {
                            if (file.Length > 0)
                            {
                                // Kiểm tra định dạng và kích thước
                                if (!file.ContentType.StartsWith("image/"))
                                {
                                    ModelState.AddModelError("imageFiles", "Vui lòng chọn file ảnh (jpg, png, etc.).");
                                    continue;
                                }
                                if (file.Length > 5 * 1024 * 1024) // Giới hạn 5MB
                                {
                                    ModelState.AddModelError("imageFiles", "Kích thước file không được vượt quá 5MB.");
                                    continue;
                                }

                                string uniqueFileName = Guid.NewGuid().ToString() + "_" + file.FileName;
                                string filePath = Path.Combine(uploadsFolder, uniqueFileName);

                                // Lưu file vào hệ thống
                                using (var fileStream = new FileStream(filePath, FileMode.Create))
                                {
                                    await file.CopyToAsync(fileStream);
                                }

                                // Thêm ảnh mới vào danh sách
                                existingLaptop.Images.Add(new LaptopImage
                                {
                                    ImagePath = "/images/" + uniqueFileName,
                                    IsPrimary = existingLaptop.Images.Count == 0 // Đặt ảnh đầu tiên là chính nếu chưa có
                                });
                            }
                        }
                    }

                    // Cập nhật và lưu vào database
                    _context.Update(existingLaptop); // Đảm bảo cập nhật cả danh sách Images
                    await _context.SaveChangesAsync(); // Commit thay đổi

                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateException ex)
                {
                    // Ghi log lỗi (nếu có)
                    Console.WriteLine("Database Error: " + ex.InnerException?.Message);
                    ModelState.AddModelError("", "Lỗi khi lưu dữ liệu vào database.");
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", "Lỗi không xác định: " + ex.Message);
                }
            }

            // Nếu có lỗi, trả lại view với model để hiển thị lỗi
            return View(laptop);
        }
        // Helper method


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
            var laptop = await _context.Laptops.FindAsync(id);
            if (laptop != null)
            {
                _context.Laptops.Remove(laptop);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool LaptopExists(int id)
        {
            return _context.Laptops.Any(e => e.Id == id);
        }
    }
}