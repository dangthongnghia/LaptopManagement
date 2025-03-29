using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using LaptopManagement.Data;
using LaptopManagement.Models;

namespace LaptopManagement.Controllers
{
    public class ChatbotController : Controller
    {
        private readonly LaptopManagementContext _context;

        public ChatbotController(LaptopManagementContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ProcessQuery(string query)
        {
            if (string.IsNullOrEmpty(query))
            {
                return Json(new { success = false, message = "Vui lòng nhập câu hỏi" });
            }

            var response = await GenerateResponse(query);
            return Json(new { success = true, message = response });
        }

        private async Task<string> GenerateResponse(string query)
        {
            query = query.ToLower().Trim();

            // Xử lý các câu hỏi phổ biến
            if (query.Contains("sản phẩm") && (query.Contains("mới") || query.Contains("mới nhất")))
            {
                var latestLaptops = await _context.Laptops
                    .Where(l => l.Status)
                    .OrderByDescending(l => l.Id)
                    .Take(3)
                    .Select(l => l.Name)
                    .ToListAsync();

                if (latestLaptops.Any())
                {
                    return $"Các sản phẩm mới nhất của chúng tôi: {string.Join(", ", latestLaptops)}";
                }
                return "Hiện tại chúng tôi chưa có sản phẩm mới.";
            }
            else if (query.Contains("giá") && query.Contains("laptop"))
            {
                return "Giá laptop của chúng tôi dao động từ 10 triệu đến 50 triệu đồng tùy thuộc vào cấu hình. Bạn có thể xem chi tiết tại trang sản phẩm.";
            }
            else if (query.Contains("bảo hành"))
            {
                return "Tất cả laptop của chúng tôi đều được bảo hành 12 tháng chính hãng. Đối với một số dòng cao cấp, thời gian bảo hành có thể lên đến 24 tháng.";
            }
            else if (query.Contains("thanh toán") || query.Contains("payment"))
            {
                return "Chúng tôi hỗ trợ nhiều phương thức thanh toán: tiền mặt, chuyển khoản ngân hàng, và các ví điện tử phổ biến.";
            }
            else if (query.Contains("giao hàng") || query.Contains("vận chuyển") || query.Contains("shipping"))
            {
                return "Chúng tôi giao hàng toàn quốc. Thời gian giao hàng từ 1-3 ngày tùy khu vực. Miễn phí giao hàng cho đơn hàng từ 10 triệu đồng.";
            }
            else if (query.Contains("liên hệ") || query.Contains("contact"))
            {
                return "Bạn có thể liên hệ với chúng tôi qua số điện thoại: 0123456789 hoặc email: support@laptopmanagement.com";
            }
            else
            {
                return "Xin lỗi, tôi chưa hiểu câu hỏi của bạn. Vui lòng thử lại với câu hỏi khác hoặc liên hệ trực tiếp với chúng tôi qua hotline: 0123456789.";
            }
        }
    }
}
