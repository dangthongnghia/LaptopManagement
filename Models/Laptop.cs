using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using static System.Runtime.InteropServices.JavaScript.JSType;
using System.Net.Sockets;
namespace LaptopManagement.Models
{
    public class Laptop
    {
       
        public int Id { get; set; }
        [Required(ErrorMessage = "Vui lòng nhập tên laptop")]
        [StringLength(100)]
        public string Name { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập thương hiệu")]
        [StringLength(50)]
        public string Brand { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập giá")]
        [Range(0, double.MaxValue, ErrorMessage = "Giá phải lớn hơn 0")]
        [Column(TypeName = "decimal(18,2)")]
        public decimal Price { get; set; }

        [StringLength(1000)]
        [Display(Name = "Mô tả")]
        public string? Description { get; set; }

        [StringLength(50)]
        public string? CPU { get; set; }

        [StringLength(50)]
        public string? RAM { get; set; }

        [StringLength(50)]
        public string? Storage { get; set; }

        public bool Status { get; set; }

        [Required(ErrorMessage = "Vui lòng chọn danh mục")]
        [Display(Name = "Category")]
        public int CategoryId { get; set; }

        [ForeignKey("CategoryId")]
        public virtual Category? Category { get; set; }

        // Bỏ [Required] ở đây vì hình ảnh sẽ được xử lý trong controller
        public virtual ICollection<LaptopImage> Images { get; set; } = new List<LaptopImage>();

        public virtual ICollection<Order> Orders { get; set; } = new List<Order>();
    }
}