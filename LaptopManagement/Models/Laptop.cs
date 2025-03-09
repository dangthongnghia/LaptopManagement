using LaptopManagement.Models;


namespace LaptopManagement.Models
{
    public class Laptop
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Brand { get; set; }
        public decimal Price { get; set; }
        public string CPU { get; set; }
        public string RAM { get; set; }
        public string Storage { get; set; }
        public bool Status { get; set; }
        public int CategoryId { get; set; } // Khóa ngoại liên kết với Category
        public virtual Category Category { get; set; } // Tham chiếu đến Category
        public virtual ICollection<LaptopImage> Images { get; set; } = new List<LaptopImage>();
        public virtual ICollection<Order> Orders { get; set; } = new List<Order>();
    }
}
