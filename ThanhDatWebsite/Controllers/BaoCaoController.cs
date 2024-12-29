using Microsoft.Reporting.WebForms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Web;
using System.Web.Mvc;
using System.Web.UI.WebControls;
using System.Xml.Linq;


namespace ThanhDatWebsite.Controllers
{
    public class BaoCaoController : Controller
    {
        private ReportViewer reportViewer;
        private string url = "http://localhost/BaoCao/";
        // GET: BaoCao
        public BaoCaoController()
        {
            reportViewer = new ReportViewer
            {
                ProcessingMode = ProcessingMode.Remote,
                SizeToReportContent = true,
                AsyncRendering = true,
                Width = Unit.Percentage(100),
                Height = Unit.Percentage(100)
            };
            reportViewer.ServerReport.ReportServerUrl = new Uri(url);
        }
        public ActionResult DoanhThu()
        {
            ViewBag.viewName = "BaoCaoDT";
            return View();
        }
        public ActionResult BaoCaoTK()
        {
            
            //Viewbag hien thi tren title
            
            ViewBag.Name = "BaoCaoTK";
            reportViewer.ServerReport.ReportPath = "/DemoReport/BaoCaoTonKho";
            reportViewer.ServerReport.Refresh();
            ViewBag.ReportViewer = reportViewer;
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
            reportViewer.ServerReport.ReportPath = "/DemoReport/BaoCaoDT";
            ReportParameter[] parameters = new ReportParameter[2];
            parameters[0] = new ReportParameter("FromDate", fromDate.ToString("MM/dd/yyyy"), true);
            parameters[1] = new ReportParameter("ToDate", toDate.ToString("MM/dd/yyyy"), true);
            reportViewer.ServerReport.SetParameters(parameters);
            reportViewer.ServerReport.Refresh();
            ViewBag.ReportViewer = reportViewer;
            return View();
        }
    }
}