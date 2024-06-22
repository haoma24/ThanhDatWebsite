using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ThanhDatWebsite.Models
{
    public class GioHangModel
    {
        public GioHangModel(Products sanPham, int sl)
        {
            Item = sanPham;
            Quantity = sl;
            Price = (double)(Item.UnitPrice * Quantity);
            ProductNum = sanPham.ProductID.Count();
        }

        public int Quantity { get; set; }
        public int ProductNum { get; set; }

        public double Price { get; set; }


        public virtual Products Item { get; set; }
    }
}