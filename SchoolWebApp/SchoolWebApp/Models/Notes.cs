using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApplication1.Models;

namespace SchoolWebApp.Models
{
    public class Notes
    {
        public int Id { get; set; } // Primary Key

        [AllowHtml]
        [DisplayName("الملاحظة")]
        public string Note { get; set; }


        [DisplayName("تاريخ الملاحظة")]
        public DateTime NoteDate { get; set; }


        [DisplayName("الصف")]
        public int GradesId { get; set; }          //Foergin Key
        public virtual Grades Grade { get; set; }


        [DisplayName("الشعبة")]
        public int SectionsId { get; set; }          //Foergin Key
        public virtual Sections Section { get; set; }


        [Display(Name = "الأستاذ")]
        public string TeacherID { get; set; }          //Foergin Key
        public virtual ApplicationUser Teacher { get; set; }


        [Display(Name = "الطالب")]
        public string StudentID { get; set; }          //Foergin Key
        public virtual ApplicationUser Student { get; set; }


    }
}