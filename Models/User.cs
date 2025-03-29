using System.ComponentModel.DataAnnotations;
using System;
using System.Collections.Generic;

namespace LaptopManagement.Models
{
    public class User
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string PasswordHash { get; set; }
        public string Email { get; set; }
        public string FullName { get; set; }
        public bool IsAdmin { get; set; }
        public DateTime CreatedDate { get; set; }
        public bool IsActive { get; set; }
        public string Role { get; set; } // Added Role property
         public DateTime CreatedAt { get; set; }

    }
}

