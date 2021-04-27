using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApplication1.Models;

namespace SchoolWebApp.Models
{
    public class TimeTables
    {
        public int Id { get; set; } // Primary Key


        [Required]
        [DisplayName("عنوان الجدول")]
        public string TableTitle { get; set; }

        
        [Required]
        [AllowHtml]
        [DisplayName("تفاصيل الجدول")]
        public string TableTitleDetails { get; set; }

     
        [DisplayName(" الجدول")]
        public string TableTitleURL { get; set; }


        [DisplayName("التاريخ ")]
        public DateTime TableTitleDate { get; set; }


        [Required]
        [DisplayName("نوع الجدول")]
        public int TimeTablesTypesId { get; set; }          //Foergin Key
        public virtual TimeTablesTypes TimeTablesType { get; set; }


        [Display(Name = "المدرس")]
        public string UserID { get; set; }          //Foergin Key
        public virtual ApplicationUser User { get; set; }

        public virtual ICollection<GradeTimeTable> GradeTimeTable { get; set; }


    }
}