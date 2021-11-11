using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MVC_StokTakip.Models.Entity;
using MVC_StokTakip.MyModel;
using MVC_StokTakip.ViewModels;

namespace MVC_StokTakip.Controllers
{
    
    public class MarkalarController : Controller
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

            DropdownViewModel katViewModel = new DropdownViewModel()
            {
                KategoriData = new SelectList(db.Kategoriler.ToList(), "ID", "Kategori"),
                Marka = new MyMarkalar(),
                SelectedKatId = -1

            };

            return View(katViewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult MarkaEkle(DropdownViewModel dropmodel)
        {
            if (!ModelState.IsValid)
            {
                DropdownViewModel katViewModel = new DropdownViewModel()
                {
                    KategoriData = new SelectList(db.Kategoriler.ToList(), "ID", "Kategori"),
                    Marka = dropmodel.Marka,
                    SelectedKatId = dropmodel.SelectedKatId
                };
                return View(katViewModel);
            }


            //db.Markalar.Add(DropdownViewModel.Marka);
            dropmodel.Marka.KategoriID = dropmodel.SelectedKatId;

            Markalar markalar = new Markalar();
            markalar.ID = dropmodel.Marka.ID;
            markalar.KategoriID = dropmodel.Marka.KategoriID;
            markalar.Marka = dropmodel.Marka.Marka;
            markalar.Aciklama = dropmodel.Marka.Aciklama;

            //db.Markalar.Add(markalar);
            db.Entry(markalar).State = System.Data.Entity.EntityState.Added;
            db.SaveChanges();

            return RedirectToAction("Index");
        }


        //Güncelleme İşlemleri-----------------------------------------------------------
        public ActionResult MarkaGetir(int Id)
        {
            //var sorgu = db.Markalar.Where(x => x.ID == Id).Select(y => y.KategoriID).Single();
            var sorgu = db.Markalar.Where(x => x.ID == Id).FirstOrDefault();

            MyMarkalar markalar = new MyMarkalar();
            markalar.ID = sorgu.ID;
            markalar.KategoriID = sorgu.KategoriID;
            markalar.Marka = sorgu.Marka;
            markalar.Aciklama = sorgu.Aciklama;

            DropdownViewModel katViewModel = new DropdownViewModel()
            {
                KategoriData = new SelectList(db.Kategoriler, "ID", "Kategori"),
                SelectedKatId = markalar.KategoriID,
                Marka = markalar

            };

            return View(katViewModel);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult MarkaGuncelle(DropdownViewModel dropModel)
        {
            if (!ModelState.IsValid)
            {
                DropdownViewModel katViewModel = new DropdownViewModel()
                {
                    KategoriData = new SelectList(db.Kategoriler, "ID", "Kategori"),
                    Marka = dropModel.Marka,
                    SelectedKatId = dropModel.SelectedKatId

                };
                return View("MarkaGetir", katViewModel);
            }



           
            dropModel.Marka.KategoriID = dropModel.SelectedKatId;

            Markalar markalar = new Markalar();
            markalar.ID = dropModel.Marka.ID;
            markalar.KategoriID = dropModel.Marka.KategoriID;
            markalar.Marka = dropModel.Marka.Marka;
            markalar.Aciklama = dropModel.Marka.Aciklama;


            //db.Markalar.Add(DropdownViewModel.Marka);
            db.Entry(markalar).State = System.Data.Entity.EntityState.Modified;
            db.SaveChanges();

            return RedirectToAction("Index");

        }

        //Güncelleme İşlemleri-------------------------------------------------------





        //Silme İşlemleri--------------------------------------------------

        //Modalla silme----------------------
        public ActionResult MarkalarSil(int id)
        {
            var silinecekMarka = db.Markalar.Find(id);
            if (silinecekMarka == null)//Böyle bir kayıt yok ise          
                return HttpNotFound();//hata döndür

            db.Markalar.Remove(silinecekMarka);
            db.SaveChanges();
            return RedirectToAction("Index");

        }
        //------------------------------------

        //Farklı sayfaya taşıyarak silme
        public ActionResult SilBilgiGetir(int Id)
        {
            var model = db.Markalar.Find(Id);

            MyMarkalar markalar = new MyMarkalar();
            markalar.ID = model.ID;
            markalar.KategoriID = model.KategoriID;
            markalar.Marka = model.Marka;
            markalar.Aciklama = model.Aciklama;

            if (markalar == null)
                return HttpNotFound();

            return View("SilBilgiGetir", markalar);
        }
        public ActionResult Sil(Markalar p)
        {
            db.Entry(p).State = System.Data.Entity.EntityState.Deleted;
            //var silinecekKategori = db.Kategoriler.Find(p.ID);
            //db.Kategoriler.Remove(silinecekKategori);

            db.SaveChanges();
            return RedirectToAction("Index");
        }
        //Silme İşlemleri--------------------------------------------------
    }
}