using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ThanhDatWebsite.Models
{
    public class KhachHangModel
    {
        public List<CustomersModel> listCustomers { get; set; }
        public List<Products> listProducts { get; set; }
    }
}