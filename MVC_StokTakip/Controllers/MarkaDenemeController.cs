using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MVC_StokTakip.Models.Entity;

namespace MVC_StokTakip.Controllers
{
    
    public class MarkaDenemeController : Controller
    {
        MVC_StokTakipEntities db = new MVC_StokTakipEntities();
        public ActionResult Index()
        {
            var markamodel = db.Markalar.ToList();
            return View(markamodel);
        }

        [HttpGet]
        public ActionResult MarkaEkle()
        {
            ViewBag.KategoriID = new SelectList(db.Kategoriler, "ID", "Kategori");
            return View();
        }

        [HttpPost]
        public ActionResult MarkaEkle(Markalar p)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.KategoriID = new SelectList(db.Kategoriler, "ID", "Kategori");
                return View();
            }

            db.Entry(p).State = System.Data.Entity.EntityState.Added;
            db.SaveChanges();

            return RedirectToAction("Index");
        }

    }
}