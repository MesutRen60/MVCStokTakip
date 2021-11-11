using MVC_StokTakip.Models.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MVC_StokTakip.Controllers
{
    public class ChartController : Controller
    {
        MVC_StokTakipEntities db = new MVC_StokTakipEntities();
        // GET: Chart
        public ActionResult Index()
        {
            return View();
        }
        public JsonResult GetDataforChart()
        {

            var query = db.Urunler.GroupBy(k => k.KategoriID).
                         Select(g => new
                         {
                             g.Key,
                             sayi = g.Count()  // now there is a 'bp' in scope
                         }).ToList();


            return Json(new { data = query }, JsonRequestBehavior.AllowGet);
        }
    }
}