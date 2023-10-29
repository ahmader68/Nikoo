using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nikoo.Models.ViewModels
{
    public class DiscountViewModel
    {
        public Discount Discount { get; set; }
        [ValidateNever]
        public IEnumerable<SelectListItem> DiscountList { get; set; }
    }
}
