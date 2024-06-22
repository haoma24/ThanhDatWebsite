using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace ThanhDatWebsite.Controllers
{
    public class IdIncrementer
    {
        public static string idincreament(string TenBang)
        {
            using (var context = new thanhdatEntities())
            {
                string newId = "Error";
                if (TenBang == "Orders")
                {
                    // Lấy tất cả các Id từ bảng Orders và bỏ hai ký tự đầu, sau đó chuyển phần còn lại thành số nguyên
                    var allIds = context.Orders
                                .Select(o => o.OrderID)
                                .ToList();

                    // Chuyển đổi các Id và tìm giá trị lớn nhất trong bộ nhớ
                    int maxNumericId = allIds
                                        .Select(id => int.Parse(id.Substring(2)))
                                        .DefaultIfEmpty(0)
                                        .Max();

                    // Tăng giá trị lớn nhất lên 1
                    int newNumericId = maxNumericId + 1;

                    // Tạo Id mới
                    newId = "DH" + newNumericId.ToString("D4");
                }
                if (TenBang == "Customers")
                {
                    // Lấy tất cả các Id từ bảng Orders và bỏ hai ký tự đầu, sau đó chuyển phần còn lại thành số nguyên
                    var allIds = context.Orders
                                .Select(o => o.CustomerID)
                                .ToList();

                    // Chuyển đổi các Id và tìm giá trị lớn nhất trong bộ nhớ
                    int maxNumericId = allIds
                                        .Select(id => int.Parse(id.Substring(2)))
                                        .DefaultIfEmpty(0)
                                        .Max();

                    // Tăng giá trị lớn nhất lên 1
                    int newNumericId = maxNumericId + 1;

                    // Tạo Id mới
                    newId = "KH" + newNumericId.ToString("D4");
                }
                if (TenBang == "AdminAccounts")
                {
                    // Lấy tất cả các Id từ bảng Orders và bỏ hai ký tự đầu, sau đó chuyển phần còn lại thành số nguyên
                    var allIds = context.AdminAccounts
                                .Select(o => o.AccountID)
                                .ToList();

                    // Chuyển đổi các Id và tìm giá trị lớn nhất trong bộ nhớ
                    int maxNumericId = allIds
                                        .Select(id => int.Parse(id.Substring(2)))
                                        .DefaultIfEmpty(0)
                                        .Max();

                    // Tăng giá trị lớn nhất lên 1
                    int newNumericId = maxNumericId + 1;

                    // Tạo Id mới
                    newId = "TK" + newNumericId.ToString("D4");
                }
                return newId;

            }
        }
    }
}