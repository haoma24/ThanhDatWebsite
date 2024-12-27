using Microsoft.Ajax.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ThanhDatWebsite.Models;

namespace ThanhDatWebsite.Controllers
{
    public class UserDashboardController : Controller
    {
        private thanhdatEntities1 db = new thanhdatEntities1();
        // GET: UserDashboard
        public ActionResult Index()
        {
            return View();
        }
        [HttpGet]
        public ActionResult users(string id)
        {
            try
            {
                // Tìm customer dựa trên ID truyền vào
                var customer = db.Customers.FirstOrDefault(c => c.CustomerID == id);
                if (customer != null)
                {
                    // Trả về JSON nếu tìm thấy customer
                    return Json(new
                    {
                        status = "200",
                        cus = new
                        {
                            Fullname = customer.FullName,
                            Email = customer.Email,
                            Phone = customer.Phone,
                            Sex = customer.Sex,
                            Address = customer.Address
                        }
                    }, JsonRequestBehavior.AllowGet);
                }

                // Trả về status 404 nếu không tìm thấy
                return Json(new { status = "404", message = "Customer not found" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                // Trả về status 500 nếu có lỗi xảy ra
                return Json(new { status = "500", message = "Internal Server Error", error = e.Message }, JsonRequestBehavior.AllowGet);
            }
        }
        [HttpPost]
        public ActionResult users(CustomersModel model)
        {
            try
            {
                var customer = db.Customers.FirstOrDefault(c => c.CustomerID == model.ID);
                if (customer != null)
                {
                    customer.FullName = model.Fullname;
                    customer.Address = model.Address;
                    customer.Email = model.Email;
                    customer.Phone = model.Phone;
                    customer.Sex = model.Sex;
                    db.SaveChanges();
                }
                return Json(new {status = "200" });
            }
            catch (Exception e)
            {
                return Json(new { status = "500", message = "Internal Server Error", error = e.Message });
            }
        }
        public ActionResult OrderHistory(string id)
        {
            var orderproduct = db.Orders.Where(o => o.CustomerID == id)
                    .Join(db.OrderDetails,
                    o => o.OrderID,
                    od => od.OrderID,
                    (o, od) => new { o, od })
                    .Join(db.Products,
                    temp => temp.od.ProductID,
                    p => p.ProductID,
                    (temp, p) => new
                    {
                        temp.o.OrderID,
                        ProductName = p.ProductName,
                        Quantity = temp.od.Quantity,
                        TotalPrice = temp.od.TotalPrice,
                        Image = p.Image,
                        Status = temp.o.Status,
                        TotalAmount = temp.o.TotalAmount,
                        OrderDate = temp.o.OrderDate,
                        Description = p.Description
                    }).GroupBy(x => x.OrderID)
                    .Select(group => new OrderProductViewModel
                    {
                        OrderID = group.Key,
                        TotalAmount = group.FirstOrDefault().TotalAmount,
                        OrderDate = group.FirstOrDefault().OrderDate,
                        Status = group.FirstOrDefault().Status,
                        Products = group.Select(g => new ProductViewModel
                        {
                            ProductName = g.ProductName,
                            Quantity = g.Quantity,
                            TotalPrice = g.TotalPrice,
                            Image = g.Image,
                            Description = g.Description
                            
                        }).ToList()
                    })
    .               ToList();

            return View(orderproduct.ToList());
        }
        [HttpGet]
        public ActionResult orders(string id)
        {
            try
            {
                var orderproduct = db.Orders.Where(o => o.CustomerID == id)
                    .Join(db.OrderDetails,
                    o => o.OrderID,
                    od => od.OrderID,
                    (o, od) => new { o, od })
                    .Join(db.Products,
                    temp => temp.od.ProductID,
                    p => p.ProductID,
                    (temp, p) => new
                    {
                        ProductName = p.ProductName,
                        Image = p.Image,
                        Status = temp.o.Status,
                        TotalAmount = temp.o.TotalAmount,
                        OrderDate = temp.o.OrderDate
                    });
                return Json(orderproduct, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                return Json(new { status = "500", message = "Internal Server Error", error = e });
            }
        }
        public ActionResult UserRank()
        {
            return View();
        }
        [HttpGet]
        public ActionResult getAmount(string id)
        {
            var Amount = db.Orders.Where(o => o.CustomerID == id)
                .Sum(o => o.TotalAmount);
            return Json(new { amount = Amount },JsonRequestBehavior.AllowGet);
        }
        public ActionResult WishList(string id)
        {
            var Product = db.Products.SelectMany(p => p.Wishlist).Where(w => w.CustomerID == id).Select(w=>w.Products);
            return View(Product.ToList());
        }
        public ActionResult MyAccount()
        {
            return View();
        }
        public ActionResult accounts(string id)
        {
            var accountid = db.Customers.Where(c=>c.CustomerID==id).Select(c=>c.AccountID).FirstOrDefault();
            var acc = db.Accounts.Where(a => a.AccountID == accountid).FirstOrDefault();
            return Json(new {status="200",name=acc.UserName, pass=acc.PasswordHash,email = acc.Email},JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public ActionResult accounts(AccountModel model)
        {
            var account = db.Customers.Where(c => c.CustomerID == model.Id).Select(c=>c.Accounts).FirstOrDefault();
            account.Email = model.Email;
            account.UserName = model.UserName;
            db.SaveChanges();
            return Json(new { status = "200" }, JsonRequestBehavior.AllowGet);
        }
    }
}