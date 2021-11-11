using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MVC_StokTakip.Models.Entity;
using System.Web.Security;//ÇerezOluşturmak için
using System.Net.Mail;
using System.Net;

namespace MVC_StokTakip.Controllers
{
    [AllowAnonymous]
    public class KullanicilarController : Controller
    {
        MVC_StokTakipEntities db = new MVC_StokTakipEntities();


        [HttpGet]
        public ActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Login(Kullanicilar kullanici)
        {
            var model = db.Kullanicilar.FirstOrDefault(x => x.KullaniciAdi == kullanici.KullaniciAdi && x.Sifre == kullanici.Sifre);
            if (model != null)
            {
                FormsAuthentication.SetAuthCookie(kullanici.KullaniciAdi, false);
                Session["AdSoyad"] = model.AdiSoyadi;
                Session["Email"] = model.Email;

                return RedirectToAction("Index", "Home");


            }
            ViewBag.Hata = "Kullanıcı adı veya şifre yanlış";
            return View();
        }

        public ActionResult LogOut()
        {
            Session.Abandon();
            FormsAuthentication.SignOut();
            return RedirectToAction("Login");
        }
        [HttpGet]
        public ActionResult ResetPassword()
        {
            return View();
        }

        [HttpPost]
        public ActionResult ResetPassword(Kullanicilar k)
        {
            var model = db.Kullanicilar.Where(x => x.Email == k.Email).FirstOrDefault();
            if (model != null)
            {
                Guid rastgele = Guid.NewGuid();
                model.Sifre = rastgele.ToString().Substring(0, 8);//8 karakterli şifre oluşturduk
                db.SaveChanges();

                //maili gönderme
                //Sunucu belirleme gmail kullanıyoruz
                SmtpClient client = new SmtpClient("smtp.gmail.com", 587);
                client.EnableSsl = true;//Güvenlik ssl aktif et
                MailMessage mail = new MailMessage();
                mail.From = new MailAddress("mesut.ren@gmail.com", "Şifre sıfırlama");
                mail.To.Add(model.Email);//Kime gönderilecek
                mail.IsBodyHtml = true;//Html yazımı aktif et
                mail.Subject = "Şifre değiştirme İsteği";//MEsaj konusunu belirle
                mail.Body += "Merhaba " + model.AdiSoyadi + "</br> Kullanıcı Adınız:" + model.KullaniciAdi + "<br> Şifreniz:" + model.Sifre;//Mesaj içeriğini oluştur
                NetworkCredential net = new NetworkCredential("mesut.ren@gmail.com", "m.s.ttokat1990");//Mesaj gönderilecek mail adresi ve şifreyi gir
                client.Credentials = net;
                client.Send(mail);//Mesajı gönder

                return RedirectToAction("Login");

            }
            ViewBag.hata = "Böyle bir E-mail adresi bulunamadı..";
            return View();
        }

        [HttpGet]
        public ActionResult Kaydol()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Kaydol(Kullanicilar k)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }
            var model = db.Kullanicilar.Where(x => x.KullaniciAdi == k.KullaniciAdi || x.Email == k.Email).FirstOrDefault();
            if (model != null)
            {
                ViewBag.Uyari = "Girilen Kullanıcı Adı veya Email sistemde kayıtlı.Lütfen bu bilgileri değiştiriniz..";
                return View();
            }
            db.Entry(k).State = System.Data.Entity.EntityState.Added;
            db.SaveChanges();
            return RedirectToAction("Login");
        }

        [HttpGet]
        public ActionResult Guncelle()
        {
            if (User.Identity.IsAuthenticated)//Kullanıcı Autentike olmuşmu giriş yapılmışmı
            {
                // bir giriş yapılmışsa  giriş yapan kullanıcının adinı al ve kullanıcılar tablosudaki kullanıcı adı ile karşılaştır var mı böyle bir kullnıcı kontrol et
                var kullaniciAdi = User.Identity.Name;
                var model = db.Kullanicilar.FirstOrDefault(x => x.KullaniciAdi == kullaniciAdi);
                if (model != null)
                {
                    model.Tarih = DateTime.Now;
                    return View(model);
                }
                else
                {
                    return View(new Kullanicilar());
                }

            }
            return HttpNotFound();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Guncelle(Kullanicilar kul)
        {
            //Burda güncelleme işleminden sonra kullanıcıya çıkış yaptırıp Logine gönderdik ama istenirse çıkış yaprılmadan istenilen bir sayfaya da yönlendirilebilir..

            if (!ModelState.IsValid)
            {
                return View();
            }

            //1.yöntem
            db.Entry(kul).State = System.Data.Entity.EntityState.Modified;
            db.SaveChanges();
            FormsAuthentication.SignOut();
            return RedirectToAction("Login");

            //2.Yöntem

            //db.Entry(kul).State = System.Data.Entity.EntityState.Modified;
            //db.SaveChanges();
            //FormsAuthentication.SignOut();
            //FormsAuthentication.SetAuthCookie(kul.KullaniciAdi, false);
            //return RedirectToAction("Index","Urunler");


        }


    }
}