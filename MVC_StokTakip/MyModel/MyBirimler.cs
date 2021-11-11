using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MVC_StokTakip.MyModel
{
    public class MyBirimler
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public MyBirimler()
        {
            //this.Satislar = new HashSet<Satislar>();
            //this.Sepet = new HashSet<Sepet>();
            this.Urunler = new HashSet<MyUrunler>();
        }

        public int ID { get; set; }
        [Required(ErrorMessage = "Birim alanı zorunludur")]
        public string Birim { get; set; }
        [Required(ErrorMessage = "Açıklama alanı zorunludur")]
        [Display(Name = "Açıklama")]
        public string Aciklama { get; set; }

        //[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        //public virtual ICollection<Satislar> Satislar { get; set; }


        //[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        //public virtual ICollection<Sepet> Sepet { get; set; }


        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<MyUrunler> Urunler { get; set; }
    }
}