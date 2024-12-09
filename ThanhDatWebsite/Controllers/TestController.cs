using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ThanhDatWebsite.Models;

namespace ThanhDatWebsite.Controllers
{
    public class TestController : Controller
    {
        thanhdatEntities db = new thanhdatEntities();
        // GET: Test
        public ActionResult Index()
        {
            KhachHangModel model = new KhachHangModel();
            model.listCustomers = db.Customers.OrderBy(x => x.CustomerID).ToList();
            model.listProducts = db.Products.OrderBy(x => x.ProductID).ToList();
            return View(model);
        }
    }
}