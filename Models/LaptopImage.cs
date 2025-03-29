namespace LaptopManagement.Models
{
    public class LaptopImage
    {
        public int Id { get; set; }
        public int LaptopId { get; set; } // Khóa ngoại liên kết với Laptop
        public virtual Laptop Laptop { get; set; }
        public string ImagePath { get; set; } // Đường dẫn ảnh
        public bool IsPrimary { get; set; } // Đánh dấu ảnh chính (nếu cần)
    }
}