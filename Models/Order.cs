namespace LaptopManagement.Models
{
    public class Order
    {
        public int Id { get; set; }
        public int LaptopId { get; set; }
        public virtual Laptop Laptop { get; set; }
        public string CustomerName { get; set; }
        public string CustomerEmail { get; set; }
        public string CustomerPhone { get; set; }
        public string ShippingAddress { get; set; } // Added this property
        public string Notes { get; set; } // Added this property
        public DateTime OrderDate { get; set; }
        public string Status { get; set; }
        public string? ImagePath { get; set; } // Nếu bạn thêm ảnh cho đơn hàng
        public List<CartItem> Items { get; set; } = new List<CartItem>();
        public DateTime CreatedDate { get; set; }

        // Add a calculated property
        public decimal TotalAmount
        {
            get { return Items?.Sum(item => item.Price * item.Quantity) ?? 0; }
            set { }
        }
        // Thêm vào model Order hiện có của bạn
        public virtual ICollection<OrderDetail> OrderDetails { get; set; } = new List<OrderDetail>();
    }
}