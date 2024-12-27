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
    public class ReceiptsController : Controller
    {
        private thanhdatEntities1 db = new thanhdatEntities1();

        // GET: Receipts
        public ActionResult Index()
        {
            var receipts = db.Receipts.Include(r => r.Branches);
            return View(receipts.ToList());
        }

        // GET: Receipts/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Receipts receipts = db.Receipts.Find(id);
            if (receipts == null)
            {
                return HttpNotFound();
            }
            return View(receipts);
        }

        // GET: Receipts/Create
        public ActionResult Create()
        {
            ViewBag.BranchID = new SelectList(db.Branches, "BranchID", "BranchName");
            return View();
        }

        // POST: Receipts/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ReceiptID,BranchID,ReceiptDate")] Receipts receipts)
        {
            if (ModelState.IsValid)
            {
                db.Receipts.Add(receipts);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.BranchID = new SelectList(db.Branches, "BranchID", "BranchName", receipts.BranchID);
            return View(receipts);
        }

        // GET: Receipts/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Receipts receipts = db.Receipts.Find(id);
            if (receipts == null)
            {
                return HttpNotFound();
            }
            ViewBag.BranchID = new SelectList(db.Branches, "BranchID", "BranchName", receipts.BranchID);
            return View(receipts);
        }

        // POST: Receipts/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ReceiptID,BranchID,ReceiptDate")] Receipts receipts)
        {
            if (ModelState.IsValid)
            {
                db.Entry(receipts).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.BranchID = new SelectList(db.Branches, "BranchID", "BranchName", receipts.BranchID);
            return View(receipts);
        }

        // GET: Receipts/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Receipts receipts = db.Receipts.Find(id);
            if (receipts == null)
            {
                return HttpNotFound();
            }
            return View(receipts);
        }

        // POST: Receipts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            Receipts receipts = db.Receipts.Find(id);
            db.Receipts.Remove(receipts);
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
