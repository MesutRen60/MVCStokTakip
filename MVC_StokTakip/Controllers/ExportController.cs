using MVC_StokTakip.Models.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MVC_StokTakip.Controllers
{
    public class ExportController : Controller
    {
        MVC_StokTakipEntities db = new MVC_StokTakipEntities();
        // GET: Export
        public ActionResult Index()
        {
           
            var model = db.Urunler.ToList();
            return View(model);
        }
    }
}