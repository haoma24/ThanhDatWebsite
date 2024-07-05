using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using ThanhDatWebsite;

namespace ThanhDatWebsite.Controllers
{
    public class OrderDetailsController : Controller
    {
        private thanhdatEntities db = new thanhdatEntities();

        // GET: OrderDetails
        public ActionResult Index()
        {
            var orderDetails = db.OrderDetails.OrderBy(x => x.OrderID).Include(o => o.Orders).Include(o => o.Products);
            return View(orderDetails.ToList());
        }

        // GET: OrderDetails/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            OrderDetails orderDetails = db.OrderDetails.Where(x => x.OrderID == id).First();
            if (orderDetails == null)
            {
                return HttpNotFound();
            }
            return View(orderDetails);
        }

        // GET: OrderDetails/Create
        public ActionResult Create()
        {
            ViewBag.OrderID = new SelectList(db.Orders, "OrderID", "CustomerID");
            ViewBag.ProductID = new SelectList(db.Products, "ProductID", "ProductName");
            return View();
        }

        // POST: OrderDetails/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "OrderID,ProductID,Quantity,UnitPrice,TotalPrice")] OrderDetails orderDetails)
        {
            if (ModelState.IsValid)
            {
                db.OrderDetails.Add(orderDetails);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.OrderID = new SelectList(db.Orders, "OrderID", "CustomerID", orderDetails.OrderID);
            ViewBag.ProductID = new SelectList(db.Products, "ProductID", "ProductName", orderDetails.ProductID);
            return View(orderDetails);
        }

        // GET: OrderDetails/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            OrderDetails orderDetails = db.OrderDetails.Where(x => x.OrderID == id).First();
            if (orderDetails == null)
            {
                return HttpNotFound();
            }
            ViewBag.OrderID = new SelectList(db.Orders, "OrderID", "CustomerID", orderDetails.OrderID);
            ViewBag.ProductID = new SelectList(db.Products, "ProductID", "ProductName", orderDetails.ProductID);
            return View(orderDetails);
        }

        // POST: OrderDetails/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "OrderID,ProductID,Quantity,UnitPrice,TotalPrice")] OrderDetails orderDetails)
        {
            if (ModelState.IsValid)
            {
                db.Entry(orderDetails).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.OrderID = new SelectList(db.Orders, "OrderID", "CustomerID", orderDetails.OrderID);
            ViewBag.ProductID = new SelectList(db.Products, "ProductID", "ProductName", orderDetails.ProductID);
            return View(orderDetails);
        }

        // GET: OrderDetails/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            OrderDetails orderDetails = db.OrderDetails.Where(x => x.OrderID == id).First();
            if (orderDetails == null)
            {
                return HttpNotFound();
            }
            return View(orderDetails);
        }

        // POST: OrderDetails/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            OrderDetails orderDetails = db.OrderDetails.Where(x => x.OrderID == id).First();
            db.OrderDetails.Remove(orderDetails);
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
