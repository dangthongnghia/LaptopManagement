using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LaptopManagement.Models
{
    public class Cart
    {
        [Key]
        public int Id { get; set; }
        
        // Add UserId property to match the query in CartController
        public int UserId { get; set; }
        
        [ForeignKey("UserId")]
        public User User { get; set; }
        
        public DateTime CreatedAt { get; set; }
        
        public virtual ICollection<CartItem> Items { get; set; }
    }
}