using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MVC_StokTakip.Models.Entity;

namespace MVC_StokTakip.Controllers
{
    public class SepetController : Controller
    {
        MVC_StokTakipEntities db = new MVC_StokTakipEntities();
        // GET: Sepet
        public ActionResult Index(decimal? Tutar)
        {
            if (User.Identity.IsAuthenticated)//Kullanıcı atantike olmuşsa
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
                        ViewBag.Tutar = "Toplam Tutar: " + Tutar + "₺";
                    }
                    return View(model);
                }


            }
            return HttpNotFound();
        }
        public ActionResult SepeteEkle(int Id)
        {
            if (User.Identity.IsAuthenticated)//Kullanıcı atantike olmuşsa
            {
                var kullaniciAdi = User.Identity.Name;//giriş yapan kullanıcının adını al
                //Kullanıcılar tablosunda bu kullanıcı var mı
                var model = db.Kullanicilar.FirstOrDefault(x => x.KullaniciAdi == kullaniciAdi);
                //Kullanıcının eklemeye çalıtığı ürün ürünler tablosunda var mı
                var urun = db.Urunler.Find(Id);

                //Giriş yapan kullanıcının kullanıcı Id si ve seçilen ürünün Idsi sepet tablosunda var mı
                var sepet = db.Sepet.FirstOrDefault(x => x.KullaniciID == model.Id && x.UrunID == Id);

                if (model != null)
                {
                    if (sepet != null)
                    {
                        //Giriş yapan kullanıcının kullanıcı Id si ve seçilen ürünün Idsi sepet tablosunda varsa demekki sepetinde aynı üründen ürün var Bu durumda ürünün miktarını ve fiyatını güncelliyoruz.
                        sepet.Miktari++;
                        sepet.ToplamFiyati = urun.SatisFiyati * sepet.Miktari;
                        db.SaveChanges();
                        return RedirectToAction("Index");
                    }
                    var yenisepet = new Sepet()
                    {
                        KullaniciID = model.Id,
                        UrunID = urun.ID,
                        Miktari = 1,
                        BirimFiyati = urun.SatisFiyati,
                        ToplamFiyati = urun.SatisFiyati,
                        Tarih = DateTime.Now,
                        Saat = DateTime.Now
                    };
                    db.Entry(yenisepet).State = System.Data.Entity.EntityState.Added;
                    db.SaveChanges();
                    return RedirectToAction("Index");


                }
            }
            return HttpNotFound();
        }


        //1.Yöntem Header sayfasına gönderiyoruz.. @{ Html.RenderAction("UrunSayisi", "Sepet"); }
        public int? UrunSayisi()
        {

            if (User.Identity.IsAuthenticated)//Kullanıcı atantike olmuşsa
            {
                var kullaniciAdi = User.Identity.Name;
                var kullanici = db.Kullanicilar.FirstOrDefault(x => x.KullaniciAdi == kullaniciAdi);
                var kid = db.Sepet.FirstOrDefault(x => x.KullaniciID == kullanici.Id);

                if (kid == null)
                {
                    return 0;
                }
                else if (kid != null)
                {
                    return db.Sepet.Count();
                }
            }
            return 0;
        }

        //2.Yöntem Header sayfasına partilview ile gönderiyoruz..
        public ActionResult TotalCount(int? count)
        {

            if (User.Identity.IsAuthenticated)//Kullanıcı atantike olmuşsa
            {
                var model = db.Kullanicilar.FirstOrDefault(x => x.KullaniciAdi == User.Identity.Name);
                count = db.Sepet.Where(x => x.KullaniciID == model.Id).Count();

                ViewBag.Count = count;
                if (count == 0)
                {
                    ViewBag.Count = 0;
                }
                return PartialView();

            }
            return HttpNotFound();
        }

        public ActionResult Arttir(int Id)
        {
            var model = db.Sepet.Find(Id);
            model.Miktari++;
            model.ToplamFiyati = model.Miktari * model.BirimFiyati;
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        public ActionResult Azalt(int Id)
        {
            var model = db.Sepet.Find(Id);
            if (model.Miktari == 1)
            {
                db.Sepet.Remove(model);
                db.SaveChanges();

            }
            model.Miktari--;
            model.ToplamFiyati = model.Miktari * model.BirimFiyati;
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        public void DinamikMiktar(int Id, decimal miktar)
        {
            var model = db.Sepet.Find(Id);
            model.Miktari = miktar;
            model.ToplamFiyati = model.Miktari * model.BirimFiyati;
            db.SaveChanges();

        }

        public ActionResult SepetSil(int Id)
        {
            var model = db.Sepet.Find(Id);

            db.Entry(model).State = System.Data.Entity.EntityState.Deleted;
            //db.Sepet.Remove(model);
            db.SaveChanges();
            return RedirectToAction("Index");

        }
        public ActionResult HepsiniSil()
        {
            if (User.Identity.IsAuthenticated)//Kullanıcı atantike olmuşsa
            {
                var kullaniciAdi = User.Identity.Name;
                var kullanici = db.Kullanicilar.FirstOrDefault(x => x.KullaniciAdi.Equals(kullaniciAdi));
                var KullaniciSepeti = db.Sepet.Where(x => x.KullaniciID == kullanici.Id);

                db.Sepet.RemoveRange(KullaniciSepeti);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return HttpNotFound();

        }

        public ActionResult SeciliSil(FormCollection form)
        {
            string[] seciliid = form["secim_id"].Split(new char[] {','});
            foreach (string id in seciliid)
            {
                Sepet model = db.Sepet.Find(int.Parse(id));
                db.Sepet.Remove(model);
                db.SaveChanges();
            }
            return RedirectToActionPermanent("Index");
        }
    }
}