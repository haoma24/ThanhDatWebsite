using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ThanhDatWebsite.Controllers
{
    public class BaoCaoController : Controller
    {
        // GET: BaoCao
        public ActionResult DoanhThu()
        {
            ViewBag.viewName = "BaoCaoDT";
            return View();
        }

        public ActionResult BaoCaoDT(FormCollection form)
        {
            //Sau khi chon ngay bao cao
            if (form == null)
                return RedirectToAction("DoanhThu");
            DateTime fromDate = DateTime.Parse(form["fromDate"]);
            DateTime toDate = DateTime.Parse(form["toDate"]);

            //Viewbag hien thi tren title
            ViewBag.fromDate = fromDate.ToString("MM/dd/yyyy");
            ViewBag.toDate = toDate.ToString("MM/dd/yyyy");
            ViewBag.Name = "BaoCaoDT";
            return View();
        }
    }
}