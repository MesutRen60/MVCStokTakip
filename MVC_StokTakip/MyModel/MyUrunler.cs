using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MVC_StokTakip.MyModel
{
    public class MyUrunler
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]


        public MyUrunler()
        {
            //this.Satislar = new HashSet<Satislar>();
            //this.Sepet = new HashSet<Sepet>();
            this.MarkaListesi = new List<SelectListItem>();
            MarkaListesi.Insert(0, new SelectListItem { Text = "Önce Kategori Alanı seçilmelidir!", Value = "" });
        }

        public int ID { get; set; }
        [Required(ErrorMessage = "Kategori alanı boş geçilemez..")]
        [Display(Name = "Kategori")]
        public int KategoriID { get; set; }

        [Display(Name = "Marka")]
        [Required(ErrorMessage = "Marka alanı boş geçilemez..")]
        public int MarkaID { get; set; }

        [Required(ErrorMessage = "Ürün Adı alanı boş geçilemez..")]
        [Display(Name = "Ürün Adı")]
        public string UrunAdi { get; set; }

        [Required(ErrorMessage = "Barkod Na Adı alanı boş geçilemez..")]
        [Display(Name = "Barkod No")]
        public string BarkodNo { get; set; }

        [Required(ErrorMessage = "Alis Fiyati alanı boş geçilemez..")]
        [Display(Name = "Alış Fiyatı")]
        public decimal? AlisFiyati { get; set; }

        [Required(ErrorMessage = "Satış Fiyati alanı boş geçilemez..")]
        [Display(Name = "Satış Fiyatı")]
        public decimal? SatisFiyati { get; set; }

        [Required(ErrorMessage = "Miktar alanı boş geçilemez..")]
        [Display(Name = "Miktar")]
        public decimal? Miktar { get; set; }

        [Required(ErrorMessage = "Birim alanı boş geçilemez..")]
        [Display(Name = "Birim")]
        public int BirimID { get; set; }

        [Required(ErrorMessage = "K.D.V. alanı boş geçilemez..")]
        [Display(Name = "K.D.V.")]
        [Range(0, 100, ErrorMessage = "0-100 Arası rakam girilmelidir..")]
        public int? KDV { get; set; }

        [Required(ErrorMessage = "Tarih alanı boş geçilemez..")]
        [Display(Name = "Tarih")]
        [DataType(DataType.Date)]
        //[DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy}")]
        public System.DateTime Tarih { get; set; }

        [Required(ErrorMessage = "Açıklama alanı zorunludur")]
        [Display(Name = "Açıklama")]
        public string Aciklama { get; set; }

        public virtual MyBirimler Birimler { get; set; }
        public virtual MyKategoriler Kategoriler { get; set; }
        public virtual MyMarkalar Markalar { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        //public virtual ICollection<Satislar> Satislar { get; set; }
        //[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]

        //public virtual ICollection<Sepet> Sepet { get; set; }
        public List<SelectListItem> KategoriListesi { get; set; }
        public List<SelectListItem> MarkaListesi { get; set; }
        public List<SelectListItem> BirimListesi { get; set; }
    }
}