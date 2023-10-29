using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nikoo.Models
{
    public class Basket
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string UserId { get; set; }
        [Required]
        public string? Address { get; set; } = "";
        [Required]
        public int? PostType { get; set; } = 0;
        public int TotalFactor { get; set; }
        public int FinalFactor { get; set; }
        public int? DiscountId { get; set; }
        [ValidateNever]
        public Discount? Discount { get; set; }
        public DateTime CreateDate { get; set; }
        public bool? IsPaid { get; set; }
        [ValidateNever]
        public IEnumerable<BasketItem> BasketItem { get; set; }
    }
}
