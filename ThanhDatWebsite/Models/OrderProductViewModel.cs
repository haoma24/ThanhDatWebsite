using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ThanhDatWebsite.Models
{
    public class OrderProductViewModel
    {
        public string OrderID { get; set; }
        public string Status { get; set; }
        public double? TotalAmount { get; set; }
        public DateTime? OrderDate { get; set; }
        public IEnumerable<ProductViewModel> Products { get; set; }
    }
}