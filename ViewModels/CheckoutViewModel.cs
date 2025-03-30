using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
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

        // Direct cart items list without relying on Cart object
        public List<CartItem> CartItems { get; set; } = new List<CartItem>();

        // Calculate total directly from CartItems
        public decimal TotalAmount => CartItems?.Sum(i => i.Quantity * (i.Laptop?.Price ?? 0)) ?? 0;

        [Display(Name = "Địa chỉ")]
        public string Address { get; set; }

        [Display(Name = "Thành phố")]
        public string City { get; set; }

        [Display(Name = "Tỉnh/Thành phố")]
        public string State { get; set; }

        [Display(Name = "Mã bưu điện")]
        public string ZipCode { get; set; }

        [Display(Name = "Số thẻ")]
        public string CardNumber { get; set; }

        [Display(Name = "Ngày hết hạn")]
        public string ExpirationDate { get; set; }

        [Display(Name = "Mã CVV")]
        public string CVV { get; set; }
    }
}