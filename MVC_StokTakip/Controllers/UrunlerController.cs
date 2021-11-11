using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MVC_StokTakip.Models.Entity;
using MVC_StokTakip.MyModel;
using Excel = Microsoft.Office.Interop.Excel;

namespace MVC_StokTakip.Controllers
{
    public class UrunlerController : Controller
    {
        MVC_StokTakipEntities db = new MVC_StokTakipEntities();
        public ActionResult Index(string ara)
        {
          
            if (!string.IsNullOrEmpty(ara))
            {
                var modelara = db.Urunler.Where(x => x.UrunAdi.Contains(ara) || x.BarkodNo.Contains(ara)).ToList(); ;
                return View(modelara);
            }
            var model = db.Urunler.ToList();
            return View(model);
        }
        //Ürün Ekleme İşlemleri-------------------
        public ActionResult Ekle()
        {
            var mymodel = new MyUrunler();
            //Kategori ve Birim dropdownları burda 
            Yenile(mymodel);
            //Seçilen kategoriye bağlı olarak ta Narka json GetMarka ile dolduruluyor

            return View(mymodel);
        }

        private void Yenile(MyUrunler model)
        {
            //KAtegori Listesini Alıp Modeldeki KategoriListesine dolduruyoruz
            List<Kategoriler> kategoriList = db.Kategoriler.OrderBy(x => x.Kategori).ToList();
            model.KategoriListesi = (from x in kategoriList
                                     select new SelectListItem
                                     {
                                         Text = x.Kategori,
                                         Value = x.ID.ToString()

                                     }).ToList();

            //Birim Listesini Alıp Modeldeki BirimListesine dolduruyoruz
            List<Birimler> birimList = db.Birimler.OrderBy(x => x.Birim).ToList();
            model.BirimListesi = (from x in birimList
                                  select new SelectListItem
                                  {
                                      Text = x.Birim,
                                      Value = x.ID.ToString()

                                  }).ToList();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Ekle(Urunler p)
        {
            if (!ModelState.IsValid)
            {
                var model = new MyUrunler();
                Yenile(model);

                return View(model);
            }

            db.Entry(p).State = System.Data.Entity.EntityState.Added;
            db.SaveChanges();
            return RedirectToAction("Index");
        }
        //---------------------------------------------------------------------
        //Miktar Değişiklik İşlmeleri

        [HttpGet]
        public ActionResult MiktarEkle(int Id)
        {
            var model = db.Urunler.Find(Id);

            var urnModel = new MyUrunler();
            urnModel.ID = model.ID;
            //urnModel.KategoriID = model.KategoriID;
            //urnModel.MarkaID = model.MarkaID;
            //urnModel.UrunAdi = model.UrunAdi;
            //urnModel.BarkodNo = model.BarkodNo;
            //urnModel.AlisFiyati = model.AlisFiyati;
            //urnModel.SatisFiyati = model.SatisFiyati;
            urnModel.Miktar = model.Miktar;
            //urnModel.BirimID = model.BirimID;
            //urnModel.KDV = model.KDV;
            //urnModel.Tarih = model.Tarih;
            //urnModel.Aciklama = model.Aciklama;

            return View(urnModel);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult MiktarEkle(Urunler p)
        {
            var model = db.Urunler.Find(p.ID);
            //model.Miktar = model.Miktar + p.Miktar;//Burda elle değer girerek yaprık
            model.Miktar = p.Miktar;//Butonla artırarak yaptık
            db.SaveChanges();
            return RedirectToAction("Index");
        }
        //-------------------------------------------------------------------------

        [HttpPost]
        public JsonResult GetMarka(int id)
        {
            //Idd Bu script tarafındaki parametre ile aynı olmalı
            var model = new MyUrunler();
            List<Markalar> markaListe = db.Markalar.Where(x => x.KategoriID == id).OrderBy(x => x.Marka).ToList();

            model.MarkaListesi = (from x in markaListe
                                  select new SelectListItem
                                  {
                                      Text = x.Marka,
                                      Value = x.ID.ToString()
                                  }).ToList();

            model.MarkaListesi.Insert(0, new SelectListItem { Text = "Seçiniz", Value = "" });
            return Json(model.MarkaListesi, JsonRequestBehavior.AllowGet);
        }

        //Güncelleme İşlemleri
        public ActionResult GuncelleBilgiGetir(int Id)
        {
            var model = db.Urunler.Find(Id);
            model.Tarih = Convert.ToDateTime(DateTime.Now);

            var urnModel = new MyUrunler();
            urnModel.ID = model.ID;
            urnModel.KategoriID = model.KategoriID;
            urnModel.MarkaID = model.MarkaID;
            urnModel.UrunAdi = model.UrunAdi;
            urnModel.BarkodNo = model.BarkodNo;
            urnModel.AlisFiyati = model.AlisFiyati;
            urnModel.SatisFiyati = model.SatisFiyati;
            urnModel.Miktar = model.Miktar;
            urnModel.BirimID = model.BirimID;
            urnModel.KDV = model.KDV;
            urnModel.Tarih = model.Tarih;
            urnModel.Aciklama = model.Aciklama;

            Yenile(urnModel);

            List<Markalar> markalist = db.Markalar.Where(x => x.KategoriID == model.KategoriID).OrderBy(x => x.Marka).ToList();
            urnModel.MarkaListesi = (from x in markalist
                                  select new SelectListItem
                                  {
                                      Text = x.Marka,
                                      Value = x.ID.ToString()
                                  }).ToList();
            return View(urnModel);
        }
        [HttpPost]
        public ActionResult Guncelle(Urunler p)
        {
            if (!ModelState.IsValid)
            {
                var model = db.Urunler.Find(p.ID);
                var urnModel = new MyUrunler();             
                Yenile(urnModel);
                List<Markalar> markalist = db.Markalar.Where(x => x.KategoriID == model.KategoriID).OrderBy(x => x.Marka).ToList();
                urnModel.MarkaListesi = (from x in markalist
                                      select new SelectListItem
                                      {
                                          Text = x.Marka,
                                          Value = x.ID.ToString()
                                      }).ToList();
                return View(urnModel);

            }

            db.Entry(p).State = System.Data.Entity.EntityState.Modified;
            db.SaveChanges();
            return RedirectToAction("Index");

        }
        //Ürü silme İşlemleri
        public ActionResult Sil(int Id)
        {
            var model = db.Urunler.Find(Id);
            db.Entry(model).State = System.Data.Entity.EntityState.Deleted;
            db.SaveChanges();
            return RedirectToAction("Index");

        }

        public ActionResult ExcellExport()
        {
            try
            {
                var liste = db.Urunler.ToList();
                Excel.Application application =new Excel.Application();
                Excel.Workbook workbook =application.Workbooks.Add(System.Reflection.Missing.Value);
                Excel.Worksheet worksheet = workbook.ActiveSheet;//Uygulamada o an hangi sayfa açıksa onu aktif et
                //Başlık kısımlarını ayarla
                worksheet.Cells[1, 1] = "ID";
                worksheet.Cells[1, 2] = "Ürün Adı";
                worksheet.Cells[1, 3] = "Barkod No";
                worksheet.Cells[1, 4] = "Fiyatı";
                worksheet.Cells[1, 5] = "Miktarı";
                worksheet.Cells[1, 6] = "Tarih";
                int row = 2;//1.satır başlıktı
                foreach (var item in liste)
                {
                    worksheet.Cells[row, 1] = item.ID;
                    worksheet.Cells[row, 2] = item.UrunAdi;
                    worksheet.Cells[row, 3] = item.BarkodNo;
                    worksheet.Cells[row, 4] = item.SatisFiyati;
                    worksheet.Cells[row, 5] = item.Miktar;
                    worksheet.Cells[row, 6] = item.Tarih;


                    worksheet.Cells[row, 2].ColumnWidth = 15;
                    worksheet.Cells[row, 4].ColumnWidth = 15;
                    worksheet.Cells[row, 6].ColumnWidth = 15;      
                    row++;
                }

                //Başlık satırının özelliklerini ayarlıyoruz..
                var heading = worksheet.get_Range("A1", "F1");
                heading.Font.Bold = true;
                heading.Font.Size = 13;
                heading.Font.Color = System.Drawing.Color.Red;
                //Satış fiyat toplamı bulduk
                var sum = db.Urunler.Sum(x => x.SatisFiyati).ToString("#,###,###.00");
                var range_sum = worksheet.get_Range("D" + row);
                range_sum.Value2 = "Total: " + sum;
                range_sum.Font.Bold = true;

                var range_fiyat = worksheet.get_Range("D2", "D" + row);
                range_fiyat.NumberFormat = "#,###,###.00";

                var range_tarih = worksheet.get_Range("F2", "F" + row);
                range_tarih.NumberFormat = "dd.MM.yyyy";

                string GuidKey = Guid.NewGuid().ToString();
                workbook.SaveAs("F:\\"+GuidKey.ToString().Substring(0,5));
                workbook.Close();
                application.Quit();

                TempData["Mesaj"] = " İşlem Başarılı...";

            }
            catch (Exception ex)
            {

                TempData["Mesaj"] = ex.Message + " İşlem Başarısız...";
            }


            return RedirectToAction("Index");
        }


    }
}