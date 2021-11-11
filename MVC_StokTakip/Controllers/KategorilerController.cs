using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MVC_StokTakip.Models.Entity;
using MVC_StokTakip.MyModel;


namespace MVC_StokTakip.Controllers
{

    [Authorize(Roles ="A")]
    
    public class KategorilerController : Controller
    {

        MVC_StokTakipEntities db = new MVC_StokTakipEntities();
        public ActionResult Index()
        {
            var kategori = db.Kategoriler.ToList();
            return View(kategori);
        }
        [HttpGet]
        public ActionResult KategoriEkle()
        {
            return View();
        }
        //[HttpPost,ValidateAntiForgeryToken]
       
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult KategoriEkle(Kategoriler kat)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }

            //var model = new Kategoriler();
            //model.ID = kat.ID;
            //model.Kategori = kat.Kategori;
            //model.Aciklama = kat.Aciklama;

            db.Kategoriler.Add(kat);
            db.SaveChanges();
            //System.Threading.Thread.Sleep(1000);
            return RedirectToAction("Index");
        }
        public ActionResult KategoriGetir(int Id)
        {

            var ilgilimodel = db.Kategoriler.Find(Id);//İlgili kaydı bul
            MyKategoriler mymodel = new MyKategoriler();
            mymodel.ID = ilgilimodel.ID;
            mymodel.Kategori = ilgilimodel.Kategori;
            mymodel.Aciklama = ilgilimodel.Aciklama;
            if (mymodel == null)//Böyle bir kayıt yok ise          
                return HttpNotFound();//hata döndür

            return View(mymodel);

        }
        [ValidateAntiForgeryToken]
        public ActionResult KategoriGuncelle(Kategoriler p)
        {
            if (!ModelState.IsValid)
            {
                return View("KategoriGetir");
            }
            //Birinci Yöntem
            //var deger = db.TBLKATEGORI.Where(x => x.ID == p.ID).FirstOrDefault();
            //deger.AD = p.AD;
            //db.SaveChanges();
            //return RedirectToAction("Index");

            //İkinci Yöntem
            //var ktg = db.Kategoriler.Find(p.ID);
            //ktg.Kategori = p.Kategori.ToString();
            //ktg.Aciklama = p.Aciklama.ToString();
            //db.SaveChanges();
            //return RedirectToAction("Index");

            //Üçüncü Yöntem
            db.Entry(p).State = System.Data.Entity.EntityState.Modified;
            db.SaveChanges();
            return RedirectToAction("Index");


        }

        //Modalla silme----------------------
        public ActionResult KategorilerSil(int id)
        {
            var silinecekKategori = db.Kategoriler.Find(id);
            
            if (silinecekKategori == null)//Böyle bir kayıt yok ise          
                return HttpNotFound();//hata döndür

            db.Kategoriler.Remove(silinecekKategori);
            db.SaveChanges();
            return RedirectToAction("Index");

        }
        //------------------------------------

        public ActionResult SilBilgiGetir(int Id)
        {
            var model = db.Kategoriler.Find(Id);

            MyKategoriler mymodel = new MyKategoriler();
            mymodel.ID = model.ID;
            mymodel.Kategori = model.Kategori;
            mymodel.Aciklama = model.Aciklama;

            if (mymodel == null)
                return HttpNotFound();

            return View("SilBilgiGetir", mymodel);
        }
        public ActionResult Sil(Kategoriler p)
        {
            db.Entry(p).State = System.Data.Entity.EntityState.Deleted;
            //var silinecekKategori = db.Kategoriler.Find(p.ID);
            //db.Kategoriler.Remove(silinecekKategori);

            db.SaveChanges();
            return RedirectToAction("Index");
        }
        //Seçilen Kategoriye ait Markalar
        public ActionResult Markalar(int Id)
        {
            var model = db.Markalar.Where(x => x.Kategoriler.ID == Id).ToList();
            var Kategori = db.Kategoriler.Where(x => x.ID ==Id ).Select(x=>x.Kategori).FirstOrDefault();
            ViewBag.ViewKategori = Kategori;

            return View(model);
        }

        //Seçilen Kategoriye ait Ürünler
        public ActionResult Urunler(int Id)
        {
            var model = db.Urunler.Where(x => x.Kategoriler.ID == Id).ToList();
            var Kategori = db.Kategoriler.Where(x => x.ID == Id).Select(x => x.Kategori).FirstOrDefault();
            ViewBag.ViewKategori = Kategori;

            return View(model);
        }



    }
}