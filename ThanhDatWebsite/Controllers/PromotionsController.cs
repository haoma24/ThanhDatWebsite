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
    public class PromotionsController : Controller
    {
        private thanhdatEntities1 db = new thanhdatEntities1();

        // GET: Promotions
        public ActionResult Index()
        {
            return View(db.Promotions.ToList());
        }

        // GET: Promotions/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Promotions promotions = db.Promotions.Find(id);
            if (promotions == null)
            {
                return HttpNotFound();
            }
            return View(promotions);
        }

        // GET: Promotions/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Promotions/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "PromotionID,PromotionName,StartDate,EndDate,DiscountPercentage")] Promotions promotions)
        {
            if (ModelState.IsValid)
            {
                db.Promotions.Add(promotions);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(promotions);
        }

        // GET: Promotions/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Promotions promotions = db.Promotions.Find(id);
            if (promotions == null)
            {
                return HttpNotFound();
            }
            return View(promotions);
        }

        // POST: Promotions/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "PromotionID,PromotionName,StartDate,EndDate,DiscountPercentage")] Promotions promotions)
        {
            if (ModelState.IsValid)
            {
                db.Entry(promotions).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(promotions);
        }

        // GET: Promotions/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Promotions promotions = db.Promotions.Find(id);
            if (promotions == null)
            {
                return HttpNotFound();
            }
            return View(promotions);
        }

        // POST: Promotions/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            Promotions promotions = db.Promotions.Find(id);
            db.Promotions.Remove(promotions);
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
