using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ThanhDatWebsite.Models
{
    public class DonHangModel
    {
        [Required]
        public string MaDH { get; set; }
        public int SL { get; set; }
        public IEnumerable<Orders> DH { get; set; }
        public IEnumerable<OrderDetails> CTDH { get; set; }
        public List<Products> SanPham { get; set; }
        public List<string> TenSP { get; set; }
    }
}