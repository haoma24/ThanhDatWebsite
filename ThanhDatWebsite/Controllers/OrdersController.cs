using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using ThanhDatWebsite;

namespace ThanhDatWebsite.Controllers
{
    public class OrdersController : Controller
    {
        private thanhdatEntities1 db = new thanhdatEntities1();

        // GET: Orders
        public ActionResult Index()
        {
            var orders = db.Orders.OrderByDescending(o => o.Status).Include(o => o.Branches).Include(o => o.Customers).Include(o => o.DeliveryMethod).Include(o => o.Payment);
            return View(orders.ToList());
        }

        // GET: Orders/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Orders orders = db.Orders.Find(id);
            if (orders == null)
            {
                return HttpNotFound();
            }
            return View(orders);
        }

        // GET: Orders/Create
        public ActionResult Create()
        {
            ViewBag.BranchID = new SelectList(db.Branches, "BranchID", "BranchName");
            ViewBag.CustomerID = new SelectList(db.Customers, "CustomerID", "FullName");
            ViewBag.DeliveryMethodID = new SelectList(db.DeliveryMethod, "DeliveryMethodID", "DeliveryName");
            ViewBag.PaymentID = new SelectList(db.Payment, "PaymentID", "Payment1");
            return View();
        }

        // POST: Orders/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "OrderID,CustomerID,BranchID,Name,Address,Phone,PaymentID,DeliveryMethodID,OrderDate,TotalAmount,Status")] Orders orders)
        {
            if (ModelState.IsValid)
            {
                db.Orders.Add(orders);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.BranchID = new SelectList(db.Branches, "BranchID", "BranchName", orders.BranchID);
            ViewBag.CustomerID = new SelectList(db.Customers, "CustomerID", "FullName", orders.CustomerID);
            ViewBag.DeliveryMethodID = new SelectList(db.DeliveryMethod, "DeliveryMethodID", "DeliveryName", orders.DeliveryMethodID);
            ViewBag.PaymentID = new SelectList(db.Payment, "PaymentID", "Payment1", orders.PaymentID);
            return View(orders);
        }

        // GET: Orders/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Orders orders = db.Orders.Find(id);
            if (orders == null)
            {
                return HttpNotFound();
            }
            ViewBag.BranchID = new SelectList(db.Branches, "BranchID", "BranchName", orders.BranchID);
            ViewBag.CustomerID = new SelectList(db.Customers, "CustomerID", "FullName", orders.CustomerID);
            ViewBag.DeliveryMethodID = new SelectList(db.DeliveryMethod, "DeliveryMethodID", "DeliveryName", orders.DeliveryMethodID);
            ViewBag.PaymentID = new SelectList(db.Payment, "PaymentID", "Payment1", orders.PaymentID);
            return View(orders);
        }

        // POST: Orders/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "OrderID,CustomerID,BranchID,Name,Address,Phone,PaymentID,DeliveryMethodID,OrderDate,TotalAmount,Status")] Orders orders)
        {
            if (ModelState.IsValid)
            {
                db.Entry(orders).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.BranchID = new SelectList(db.Branches, "BranchID", "BranchName", orders.BranchID);
            ViewBag.CustomerID = new SelectList(db.Customers, "CustomerID", "FullName", orders.CustomerID);
            ViewBag.DeliveryMethodID = new SelectList(db.DeliveryMethod, "DeliveryMethodID", "DeliveryName", orders.DeliveryMethodID);
            ViewBag.PaymentID = new SelectList(db.Payment, "PaymentID", "Payment1", orders.PaymentID);
            return View(orders);
        }

        // GET: Orders/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Orders orders = db.Orders.Find(id);
            if (orders == null)
            {
                return HttpNotFound();
            }
            return View(orders);
        }

        // POST: Orders/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            Orders orders = db.Orders.Find(id);
            db.Orders.Remove(orders);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
