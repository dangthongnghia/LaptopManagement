using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using LaptopManagement.Models;

namespace LaptopManagement.ViewModels
{
    public class CheckoutViewModel
    {
        public int Id { get; set; }
        
        [Required(ErrorMessage = "Vui lòng nhập họ tên")]
        [Display(Name = "Họ tên")]
        public string CustomerName { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập email")]
        [EmailAddress(ErrorMessage = "Email không hợp lệ")]
        [Display(Name = "Email")]
        public string CustomerEmail { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập số điện thoại")]
        [Phone(ErrorMessage = "Số điện thoại không hợp lệ")]
        [Display(Name = "Số điện thoại")]
        public string CustomerPhone { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập địa chỉ giao hàng")]
        [Display(Name = "Địa chỉ giao hàng")]
        public string ShippingAddress { get; set; }

        [Display(Name = "Ghi chú")]
        public string Notes { get; set; }

        public Cart Cart { get; set; }

        [Required]
        public string Address { get; set; }
        
        [Required]
        public string City { get; set; }
        
        [Required]
        public string State { get; set; }
        
        [Required]
        [Display(Name = "Zip Code")]
        public string ZipCode { get; set; }
        
        [Required]
        [Display(Name = "Card Number")]
        public string CardNumber { get; set; }
        
        [Required]
        [Display(Name = "Expiration Date")]
        public string ExpirationDate { get; set; }
        
        [Required]
        public string CVV { get; set; }
    }
}
