using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using WebApplication1.Models;

namespace SchoolWebApp.Models
{
    public class Grades
    {
        public int Id { get; set; } // Primary Key

        [Required]
        [DisplayName("الصف  ")]
        public int ClassGrade { get; set; }

       
        // many News to one type
        public virtual ICollection<GradeTimeTable> GradeTimeTable { get; set; }
        public virtual ICollection<ApplicationUser> StudentClass { get; set; }
        public virtual ICollection<Notes> Notes { get; set; }


    }
}