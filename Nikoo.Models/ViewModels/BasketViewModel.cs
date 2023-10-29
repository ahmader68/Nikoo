using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nikoo.Models.ViewModels
{
    public class BasketViewModel
    {
        public Basket? Basket { get; set; }
        public IEnumerable<BasketItem>? BasketItems { get; set; }
        public IEnumerable<SelectListItem>? PostTypes { get; set; }
        public Payment? Payment { get; set; }
    }
}
