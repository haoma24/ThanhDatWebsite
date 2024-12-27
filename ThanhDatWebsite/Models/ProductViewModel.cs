using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ThanhDatWebsite.Models
{
    public class ProductViewModel
    {
        public string ProductName { get; set; }
        public string Image { get; set; }
        public int? Quantity { get; set; }
        public double? TotalPrice { get; set; }
        public string Description { get; set; }
    }
}