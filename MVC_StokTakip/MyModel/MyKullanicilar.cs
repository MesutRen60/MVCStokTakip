using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MVC_StokTakip.MyModel
{
    public partial class MyKullanicilar
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public MyKullanicilar()
        {
            //this.KullaniciRolleri = new HashSet<KullaniciRolleri>();
        }


        public int Id { get; set; }

        [Required(ErrorMessage = "Kullanıcı Adı alanı zorunludur..")]
        public string KullaniciAdi { get; set; }

        [Required(ErrorMessage = "Şifre alanı zorunludur..")]
        public string Sifre { get; set; }

        [Required(ErrorMessage = "Ad Soyad alanı zorunludur..")]
        public string AdiSoyadi { get; set; }

        [Required(ErrorMessage = "Telefon alanı zorunludur..")]
        [DataType(DataType.PhoneNumber)]
        [RegularExpression(@"^\(?([0-9]{3})\)?[-. ]?([0-9]{3})[-. ]?([0-9]{4})$",
                   ErrorMessage = "Uygun formatta telefon numarası giriniz.")]
        //5078303815 formatı için
        public string Telefon { get; set; }

        [Required(ErrorMessage = "Adres alanı zorunludur..")]
        public string Adres { get; set; }

        [Required(ErrorMessage = "Email alanı zorunludur..")]
        [DataType(DataType.EmailAddress)]
        [RegularExpression("^[a-zA-Z0-9_\\.-]+@([a-zA-Z0-9-]+\\.)+[a-zA-Z]{2,6}$",
            ErrorMessage = "Lütfen uygun formatta e-mail adresi giriniz.")]
        public string Email { get; set; }

        [DataType(DataType.Date)]
        //[DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy}")]
        public System.DateTime Tarih { get; set; }


        //[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        //public virtual ICollection<KullaniciRolleri> KullaniciRolleri { get; set; }
    }
}

