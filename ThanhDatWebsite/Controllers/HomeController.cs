using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Security.Principal;
using System.Web;
using System.Web.Mvc;
using ThanhDatWebsite.Models;
using MailKit.Net.Smtp;
using MailKit;
using MimeKit;
using FluentEmail.Core;
using System.Net.Mail;
using System.Threading.Tasks;
using System.Web.Helpers;
using FluentEmail.Smtp;
using Org.BouncyCastle.Asn1.Ocsp;

namespace ThanhDatWebsite.Controllers
{
    public class HomeController : Controller
    {
        private thanhdatEntities1 db = new thanhdatEntities1();
        private IEmailSender _emailSender;
        public ActionResult Index()
        {  
            var sanPham2 = db.Products.OrderBy(x => x.CategoryID);
            var banner = db.BannerImage.OrderBy(x => x.Id).ToList();
            ViewBag.banner = banner;
            return View(sanPham2.ToList());
        }
        public ActionResult TimKiem (FormCollection form)
        {
            string NoiDung = form["txtTimKiem"];
            if (string.IsNullOrEmpty(NoiDung))
            {
                var allSanPham = from c in db.Products
                                 select c;
                return View(allSanPham.ToList());
            }
            var sanPham = from c in db.Products
                          where c.ProductName.Contains(NoiDung)
                          select c;
            ViewBag.msg = string.Empty;
            if (sanPham == null)
                ViewBag.msg = "Không tìm thấy sản phẩm";
            var banner = db.BannerImage.OrderBy(x => x.Id).ToList();
            ViewBag.banner = banner;
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
        public ActionResult DangNhapThuong(FormCollection form)
        {
            string TK = form["TenTK"];
            string MK = form["MK"];
            string _customerid = "";
            string Email = db.Accounts.Where(x => x.Email == TK || x.UserName == TK).Select(x => x.Email).FirstOrDefault();
            string AccountID = db.Accounts.Where(x => x.Email == TK || x.UserName == TK).Select(x => x.AccountID).FirstOrDefault();
            string Pass = db.Accounts.Where(x => x.PasswordHash == MK).Select(x => x.PasswordHash).FirstOrDefault();
            string Loai = db.Accounts.Where(x => x.Email == Email).Select(x => x.Role).FirstOrDefault();
            if (Email != null && Pass != null)
                if (Loai == "ad" || Loai == "emp")
                    return RedirectToAction("Dashboard", "Home");
                else
                {
                    _customerid = db.Customers.Where(c => c.AccountID == AccountID).Select(c => c.CustomerID).FirstOrDefault();
                    Session["cusid"] = _customerid;
                    var phoneNumber1 = db.Customers.Where(c => c.CustomerID == _customerid).Select(c => c.Phone).FirstOrDefault();
                    if (phoneNumber1 == null)
                    {
                        Session["isNew"] = "true";
                    }
                    else Session["isNew"] = "false";
                }
            else
            {
                ViewBag.msg = "Sai tài khoản hoặc mật khẩu";
                return RedirectToAction("DangNhap", "Home");
            }
            

            return RedirectToAction("Index", "Home");
        }
        [HttpPost]
        public JsonResult Logout()
        {
            Session.Remove("cusid");
            return Json(new { success = true });
        }
        public ActionResult KiemTraUser(FormCollection form, UserModel model)
        {
            string TK = form["TenTK"];
            string MK = form["MK"];
            string _customerid="";
            string Email = db.Accounts.Where(x => x.Email == TK || x.UserName==TK).Select(x => x.Email).FirstOrDefault();
            string AccountID = db.Accounts.Where(x => x.Email == TK || x.UserName == TK).Select(x => x.AccountID).FirstOrDefault();
            string Pass = db.Accounts.Where(x => x.PasswordHash == MK).Select(x => x.PasswordHash).FirstOrDefault();
            string Loai = db.Accounts.Where(x => x.Email == Email).Select(x => x.Role).FirstOrDefault();
            if (Email != null && Pass != null)
                if (Loai == "ad" || Loai == "emp")
                    return RedirectToAction("Dashboard", "Home");
                else
                {
                    _customerid = db.Customers.Where(c => c.AccountID == AccountID).Select(c=>c.CustomerID).FirstOrDefault();
                    Session["cusid"] = _customerid;
                    var phoneNumber1 = db.Customers.Where(c => c.Email == model.Email).Select(c => c.Phone).FirstOrDefault();
                    if (phoneNumber1 == null)
                    {

                        return Json(new { success = true, customerid = _customerid, isNew = true });
                    }
                    return Json(new { success = true, customerid = _customerid, isNew = false });
                }

            ViewBag.msg = "Sai tài khoản hoặc mật khẩu";

            if (model == null || string.IsNullOrEmpty(model.Username) || string.IsNullOrEmpty(model.Email))
            {
                return Json(new { success = false, message = "Invalid data" });
            }
            if (IdIncrementer.isExist(model.Email, "Customers"))
            {
                var cusID = db.Customers.Where(c=>c.Email == model.Email).Select(c=>c.CustomerID).FirstOrDefault();
                var phoneNumber = db.Customers.Where(c=>c.Email == model.Email).Select(c=>c.Phone).FirstOrDefault();
                if (phoneNumber==null)
                {
                    return Json(new { success = true, customerid = cusID, isNew = true });
                }
                return Json(new { success = true, customerid = cusID, isNew = false });
            }
            using (var context = new thanhdatEntities1())
            {
                // Tạo đối tượng Account từ payload
                var newAccount = new Accounts
                {
                    AccountID = IdIncrementer.idincreament("Accounts"),
                    UserName = model.Email,
                    Email = model.Email,
                    PasswordHash = "google", // Có thể set giá trị tùy theo logic của bạn
                    Role = "cus"
                };
                var newCustomer = new Customers
                {
                    CustomerID = IdIncrementer.idincreament("Customers"),
                    AccountID = newAccount.AccountID,
                    FullName = model.Username,
                    Email = model.Email,
                };
                // Thêm đối tượng vào DbSet
                context.Customers.Add(newCustomer);
                context.Accounts.Add(newAccount);
                context.SaveChanges();
                return Json(new { success = true, customerid = newCustomer.CustomerID });
            }
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
        public async Task<ActionResult> DatHang(FormCollection form)
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
            string DiaChi = form["txtSoNha"];
            string Email = form["Email"];
            double TriGia = 0;
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
                kh.Email = Email;
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
            try
            {
                string emailBody = $@"
    <html>
    <head>
        <style>
            body {{
                font-family: Arial, sans-serif;
                line-height: 1.6;
                background-color: #f9f9f9;
                color: #333;
                padding: 20px;
            }}
            .email-container {{
                max-width: 600px;
                margin: 0 auto;
                background: #ffffff;
                border: 1px solid #ddd;
                border-radius: 8px;
                padding: 20px;
                box-shadow: 0 4px 8px rgba(0, 0, 0, 0.1);
            }}
            .header {{
                background-color: #4CAF50;
                color: white;
                padding: 10px;
                border-radius: 8px 8px 0 0;
                text-align: center;
            }}
            .header h2 {{
                margin: 0;
            }}
            .details {{
                margin: 20px 0;
            }}
            .details p {{
                margin: 5px 0;
            }}
            .footer {{
                text-align: center;
                font-size: 12px;
                color: #777;
                margin-top: 20px;
            }}
            .footer a {{
                color: #4CAF50;
                text-decoration: none;
            }}
        </style>
    </head>
    <body>
        <div class='email-container'>
            <div class='header'>
                <h2>Xác nhận đơn hàng</h2>
            </div>
            <div class='details'>
                <p><strong>Mã đơn hàng:</strong> {MaDH}</p>
                <p><strong>Ngày đặt:</strong> {NgayDH}</p>
                <p><strong>Tên khách hàng:</strong> {TenKH}</p>
                <p><strong>Địa chỉ:</strong> {DiaChi}</p>
                <p><strong>Số điện thoại:</strong> {SDT}</p>
                <p>-------------------------------</p>
                <p style='color:red'><strong>Tổng giá trị hóa đơn: {TriGia}</strong></p>
            </div>
            <div>
                <p>Cảm ơn bạn đã tin tưởng và mua hàng tại <strong>Thành Đạt</strong>! Chúng tôi sẽ liên hệ với bạn sớm để xác nhận thông tin.</p>
            </div>
            <div class='footer'>
                <p>Đây là email tự động, vui lòng không trả lời trực tiếp.</p>
                <p>Liên hệ: <a href='mailto:support@thanhdat.com'>support@thanhdat.com</a> | Hotline: 19006868</p>
            </div>
        </div>
    </body>
    </html>";
                IEmailSender emailSender = new EmailSender();
                _emailSender = emailSender;
                await _emailSender.SendEmailAsync(Email, "Đặt hàng thành công", emailBody);

            }
            catch (Exception ex)
            {
                throw ex;
            }
            ViewBag.Email = Email;
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
        public ActionResult AddToWishlist(string id, string cusid)
        {
            if (string.IsNullOrEmpty(cusid))
            {
                return RedirectToAction("DangNhap", "Home");
            }
            else
            {
                var Wishlist = new Wishlist
                {
                    ProductID = id,
                    CustomerID = cusid,
                };
                db.Wishlist.Add(Wishlist);
                db.SaveChanges();
            }
            return RedirectToAction("Index", "Home");
        }
        public ActionResult RemoveFromWishlist(string id, string cusid)
        {
            if (string.IsNullOrEmpty(cusid))
            {
                return RedirectToAction("DangNhap", "Home");
            }
            else
            {
                var Wishlist = db.Wishlist.FirstOrDefault(w => w.ProductID == id && w.CustomerID == cusid);
                db.Wishlist.Remove(Wishlist);
                db.SaveChanges();
            }
            return RedirectToAction("Index", "Home");
        }
        public ActionResult isLiked(string id, string cusid)
        {
            var Wishlist = db.Wishlist.FirstOrDefault(w => w.ProductID == id && w.CustomerID == cusid);
            if (Wishlist != null)
            {
                return Json(new { isLiked = true },JsonRequestBehavior.AllowGet);
            }else 
                return Json(new {isLiked = false}, JsonRequestBehavior.AllowGet);
        }

    }
}