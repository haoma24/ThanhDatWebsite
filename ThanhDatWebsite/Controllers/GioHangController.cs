using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ThanhDatWebsite.Models;

namespace ThanhDatWebsite.Controllers
{
    public class GioHangController : Controller
    {
        private thanhdatEntities1 de = new thanhdatEntities1();
        // GET: GioHang
        public ActionResult GioHang()
        {
            return View();
        }
        private int isExisting(string id)
        {
            List<GioHangModel> cart = (List<GioHangModel>)Session["cart"];
            for (int i = 0; i < cart.Count; i++)

                if (cart[i].Item.ProductID == id)
                    return i;
            return -1;

        }
        public ActionResult Delete(string id)
        {

            List<GioHangModel> cart = (List<GioHangModel>)Session["cart"];
            for (int i = 0; i < cart.Count; i++)
            {
                if (cart[i].Item.ProductID.Trim() == id)
                {
                    cart.RemoveAt(i);
                }
            }

            Session["cart"] = cart; // Update Session["cart"]

            return RedirectToAction("GioHang", "GioHang");
        }

        public ActionResult AddToCart(string id)
        {
            if (Session["cart"] == null)
            {
                List<GioHangModel> cart = new List<GioHangModel>();

                cart.Add(new GioHangModel(de.Products.Find(id), 1)); // Add 1 Product based on id provided

                Session["cart"] = cart; // Update Session["cart"]
            }
            else
            {
                List<GioHangModel> cart = (List<GioHangModel>)Session["cart"];

                for (int i = 0; i < cart.Count; i++)
                {
                    if (cart[i].Item.ProductID.Trim() == id)
                    {
                        cart[i].Quantity++;
                        cart[i].Price = (double)(cart[i].Item.UnitPrice * cart[i].Quantity);
                        goto kt;
                    }
                }
                cart.Add(new GioHangModel(de.Products.Find(id), 1));
            kt: Session["cart"] = cart; // Update Session["cart"]
            }

            return RedirectToAction("Index", "Home");
        }
        public ActionResult PlusItem(string id)
        {
            List<GioHangModel> cart = (List<GioHangModel>)Session["cart"];
            for (int i = 0; i < cart.Count; i++)
            {
                if (cart[i].Item.ProductID.Trim() == id)
                {
                    cart[i].Quantity++;
                    cart[i].Price = (double)(cart[i].Item.UnitPrice * cart[i].Quantity);
                    goto kt;
                }
            }
            cart.Add(new GioHangModel(de.Products.Find(id), 1));
        kt: Session["cart"] = cart; // Update Session["cart"]

            return RedirectToAction("GioHang", "GioHang");
        }

        public ActionResult MinusItem(string id)
        {
            List<GioHangModel> cart = (List<GioHangModel>)Session["cart"];
            for (int i = 0; i < cart.Count; i++)
            {
                if (cart[i].Item.ProductID.Trim() == id)
                {
                    if (cart[i].Quantity == 0)
                        goto kt;
                    cart[i].Quantity--;
                    cart[i].Price = (double)(cart[i].Item.UnitPrice * cart[i].Quantity);
                    goto kt2;
                }
            }
            cart.Add(new GioHangModel(de.Products.Find(id), 1));
        kt: Session["cart"] = null; // Update Session["cart"]

        kt2: return RedirectToAction("GioHang", "GioHang");
        }
    }
}