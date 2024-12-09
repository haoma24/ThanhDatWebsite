using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using ThanhDatWebsite.Models;

namespace ThanhDatWebsite.Controllers
{
    public class HomeController : Controller
    {
        private thanhdatEntities db = new thanhdatEntities();
        public ActionResult Index()
        {  
            var sanPham2 = db.Products.OrderBy(x => x.CategoryID);       
            return View(sanPham2.ToList());
        }
        public ActionResult TimKiem (FormCollection form)
        {
            string NoiDung = form["txtTimKiem"];
            if (string.IsNullOrEmpty(NoiDung))
            {
                return RedirectToAction("Index");
            }
            var sanPham = from c in db.Products
                          where c.ProductName.Contains(NoiDung)
                          select c;
            ViewBag.msg = string.Empty;
            if (sanPham == null)
                ViewBag.msg = "Không tìm thấy sản phẩm";
            return View(sanPham.ToList());
        }
        public ActionResult DangNhap()
        {
            return View();
        }
        public ActionResult DangKy()
        {
            return View();
        }
        public ActionResult TraCuuDonDatHang()
        {
            return View();
        }
        public ActionResult DonDatHang(FormCollection form)
        {
            string MaDH = form["MaDH"];
            DonHangModel model = new DonHangModel();
            model.DH = db.Orders.Where(x => x.OrderID == MaDH.TrimEnd());
            model.CTDH = db.OrderDetails.Where(x => x.OrderID == MaDH.TrimEnd());
            List<string> MaSP = db.OrderDetails.Where(x => x.OrderID == MaDH.TrimEnd()).Select(x => x.ProductID).ToList();
            List<Products> dsSanPham = new List<Products>();
            for (int i = 0; i < MaSP.Count; i++)
            {
                var ID = MaSP[i].TrimEnd();
                Products SP = db.Products.Where(x => x.ProductID == ID).FirstOrDefault();
                dsSanPham.Add(SP);
                model.SanPham = dsSanPham;

            }
            return View(model);
        }
        public ActionResult KiemTraUser(FormCollection form)
        {
            string TK = form["TenTK"];
            string MK = form["MK"];
            string Email = db.Accounts.Where(x => x.Email == TK).Select(x => x.Email).FirstOrDefault();
            string Pass = db.Accounts.Where(x => x.PasswordHash == MK).Select(x => x.PasswordHash).FirstOrDefault();
            string Loai = db.Accounts.Where(x => x.Email == Email).Select(x => x.Role).FirstOrDefault();
            if (Email != null && Pass != null)
                if (Loai == "ad" || Loai == "emp")
                    return RedirectToAction("Dashboard", "Home");
                else return RedirectToAction("Index", "Home");

            ViewBag.msg = "Sai tài khoản hoặc mật khẩu";
            return View("DangNhap");

        }
        public ActionResult DangKyUser(FormCollection form)
        {
            Accounts taiKhoan = new Accounts();
            string TK = form["TenTK"].ToLower();
            string MK = form["MK"];
            string XacNhanMK = form["XacNhanMK"];
            string Loai = "cus";
            if (TK == "" || MK == "" || XacNhanMK == "")
            {
                ViewBag.err = "Vui lòng không điền thông tin!";
                return View("DangKy");
            }
            if (!TK.Contains("@") || !TK.Contains("."))
            {
                ViewBag.err = "Email không hợp lệ!";
                return View("DangKy");
            }
            string ID = db.Accounts.Where(x => x.Email.ToLower() == TK).Select(x => x.Email).FirstOrDefault();
            if (ID != null)
            {
                ViewBag.err = "Tài khoản đã tồn tại!";
                return View("DangKy");
            }
            else if (MK != XacNhanMK)
            {
                ViewBag.err = "Mật khẩu chưa trùng khớp!";
                return View("DangKy");
            }
            else
            {

                ID = IdIncrementer.idincreament("Accounts");
                taiKhoan.AccountID = ID;
                taiKhoan.Email = TK;
                taiKhoan.PasswordHash = MK;
                taiKhoan.Role = Loai;
                db.Accounts.Add(taiKhoan);
                db.SaveChanges();
            }

            ViewBag.success = "Đăng ký thành công";
            return View("DangNhap");

        }
        public ActionResult DatHang(FormCollection form)
        {
            List<GioHangModel> gioHangModels = (List<GioHangModel>)Session["cart"];
            Orders dh = new Orders();
            Customers kh = new Customers();
            string MaDH = IdIncrementer.idincreament("Orders");
            string MaKH = IdIncrementer.idincreament("Customers");
            string NgayDH = DateTime.Now.ToString("dd/MM/yyyy");
            DateTime NgayDHdt = DateTime.ParseExact(NgayDH, "dd/MM/yyyy", CultureInfo.InvariantCulture);
            string TenKH = form["TenKH"];
            string SDT = form["SDT"];
            string HTTT = form["rbtnHTTT"];
            string HTVC = form["rbtnHT"];
            string DiaChi = form["txtSoNha"] + ", " + form["ddlP"] + ", " + form["ddlQ"] + ", " + form["ddlTP"];
            if (SDT == db.Customers.Where(x => x.Phone == SDT).Select(x => x.Phone).FirstOrDefault())
            {
                dh.OrderID = MaDH;
                dh.Name = TenKH;
                dh.TotalAmount = null;
                dh.Address = DiaChi;
                dh.Phone = SDT;
                dh.PaymentID = HTTT;
                dh.DeliveryMethodID = HTVC;
                dh.OrderDate = NgayDHdt;
                dh.Status = "Đang xử lý";
                string _MaKH = db.Customers.Where(x => x.Phone == SDT).Select(x => x.CustomerID).First();
                dh.CustomerID = _MaKH;
                db.Orders.Add(dh);
                db.SaveChanges();
                double TriGia = 0;
                foreach (var item in gioHangModels)
                {
                    OrderDetails ctdh = new OrderDetails();
                    var sp = db.Products.Find(item.Item.ProductID);
                    ctdh.ProductID = sp.ProductID;
                    ctdh.OrderID = MaDH;
                    ctdh.Quantity = item.Quantity;
                    ctdh.UnitPrice = item.Item.UnitPrice;
                    ctdh.TotalPrice = item.Price;
                    TriGia += (double)ctdh.TotalPrice;  
                    db.OrderDetails.Add(ctdh);
                    db.SaveChanges();
                }
                var donDatHang = db.Orders.First(g => g.OrderID == MaDH);
                donDatHang.TotalAmount = TriGia;
                db.SaveChanges();
            }
            else
            {
                kh.Phone = SDT;
                kh.FullName = TenKH;
                kh.Email = null;
                kh.CustomerID = MaKH;
                db.Customers.Add(kh);
                db.SaveChanges();
                dh.OrderID = MaDH;
                dh.Name = TenKH;
                dh.TotalAmount = null;
                dh.Address = DiaChi;
                dh.Phone = SDT;
                dh.PaymentID = HTTT;
                dh.DeliveryMethodID = HTVC;
                dh.OrderDate = NgayDHdt;
                dh.Status = "Đang xử lý";
                dh.CustomerID = MaKH;
                db.Orders.Add(dh);
                db.SaveChanges();
                double TriGia = 0;
                foreach (var item in gioHangModels)
                {
                    OrderDetails ctdh = new OrderDetails();
                    var sp = db.Products.Find(item.Item.ProductID);
                    ctdh.ProductID = sp.ProductID;
                    ctdh.OrderID = MaDH;
                    ctdh.Quantity = item.Quantity;
                    ctdh.UnitPrice = item.Item.UnitPrice;
                    ctdh.TotalPrice = item.Price;
                    TriGia += (double)ctdh.TotalPrice;
                    db.OrderDetails.Add(ctdh);
                    db.SaveChanges();
                }
                var donDatHang = db.Orders.First(g => g.OrderID == MaDH);
                donDatHang.TotalAmount = TriGia;
                db.SaveChanges();
            }
            ViewBag.SDT = SDT;
            ViewBag.MaDH = MaDH;
            return View();
        }
        public ActionResult Dashboard()
        {
            var DonDatHang = db.Orders.OrderByDescending(x => x.Status);
            return View(DonDatHang.ToList());
        }
        public ActionResult DuyetDon(string id)
        {
            Orders dh = db.Orders.Find(id);
            if (dh.Status == "Đang xử lý")
                dh.Status = "Đã nhận";
            else 
                dh.Status = "Đang giao hàng";
            db.SaveChanges();
            return RedirectToAction("Dashboard");
        }
        public ActionResult GetUserImage(string url)
        {
            using (var client = new WebClient())
            {
                var imageBytes = client.DownloadData(url);
                return File(imageBytes, "image/jpeg");
            }
        }

    }
}