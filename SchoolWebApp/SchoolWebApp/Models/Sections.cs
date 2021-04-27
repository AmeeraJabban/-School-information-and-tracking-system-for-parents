using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using WebApplication1.Models;

namespace SchoolWebApp.Models
{
    public class Sections
    {
       
            public int Id { get; set; }


            [Required]
            [Display(Name = "الشعبة")]
            public string SectionName { get; set; }

            [Required]
            [Display(Name = "اتاحية الشعبة ")]
            public string Sectionprop { get; set; }
         
        // many News to one type
        public virtual ICollection<GradeTimeTable> GradeTimeTable { get; set; }
        public virtual ICollection<ApplicationUser> StudentSection { get; set; }
        public virtual ICollection<Notes> Notes { get; set; }


    }
}