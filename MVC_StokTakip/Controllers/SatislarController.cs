using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MVC_StokTakip.Models.Entity;

namespace MVC_StokTakip.Controllers
{
    public class SatislarController : Controller
    {
        MVC_StokTakipEntities db = new MVC_StokTakipEntities();
        public ActionResult Index()
        {
            var model = db.Satislar.ToList();
            return View(model);

        }
        [HttpGet]
        public ActionResult SatinAl(int Id)
        {
            var model = db.Sepet.FirstOrDefault(x => x.ID == Id);

            return View(model);
        }

        [HttpPost]
        public ActionResult SatinAl(Sepet p)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var model = db.Sepet.FirstOrDefault(x => x.ID == p.ID);
                    var satis = new Satislar
                    {
                        UrunID = model.UrunID,
                        SpetID = model.ID,
                        KullaniciID = model.KullaniciID,                      
                        BarkodNo = model.Urunler.BarkodNo,
                        BirimFiyati = model.BirimFiyati,
                        Miktari = model.Miktari,
                        ToplamFiyati = model.ToplamFiyati,
                        KDV=model.Urunler.KDV,
                        BirimID=model.Urunler.BirimID,
                        Tarih=DateTime.Now,
                        Saat=DateTime.Now
                        
                    };

                    //SAtılan ürünün mitarını ürün tablosundaki miktardan düşürme
                    var urunmodel = db.Urunler.FirstOrDefault(x=>x.ID==model.UrunID);
                    urunmodel.Miktar = urunmodel.Miktar - model.Miktari;
                    //db.Entry(urunmodel).State = System.Data.Entity.EntityState.Modified;



                    db.Sepet.Remove(model);//Spetten siliyoruz
                    db.Satislar.Add(satis);//Satışların tablosuna ekliyoruz..
                    db.SaveChanges();
                    
                    ViewBag.islem = "True";
                }
            }
            catch (Exception)
            {

                ViewBag.islem = "False";
            }

            return View("islem");

        }
        public ActionResult islem()
        {
            return View();
        }

        [HttpGet]
        public ActionResult HepsiniSatinAl(decimal? Tutar)
        {
            if (User.Identity.IsAuthenticated)//Kullanıcı otantike olmuşsa
            {
                var kullaniciAdi = User.Identity.Name;
                var kullanici = db.Kullanicilar.FirstOrDefault(x => x.KullaniciAdi == kullaniciAdi);//Kullanıcının Kullanıcılar tablosundan bilgiilerine ulaş
                var model = db.Sepet.Where(x => x.KullaniciID == kullanici.Id).ToList();//Giriş yapan kullanıcının sepet tablosundaki tüm ürünlerini listele
                var kid = db.Sepet.FirstOrDefault(x => x.KullaniciID == kullanici.Id);

                if (model != null)
                {
                    if (kid == null)
                    {
                        ViewBag.Tutar = "Sepetinizde ürün bulunmuyor..";
                    }
                    else if (kid != null)
                    {
                        Tutar = db.Sepet.Where(x => x.KullaniciID == kid.KullaniciID).Sum(x => x.ToplamFiyati);
                        ViewBag.Tutar = "Alışveriş Toplam Tutar: " + Tutar + "₺";
                    }
                    return View(model);
                }


            }
            return HttpNotFound();
        }
        [HttpPost]
        public ActionResult HepsiniSatiAl()
        {
            if (User.Identity.IsAuthenticated)//Kullanıcı atantike olmuşsa
            {
                var kullaniciAdi = User.Identity.Name;
                var kullanici = db.Kullanicilar.FirstOrDefault(x => x.KullaniciAdi.Equals(kullaniciAdi));
                var KullaniciSepeti = db.Sepet.Where(x => x.KullaniciID == kullanici.Id).ToList();
               

                //Birden Fazle Satırı Satış Tablosuna Ekleme işlemi
                int row = 0;
                foreach (var item in KullaniciSepeti)
                {
                    var satis = new Satislar()
                    {
                        KullaniciID=KullaniciSepeti[row].KullaniciID,
                        UrunID= KullaniciSepeti[row].UrunID,
                        SpetID= KullaniciSepeti[row].ID,
                        BarkodNo= KullaniciSepeti[row].Urunler.BarkodNo,
                        BirimFiyati= KullaniciSepeti[row].BirimFiyati,
                        Miktari= KullaniciSepeti[row].Miktari,
                        ToplamFiyati= KullaniciSepeti[row].ToplamFiyati,
                        KDV= KullaniciSepeti[row].Urunler.KDV,
                        BirimID= KullaniciSepeti[row].Urunler.BirimID,
                        Tarih= DateTime.Now,
                        Saat= DateTime.Now,
                    };
                    row++;
                    db.Satislar.Add(satis);
                }



                foreach (var item in KullaniciSepeti)
                {
                    var urun = db.Urunler.FirstOrDefault(x => x.ID == item.UrunID);
                    if (urun!=null)
                    {
                        urun.Miktar = urun.Miktar - item.Miktari;
                    }
                }


                db.Sepet.RemoveRange(KullaniciSepeti);
                db.SaveChanges();
                return RedirectToAction("Index","Sepet");
            }
            return HttpNotFound();
        }

    }
}