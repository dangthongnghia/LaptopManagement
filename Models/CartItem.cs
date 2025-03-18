using System;
using System.ComponentModel.DataAnnotations;

namespace LaptopManagement.Models
{
    public class CartItem
    {
        public int Id { get; set; }
        public int LaptopId { get; set; }
        public string CartId { get; set; }
        public int Quantity { get; set; }
        public DateTime DateCreated { get; set; }

        public decimal Price { get; set; }

        
        // Navigation property
        public virtual Laptop Laptop { get; set; }
    }
}
