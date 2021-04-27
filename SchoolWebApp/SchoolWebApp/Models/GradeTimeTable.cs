using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SchoolWebApp.Models
{
    public class GradeTimeTable
    {
        public int Id { get; set; }

        [Required]
        [DisplayName("الصف")]
        public int GradesId { get; set; }          //Foergin Key
        public virtual Grades Grade { get; set; }

        
        [Required]
        [DisplayName("الجدول")]
        public int TimeTablesId { get; set; }          //Foergin Key
        public virtual TimeTables TimeTable { get; set; }

        [Required]
        [DisplayName("الشعبة")]
        public int SectionsId { get; set; }          //Foergin Key
        public virtual Sections Section { get; set; }

    }
}