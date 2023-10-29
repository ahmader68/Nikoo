using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nikoo.Models
{
    public class Product
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [DisplayName("Product Name")]
        public string? Title { get; set; }
        public string? Description { get; set; }
        [Required]
        [DisplayName("Category")]
        public int CategoryId { get; set; } = -1;
        [ForeignKey("CategoryId")]
        [ValidateNever]
        public Category? Category { get; set; }
        [ValidateNever]
        public string ImageUrl { get; set; } = "";
        [Required]
        public int Price { get; set; }
        [Required]
        [DisplayName("Is Active")]
        public bool IsActive { get; set; }
        [DisplayName("Is Important")]
        public bool IsImportant { get; set; }
        [DisplayName("Is Suggested")]
        public bool IsSuggested { get; set; }
        public int SellCount { get; set; }
        [Required]
        [DisplayName("Store Capacity")]
        public int StoreCapacity { get; set; }
    }
}
