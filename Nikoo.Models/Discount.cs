using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nikoo.Models
{
    public class Discount
    {
        [Key]
        public int Id { get; set; }
        public string? Title { get; set; }
        [Required]
        public int IsPercent { get; set; }
        [Required]
        public int Value { get; set; }
        [Required]
        [Display(Name = "Discount Code")]
        public string? DiscountCode { get; set; }
        [Required]
        public DateTime CreateDate { get; set; }
        [Display(Name = "Is Used")]
        public bool IsUsed { get; set; } = false;
    }
}
