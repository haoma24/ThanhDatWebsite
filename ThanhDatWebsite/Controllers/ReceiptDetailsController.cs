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
    public class ReceiptDetailsController : Controller
    {
        private thanhdatEntities db = new thanhdatEntities();

        // GET: ReceiptDetails
        public ActionResult Index()
        {
            var receiptDetails = db.ReceiptDetails.Include(r => r.Products).Include(r => r.Receipts);
            return View(receiptDetails.ToList());
        }

        // GET: ReceiptDetails/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ReceiptDetails receiptDetails = db.ReceiptDetails.Find(id);
            if (receiptDetails == null)
            {
                return HttpNotFound();
            }
            return View(receiptDetails);
        }

        // GET: ReceiptDetails/Create
        public ActionResult Create()
        {
            ViewBag.ProductID = new SelectList(db.Products, "ProductID", "ProductName");
            ViewBag.ReceiptID = new SelectList(db.Receipts, "ReceiptID", "ReceiptID");
            ViewBag.BranchID = new SelectList(db.Branches, "BranchID", "BranchName");
            return View();
        }

        // POST: ReceiptDetails/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ReceiptID,ProductID,Quantity")] ReceiptDetails receiptDetails)
        {
            if (ModelState.IsValid)
            {
                db.ReceiptDetails.Add(receiptDetails);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.ProductID = new SelectList(db.Products, "ProductID", "ProductName", receiptDetails.ProductID);
            ViewBag.ReceiptID = new SelectList(db.Receipts, "ReceiptID", "BranchID", receiptDetails.ReceiptID);
            return View(receiptDetails);
        }

        // GET: ReceiptDetails/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ReceiptDetails receiptDetails = db.ReceiptDetails.Find(id);
            if (receiptDetails == null)
            {
                return HttpNotFound();
            }
            ViewBag.ProductID = new SelectList(db.Products, "ProductID", "ProductName", receiptDetails.ProductID);
            ViewBag.ReceiptID = new SelectList(db.Receipts, "ReceiptID", "BranchID", receiptDetails.ReceiptID);
            return View(receiptDetails);
        }

        // POST: ReceiptDetails/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ReceiptID,ProductID,Quantity")] ReceiptDetails receiptDetails)
        {
            if (ModelState.IsValid)
            {
                db.Entry(receiptDetails).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.ProductID = new SelectList(db.Products, "ProductID", "ProductName", receiptDetails.ProductID);
            ViewBag.ReceiptID = new SelectList(db.Receipts, "ReceiptID", "BranchID", receiptDetails.ReceiptID);
            return View(receiptDetails);
        }

        // GET: ReceiptDetails/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ReceiptDetails receiptDetails = db.ReceiptDetails.Find(id);
            if (receiptDetails == null)
            {
                return HttpNotFound();
            }
            return View(receiptDetails);
        }

        // POST: ReceiptDetails/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            ReceiptDetails receiptDetails = db.ReceiptDetails.Find(id);
            db.ReceiptDetails.Remove(receiptDetails);
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
