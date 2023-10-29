using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nikoo.Models
{
    public class BasketItem
    {
        [Key] 
        public int Id { get; set; }
        [Required]
        [ForeignKey("BasketId")]
        public int BasketId { get; set; }
        [Required]
        [ForeignKey("ProductId")]
        public int ProductId { get; set; }
        [ValidateNever]
        public Product? Product { get; set; }
        [Required]
        public int Quantity { get; set; }
        [Required]
        public int UnitPrice { get; set; }
    }
}
