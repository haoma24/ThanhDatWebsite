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
    public class DeliveryMethodsController : Controller
    {
        private thanhdatEntities1 db = new thanhdatEntities1();

        // GET: DeliveryMethods
        public ActionResult Index()
        {
            return View(db.DeliveryMethod.ToList());
        }

        // GET: DeliveryMethods/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DeliveryMethod deliveryMethod = db.DeliveryMethod.Find(id);
            if (deliveryMethod == null)
            {
                return HttpNotFound();
            }
            return View(deliveryMethod);
        }

        // GET: DeliveryMethods/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: DeliveryMethods/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "DeliveryMethodID,DeliveryName")] DeliveryMethod deliveryMethod)
        {
            if (ModelState.IsValid)
            {
                db.DeliveryMethod.Add(deliveryMethod);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(deliveryMethod);
        }

        // GET: DeliveryMethods/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DeliveryMethod deliveryMethod = db.DeliveryMethod.Find(id);
            if (deliveryMethod == null)
            {
                return HttpNotFound();
            }
            return View(deliveryMethod);
        }

        // POST: DeliveryMethods/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "DeliveryMethodID,DeliveryName")] DeliveryMethod deliveryMethod)
        {
            if (ModelState.IsValid)
            {
                db.Entry(deliveryMethod).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(deliveryMethod);
        }

        // GET: DeliveryMethods/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DeliveryMethod deliveryMethod = db.DeliveryMethod.Find(id);
            if (deliveryMethod == null)
            {
                return HttpNotFound();
            }
            return View(deliveryMethod);
        }

        // POST: DeliveryMethods/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            DeliveryMethod deliveryMethod = db.DeliveryMethod.Find(id);
            db.DeliveryMethod.Remove(deliveryMethod);
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
