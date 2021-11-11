using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MVC_StokTakip.Models.Entity;
using MVC_StokTakip.MyModel;

namespace MVC_StokTakip.Controllers
{
    [Authorize(Roles = "A,U,U2")]

    public class BirimlerController : Controller
    {
        MVC_StokTakipEntities db = new MVC_StokTakipEntities();
        public ActionResult Index()
        {
            var birimler = db.Birimler.ToList();
            return View(birimler);
        }
        public ActionResult BirimEkle()
        {
            return View("BirimForm", new MyBirimler());
        }

        [HttpPost]
        public ActionResult BirimKaydet(Birimler bir)
        {
            if (!ModelState.IsValid)
            {
                var mymodel = new MyBirimler();
                return View("BirimForm",mymodel);
            }
            if (bir.ID == 0)
            {
                db.Birimler.Add(bir);
            }
            else
            {
                var guncellenecekBirim = db.Birimler.Find(bir.ID);
                if (guncellenecekBirim != null)
                {
                    //db.Entry(br).State = System.Data.Entity.EntityState.Modified;
                    guncellenecekBirim.Birim = bir.Birim;
                    guncellenecekBirim.Aciklama = bir.Aciklama;
                }
                else
                    return HttpNotFound();
            }


            db.SaveChanges();
            return RedirectToAction("Index");
        }

        public ActionResult GuncelleGetir(int? Id)
        {
            var ilgilimodel = db.Birimler.Find(Id);//İlgili kaydı bul
            if (ilgilimodel == null)//Böyle bir kayıt yok ise          
                return RedirectToAction("NotFound","Error");//hata döndür

            MyBirimler myModel = new MyBirimler();
            myModel.ID = ilgilimodel.ID;
            myModel.Birim = ilgilimodel.Birim;
            myModel.Aciklama = ilgilimodel.Aciklama;

            return View("BirimForm", myModel);
        }

        public ActionResult BirimlerSil(int id)
        {
            var silinecekBirim = db.Birimler.Find(id);
            if (silinecekBirim == null)//Böyle bir kayıt yok ise          
                return HttpNotFound();//hata döndür

            db.Birimler.Remove(silinecekBirim);
            db.SaveChanges();
            return RedirectToAction("Index");

        }

        //İkinci Yöntem Silme
        public ActionResult SilBilgiGetir(int Id)
        {
            var model = db.Birimler.Find(Id);

            //Verileri kendi modelimizdeki mybirimlere aktarıyoruz..
            MyBirimler myModel = new MyBirimler();
            myModel.ID = model.ID;
            myModel.Birim = model.Birim;
            myModel.Aciklama = model.Aciklama;

            if (myModel == null)
                return HttpNotFound();

            return View("SilBilgiGetir", myModel);
        }
        public ActionResult Sil(Birimler p)
        {
            db.Entry(p).State = System.Data.Entity.EntityState.Deleted;
            db.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}