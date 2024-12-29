using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using ThanhDatWebsite;

namespace ThanhDatWebsite.Controllers
{
    public class BannerImagesController : Controller
    {
        private thanhdatEntities1 db = new thanhdatEntities1();

        // GET: BannerImages
        public ActionResult Index()
        {
            return View(db.BannerImage.ToList());
        }

        // GET: BannerImages/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            BannerImage bannerImage = db.BannerImage.Find(id);
            if (bannerImage == null)
            {
                return HttpNotFound();
            }
            return View(bannerImage);
        }

        // GET: BannerImages/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: BannerImages/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,ImageUrl")] BannerImage bannerImage)
        {
            if (ModelState.IsValid)
            {
                
                bannerImage.ImageUrl = @"\Media\" +bannerImage.ImageUrl;
                db.BannerImage.Add(bannerImage);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(bannerImage);
        }

        // GET: BannerImages/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            BannerImage bannerImage = db.BannerImage.Find(id);
            if (bannerImage == null)
            {
                return HttpNotFound();
            }
            return View(bannerImage);
        }

        // POST: BannerImages/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,ImageUrl")] BannerImage bannerImage)
        {
            if (ModelState.IsValid)
            {
                db.Entry(bannerImage).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(bannerImage);
        }

        // GET: BannerImages/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            BannerImage bannerImage = db.BannerImage.Find(id);
            if (bannerImage == null)
            {
                return HttpNotFound();
            }
            return View(bannerImage);
        }

        // POST: BannerImages/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            BannerImage bannerImage = db.BannerImage.Find(id);
            db.BannerImage.Remove(bannerImage);
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
