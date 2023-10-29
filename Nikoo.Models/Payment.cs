using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nikoo.Models
{
    public class Payment
    {
        [Key]
        public int Id { get; set; }
        public int BasketId { get; set; }
        public string UserId { get; set; }
        public int TotalPaid { get; set;}
        public bool IsPaid { get; set;}
        public string? TrackingCode { get; set;}
    }
}
