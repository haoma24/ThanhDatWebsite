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
    public class AdminAccountsController : Controller
    {
        private thanhdatEntities1 db = new thanhdatEntities1();

        // GET: AdminAccounts
        public ActionResult Index()
        {
            return View(db.Accounts.ToList());
        }

        // GET: AdminAccounts/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Accounts adminAccounts = db.Accounts.Find(id);
            if (adminAccounts == null)
            {
                return HttpNotFound();
            }
            return View(adminAccounts);
        }

        // GET: AdminAccounts/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: AdminAccounts/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "AccountID,Email,Phone,Password,Role")] Accounts adminAccounts)
        {
            if (ModelState.IsValid)
            {
                db.Accounts.Add(adminAccounts);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(adminAccounts);
        }

        // GET: AdminAccounts/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Accounts adminAccounts = db.Accounts.Find(id);
            if (adminAccounts == null)
            {
                return HttpNotFound();
            }
            return View(adminAccounts);
        }

        // POST: AdminAccounts/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "AccountID,Email,Phone,Password,Role")] Accounts adminAccounts)
        {
            if (ModelState.IsValid)
            {
                db.Entry(adminAccounts).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(adminAccounts);
        }

        // GET: AdminAccounts/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Accounts adminAccounts = db.Accounts.Find(id);
            if (adminAccounts == null)
            {
                return HttpNotFound();
            }
            return View(adminAccounts);
        }

        // POST: AdminAccounts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            Accounts adminAccounts = db.Accounts.Find(id);
            db.Accounts.Remove(adminAccounts);
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
