using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SchoolWebApp.Models
{
    public class News
    {
        public int Id { get; set; } // Primary Key


        [Required]
        [DisplayName("موجز الخبر")]
        public string NewsSummary { get; set; }


        [Required]
        [DisplayName("عنوان الخبر")]
        public string NewsTitle { get; set; }



        [Required]
        [AllowHtml]
        [DisplayName("تفاصيل الخبر")]
        public string NewsDetails { get; set; }


        [DisplayName("صورة الخبر")]
        public string NewsImage { get; set; }



        [Required]
        [DisplayName("تاريخ النشر")]
        public DateTime NewsDate { get; set; }


        [Required]
        [DisplayName("نوع الخبر")]
        public int NewsTypesID { get; set; }          //Foergin Key
        public virtual NewsTypes NewsType { get; set; }
    }
}