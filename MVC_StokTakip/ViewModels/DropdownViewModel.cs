using MVC_StokTakip.Models.Entity;
using MVC_StokTakip.MyModel;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MVC_StokTakip.ViewModels
{
    public class DropdownViewModel
    {
        [Required(ErrorMessage ="Kategori Alanı Gereklidir")]
        [Display(Name ="Kategori")]
        public int SelectedKatId { get; set; }

        public SelectList KategoriData { get; set; }
        public MyMarkalar Marka { get; set; }

    }
}