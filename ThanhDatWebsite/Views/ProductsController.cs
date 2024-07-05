using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Validation;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using ThanhDatWebsite;
using ThanhDatWebsite.Controllers;

namespace ThanhDatWebsite.Views
{
    public class ProductsController : Controller
    {
        private thanhdatEntities db = new thanhdatEntities();

        // GET: Products
        public ActionResult Index()
        {
            var products = db.Products.Include(p => p.Categories);
            return View(products.ToList());
        }

        // GET: Products/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Products products = db.Products.Find(id);
            if (products == null)
            {
                return HttpNotFound();
            }
            return View(products);
        }

        // GET: Products/Create
        public ActionResult Create()
        {
            ViewBag.ProductID = IdIncrementer.idincreament("Products");
            ViewBag.CategoryID = new SelectList(db.Categories, "CategoryID", "CategoryName");
            return View();
        }

        // POST: Products/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ProductID,ProductName,UnitPrice,Discontinued,CategoryID,Image")] Products products)
        {
            if (products.ProductName == null || products.UnitPrice == null || products.ProductID == null || products.Image == null)
            {
                ViewBag.err = "Vui lòng nhập đầy đủ các trường";
                ViewBag.ProductID = IdIncrementer.idincreament("Products");
                ViewBag.CategoryID = new SelectList(db.Categories, "CategoryID", "CategoryName");
                return View("Create");
            }
            products.Image = "..\\\\Media\\\\" + products.Image;
            if (ModelState.IsValid)
            {
                try
                {
                    db.Products.Add(products);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
                catch (DbEntityValidationException e)
                {
                    foreach (var eve in e.EntityValidationErrors)
                    {
                        Console.WriteLine("Entity of type \"{0}\" in state \"{1}\" has the following validation errors:",
                            eve.Entry.Entity.GetType().Name, eve.Entry.State);
                        foreach (var ve in eve.ValidationErrors)
                        {
                            Console.WriteLine("- Property: \"{0}\", Error: \"{1}\"",
                                ve.PropertyName, ve.ErrorMessage);
                        }
                    }
                    throw;
                }

            }

            //ViewBag.MaLoaiSP = new SelectList(db.LoaiSP, "MaLoaiSP", "TenLoaiSP", products.MaLoaiSP);
            //ViewBag.MaNCC = new SelectList(db.NhaCungCap, "MaNCC", "TenNCC", products.MaNCC);
            return View(products);
        }

        // GET: Products/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Products products = db.Products.Find(id);
            if (products == null)
            {
                return HttpNotFound();
            }
            ViewBag.CategoryID = new SelectList(db.Categories, "CategoryID", "CategoryName", products.CategoryID);
            return View(products);
        }

        // POST: Products/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ProductID,ProductName,UnitPrice,Discontinued,CategoryID,Image")] Products products)
        {
            if (products.ProductName == null || products.UnitPrice == null || products.ProductID == null || products.Image == null)
            {
                ViewBag.err = "Vui lòng nhập đầy đủ các trường";
                ViewBag.ProductID = IdIncrementer.idincreament("Products");
                ViewBag.CategoryID = new SelectList(db.Categories, "CategoryID", "CategoryName");
                return View("Create");
            }
            products.Image = "..\\\\Media\\\\" + products.Image;
            if (ModelState.IsValid)
            {
                db.Entry(products).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.CategoryID = new SelectList(db.Categories, "CategoryID", "CategoryName", products.CategoryID);
            return View(products);
        }

        // GET: Products/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Products products = db.Products.Find(id);
            if (products == null)
            {
                return HttpNotFound();
            }
            return View(products);
        }

        // POST: Products/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            Products products = db.Products.Find(id);
            db.Products.Remove(products);
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
